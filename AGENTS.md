# AGENTS.md — Unity-AI-ParticleSystem

Unity package `com.ivanmurzak.unity.mcp.particlesystem` — MCP tools `particle-system-get` and
`particle-system-modify`, which let AI inspect and modify Unity `ParticleSystem` components module by module.
An extension of [AI Game Developer](https://github.com/IvanMurzak/Unity-MCP) (`com.ivanmurzak.unity.mcp`),
generated from `Unity-AI-Tools-Template`; the user-facing tool list is in `README.md`.

## Layout

- `Unity-Package/` — Unity project hosting the package; only
  `Packages/com.ivanmurzak.unity.mcp.particlesystem/` ships.
  - `package.json` — the version source of truth and the dependency pins.
  - `Editor/Scripts/Tools/` — the MCP tools (`Tool_ParticleSystem`); new tools go here.
  - `Editor/Scripts/Data/` — `ParticleSystemData` (one `SerializedMember?` per module + renderer) and the
    `Get…` / `Modify…Response` types.
  - `Runtime/` — runtime assembly (currently empty).
  - `Tests/Editor/` (EditMode; `BaseTest` provides `CreateGameObjectWithParticleSystem` and
    `RunToolAllowWarnings`) and `Tests/Runtime/` (PlayMode).
- `Unity-Tests/<unity-version>/` — one test project per supported Unity version, consuming the package via
  `file:`. A new version must also be added to `test_pull_request.yml`, `release.yml` and
  `open-all-projects-*`.
- `Installer/` — Unity project that exports the `.unitypackage` installer
  (`Assets/com.IvanMurzak/<name> Installer/`).
- `commands/` — the scripts below, plus `open-all-projects-unix.sh` and `check-versions.py` (synced in by the
  release cascade — do not hand-edit; CI runs it as the `version-consistency` gate).
- `.github/workflows/` — `test_pull_request.yml` (PR test matrix), `test_unity_plugin.yml` (reusable test
  job), `release.yml`, `bump_version.yml`, `claude.yml` (`@claude` mentions).
- `docs/` — user docs: `Deploy-{OpenUPM,GitHub,npmjs}.md`, `Manual-Package-Rename.md`, `openupm-signing.md`,
  `img/`.

## Commands (PowerShell, from the repo root)

```powershell
./commands/bump-version.ps1 -NewVersion "x.y.z"   # package.json + Installer.cs in lock-step; -WhatIf to preview
./commands/get-version.ps1
./commands/update-ai-game-developer.ps1           # bump the com.ivanmurzak.unity.mcp pin; -WhatIf to preview
./commands/open-all-projects-windows.ps1          # open the Unity projects (generates .meta files)
```

Tests run inside the Unity Editor (NUnit + `[UnityTest]`); CI runs them with `game-ci/unity-test-runner` in
EditMode / PlayMode / Standalone for every Unity version. `release.yml` releases on push to `main` only when
the `package.json` version has no tag yet (installer, signed UPM tarball for OpenUPM, test matrix, tag, GitHub
Release); dispatch `bump_version.yml` to bump in CI. Under `pull_request_target`, tests need the
maintainer-applied `ci-ok` label and abort if the PR touches `.github/workflows/`.

## Critical invariants

- **Main thread only.** Every Unity API call inside a tool MUST be wrapped in
  `MainThread.Instance.Run(() => { ... })` — MCP calls arrive off the main thread; only argument validation
  runs outside. So must ReflectorNet calls (`reflector.Serialize`, `TryModify`).
- **Tool shape.** `[AiToolType] partial class Tool_ParticleSystem` (namespace
  `com.IvanMurzak.Unity.MCP.ParticleSystem.Editor`), one operation per file: `ParticleSystem.cs` (bare class),
  `ParticleSystem.Get.cs`, `ParticleSystem.Modify.cs`. Each tool method carries `[AiTool(<id>, …)]`,
  `[AiSkillDescription]` / `[AiSkillBody]` and `[Description]`.
- **Module data goes through ReflectorNet** (`UnityMcpPluginEditor.Instance.Reflector`):
  `reflector.Serialize(...)` reads a module into a `SerializedMember`;
  `reflector.TryModify(ref boxedModule, member, …)` applies a partial update back. Targets are addressed with
  `GameObjectRef` and an optional `ComponentRef` (for GameObjects with several ParticleSystems).
- Every C# file starts with the copyright box header and `#nullable enable`.
