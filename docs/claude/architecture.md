# Architecture

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

### Dependency
The package depends on `com.ivanmurzak.unity.mcp` (the Unity-MCP platform). To update this dependency to the latest release:
```powershell
.\commands\update-ai-game-developer.ps1
# Preview only:
.\commands\update-ai-game-developer.ps1 -WhatIf
```

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
