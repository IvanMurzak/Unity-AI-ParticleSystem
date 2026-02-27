# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Unity AI Particle System** is a Unity package (`com.ivanmurzak.unity.mcp.particlesystem`) that exposes two MCP tools — `particle-system-get` and `particle-system-modify` — enabling AI agents to inspect and modify Unity `ParticleSystem` components via natural language. It is built on top of the [AI Game Developer (Unity-MCP)](https://github.com/IvanMurzak/Unity-MCP) platform.

## Repository Structure

```
Unity-AI-ParticleSystem/
├── Unity-Package/          # Main Unity project containing the UPM package
│   └── Assets/root/        # The actual package contents (published to OpenUPM)
│       ├── Editor/Scripts/
│       │   ├── Tools/      # MCP tool implementations (partial classes)
│       │   └── Data/       # Response/data models
│       ├── Runtime/        # Runtime assembly (currently empty placeholder)
│       └── Tests/Editor/   # NUnit/UnityTest editor tests
├── Installer/              # Separate Unity project that exports the .unitypackage installer
├── commands/               # PowerShell utility scripts
└── .github/workflows/      # CI/CD pipelines
```

## Key Architectural Patterns

### MCP Tool Registration
Tools are registered via `[McpPluginToolType]` on the class and `[McpPluginTool("tool-id")]` on methods. The entry class `Tool_ParticleSystem` is split across partial classes:
- `ParticleSystem.cs` — bare class with `[McpPluginToolType]`
- `ParticleSystem.Get.cs` — implements `particle-system-get`
- `ParticleSystem.Modify.cs` — implements `particle-system-modify`

### Serialization via ReflectorNet
Module data is read/written using `UnityMcpPluginEditor.Instance.Reflector`:
- `reflector.Serialize(obj, name, recursive, logger)` — converts Unity structs to `SerializedMember`
- `reflector.TryPopulate(ref boxedModule, serializedMember, logs, logger)` — applies partial updates back to Unity structs

All module interactions are dispatched to the Unity main thread via `MainThread.Instance.Run(...)`.

### Data Models
- `ParticleSystemData` — holds one `SerializedMember?` per module (24 modules + renderer)
- `GetParticleSystemResponse` / `ModifyParticleSystemResponse` — response types returned by the tools
- `GameObjectRef` — identifies a GameObject by instanceID, name, or path
- `ComponentRef` — optionally pinpoints a specific component on a multi-component GameObject

### Dependency
The package depends on `com.ivanmurzak.unity.mcp` (the Unity-MCP platform). To update this dependency to the latest release:
```powershell
.\commands\update-ai-game-developer.ps1
# Preview only:
.\commands\update-ai-game-developer.ps1 -WhatIf
```

## Version Management

Version is tracked in `Unity-Package/Assets/root/package.json`. To bump the version across all files (package.json, Installer constant, README download URLs):
```powershell
.\commands\bump-version.ps1 -NewVersion "1.0.23"
# Preview only:
.\commands\bump-version.ps1 -NewVersion "1.0.23" -WhatIf
```

Files updated by `bump-version.ps1`:
- `Unity-Package/Assets/root/package.json`
- `Installer/Assets/com.IvanMurzak/AI Particle System Installer/Installer.cs`
- `Unity-Package/Assets/root/README.md`
- `README.md`

## CI/CD

**`release.yml`** — triggers on push to `main`:
1. Reads version from `Unity-Package/Assets/root/package.json`
2. Skips if git tag for that version already exists
3. Builds the `.unitypackage` installer via `com.IvanMurzak.Unity.MCP.ParticleSystem.Installer.PackageExporter.ExportPackage`
4. Runs Unity tests (editmode, playmode, standalone) across Unity 2022.3.62f3, 2023.2.22f1, and 6000.3.1f1
5. Creates a GitHub release and uploads `AI-ParticleSystem-Installer.unitypackage`

**`test_unity_plugin.yml`** — reusable workflow; requires `ci-ok` label on PRs before running with secrets.

## Tests

Tests are Unity NUnit tests under `Unity-Package/Assets/root/Tests/Editor/`. They use `[UnityTest]` (coroutines) and inherit from `BaseTest`, which provides:
- `CreateGameObjectWithParticleSystem(name)` — creates a test GameObject with a ParticleSystem
- `RunToolAllowWarnings(toolName, json)` — invokes a tool via JSON and asserts no errors

Tests run inside Unity Editor (not standalone CLI). In CI they run via `game-ci/unity-test-runner@v4` with `customParameters: -CI true -GITHUB_ACTIONS true`.

## Assembly Definitions

| Assembly | Purpose |
|---|---|
| `com.IvanMurzak.Unity.MCP.ParticleSystem.Editor` | Editor tools (Tools + Data) |
| `com.IvanMurzak.Unity.MCP.ParticleSystem.Runtime` | Runtime (placeholder) |
| `com.IvanMurzak.Unity.MCP.ParticleSystem.Editor.Tests` | Editor test assembly |
| `com.IvanMurzak.Unity.MCP.ParticleSystem.Tests` | Runtime test assembly |
