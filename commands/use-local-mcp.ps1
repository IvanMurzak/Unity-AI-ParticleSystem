#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Toggles com.ivanmurzak.unity.mcp between local file reference and OpenUPM version

.DESCRIPTION
    Switches the Unity-MCP dependency in manifest.json between a local file: reference
    (for development within ai-game-developer-infra workspace) and the OpenUPM version
    (for production / GitHub distribution).

.PARAMETER Local
    Switch to local file: reference pointing to Unity-MCP submodule

.PARAMETER Remote
    Restore OpenUPM version-pinned reference

.PARAMETER WhatIf
    Preview changes without applying them

.EXAMPLE
    .\use-local-mcp.ps1
    # Shows current state (local or remote)

.EXAMPLE
    .\use-local-mcp.ps1 -Local

.EXAMPLE
    .\use-local-mcp.ps1 -Remote

.EXAMPLE
    .\use-local-mcp.ps1 -Local -WhatIf
#>

param(
    [switch]$Local,
    [switch]$Remote,
    [string]$Version,
    [switch]$WhatIf
)

# Set location to repository root (parent of commands folder)
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
Push-Location $repoRoot

# Script configuration
$ErrorActionPreference = "Stop"
$PackageName = "com.ivanmurzak.unity.mcp"
$ManifestPath = "Unity-Package/Packages/manifest.json"
$PackageJsonPath = "Unity-Package/Assets/root/package.json"
$LocalFilePath = "file:./../../../../Unity-MCP/Unity-MCP-Plugin/Assets/root"

function Write-ColorText {
    param([string]$Text, [string]$Color = "White")
    Write-Host $Text -ForegroundColor $Color
}

function Get-CurrentReference {
    param([string]$FilePath, [string]$PackageName)

    if (-not (Test-Path $FilePath)) {
        return $null
    }

    $content = Get-Content $FilePath -Raw
    $pattern = [regex]::Escape("`"$PackageName`"") + ':\s*"([^"]+)"'

    if ($content -match $pattern) {
        return $Matches[1]
    }

    return $null
}

function Get-VersionFromPackageJson {
    param([string]$FilePath, [string]$PackageName)

    if (-not (Test-Path $FilePath)) {
        return $null
    }

    $json = Get-Content $FilePath -Raw | ConvertFrom-Json
    if ($json.dependencies.PSObject.Properties[$PackageName]) {
        return $json.dependencies.$PackageName
    }

    return $null
}

function Set-Reference {
    param(
        [string]$FilePath,
        [string]$PackageName,
        [string]$NewValue,
        [bool]$PreviewOnly = $false
    )

    $content = Get-Content $FilePath -Raw
    $originalContent = $content

    $pattern = '("' + [regex]::Escape($PackageName) + '":\s*")[^"]+"'
    $replacement = '${1}' + $NewValue + '"'

    $newContent = $content -replace $pattern, $replacement

    if ($originalContent -eq $newContent) {
        Write-ColorText "   No changes needed in: $FilePath" "Gray"
        return $false
    }

    if (-not $PreviewOnly) {
        Set-Content -Path $FilePath -Value $newContent -NoNewline
    }

    return $true
}

# Main execution
try {
    Write-ColorText "Toggle Local/Remote MCP Dependency" "Cyan"
    Write-ColorText "=====================================" "Cyan"

    if (-not (Test-Path $ManifestPath)) {
        throw "Manifest file not found: $ManifestPath"
    }

    # Detect current state
    $currentRef = Get-CurrentReference -FilePath $ManifestPath -PackageName $PackageName

    if ($null -eq $currentRef) {
        throw "Package '$PackageName' not found in $ManifestPath"
    }

    $isLocal = $currentRef.StartsWith("file:")
    $stateLabel = if ($isLocal) { "LOCAL ($currentRef)" } else { "REMOTE ($currentRef)" }
    Write-ColorText "Current state: $stateLabel" "White"

    # If no flags provided, just show status
    if (-not $Local -and -not $Remote) {
        Write-ColorText "`nUsage: -Local to switch to local, -Remote to switch to remote" "Gray"
        Pop-Location
        exit 0
    }

    # Validate flags
    if ($Local -and $Remote) {
        throw "Cannot specify both -Local and -Remote"
    }

    if ($Local) {
        if ($isLocal) {
            Write-ColorText "`nAlready using local reference." "Yellow"
            Pop-Location
            exit 0
        }

        Write-ColorText "`nSwitching to LOCAL reference..." "Cyan"
        Write-ColorText "   Target: $LocalFilePath" "Gray"

        $changed = Set-Reference -FilePath $ManifestPath -PackageName $PackageName -NewValue $LocalFilePath -PreviewOnly $WhatIf

        if ($WhatIf) {
            Write-ColorText "`nPreview: Would change '$currentRef' -> '$LocalFilePath'" "White"
            Write-ColorText "Run without -WhatIf to apply changes." "Green"
        }
        elseif ($changed) {
            Write-ColorText "`nSwitched to LOCAL reference." "Green"
            Write-ColorText "   Remember to run -Remote before committing!" "Cyan"
        }
    }

    if ($Remote) {
        if (-not $isLocal) {
            Write-ColorText "`nAlready using remote reference." "Yellow"
            Pop-Location
            exit 0
        }

        # Determine version to restore
        if (-not $Version) {
            $Version = Get-VersionFromPackageJson -FilePath $PackageJsonPath -PackageName $PackageName
        }

        if (-not $Version) {
            throw "Could not determine version. Specify -Version parameter or ensure $PackageJsonPath has the dependency."
        }

        Write-ColorText "`nSwitching to REMOTE reference..." "Cyan"
        Write-ColorText "   Version: $Version" "Gray"

        $changed = Set-Reference -FilePath $ManifestPath -PackageName $PackageName -NewValue $Version -PreviewOnly $WhatIf

        if ($WhatIf) {
            Write-ColorText "`nPreview: Would change '$currentRef' -> '$Version'" "White"
            Write-ColorText "Run without -WhatIf to apply changes." "Green"
        }
        elseif ($changed) {
            Write-ColorText "`nSwitched to REMOTE reference (version $Version)." "Green"
        }
    }

    Pop-Location
}
catch {
    Write-ColorText "`nScript failed: $($_.Exception.Message)" "Red"
    Pop-Location
    exit 1
}
