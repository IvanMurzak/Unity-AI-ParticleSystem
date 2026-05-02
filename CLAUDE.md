# CLAUDE.md

## What this is

Unity package `com.ivanmurzak.unity.mcp.particlesystem` exposing two MCP tools — `particle-system-get` and `particle-system-modify` — so AI agents can inspect and modify Unity `ParticleSystem` components. Built on the Unity-MCP platform.

## Build / run

- Package source: `Unity-Package/Assets/root/` (Editor + Runtime + Tests)
- Update Unity-MCP dependency: `.\commands\update-ai-game-developer.ps1` (`-WhatIf` to preview)
- Bump version: `.\commands\bump-version.ps1 -NewVersion "x.y.z"` (`-WhatIf` to preview)
- Tests run inside Unity Editor (NUnit + `[UnityTest]`); CI uses `game-ci/unity-test-runner@v4`.

## Critical invariants

- **Main thread only.** All Unity module reads/writes MUST be dispatched via `MainThread.Instance.Run(...)`. ReflectorNet calls (`reflector.Serialize`, `reflector.TryPopulate`) touch Unity structs and must not run off the main thread.
- Tools are partial classes on `Tool_ParticleSystem` — one op per file (`ParticleSystem.Get.cs`, `ParticleSystem.Modify.cs`).

## Find detail in

- `docs/claude/architecture.md` — repo structure, MCP tool registration, ReflectorNet usage, assemblies, tests
- `docs/claude/models.md` — `ParticleSystemData`, response types, `GameObjectRef`, `ComponentRef`
- `docs/claude/release.md` — `bump-version.ps1` and files it touches
- `docs/claude/ci.md` — `release.yml`, `test_unity_plugin.yml`, Unity version matrix
