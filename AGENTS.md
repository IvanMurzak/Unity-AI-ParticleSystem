# AGENTS.md — Unity-AI-ParticleSystem

Unity package `com.ivanmurzak.unity.mcp.particlesystem` — makes Unity `ParticleSystem` components available to
Unity-MCP's RQL tools (`rql-query` / `rql-mutate`; no extension MCP tools): writable module structs, MinMaxCurve /
MinMaxGradient literals and playback methods.
An extension of [AI Game Developer](https://github.com/IvanMurzak/Unity-MCP) (`com.ivanmurzak.unity.mcp`),
generated from `Unity-AI-Tools-Template`; the user-facing tool list is in `README.md`.

## Layout

- `Unity-Package/` — Unity project hosting the package; only
  `Packages/com.ivanmurzak.unity.mcp.particlesystem/` ships.
  - `package.json` — the version source of truth and the dependency pins.
  - `Editor/Scripts/Rql/` — the RQL module (`ParticleSystemModule`: adapters, guide), one adapter per file
    (`<Namespace_Type>_Adapter.cs`); new operations are adapter methods.
  - `Runtime/` — runtime assembly (currently empty).
  - `Tests/Editor/Rql/` (EditMode, `ParticleTestBase` + one file per adapter / module) and `Tests/Runtime/` (PlayMode).
- `Unity-Tests/<unity-version>/` — one test project per supported Unity version, consuming the package via
  `file:`. A new version must also be added to `test_pull_request.yml` and `release.yml` (and
  `open-all-projects-*`).
- `Installer/` — Unity project that exports the `.unitypackage` installer
  (`Assets/com.IvanMurzak/<name> Installer/`).
- `commands/` — PowerShell helpers (below) and `check-versions.py` (synced in by the release cascade — do not
  hand-edit; CI runs it as the `version-consistency` gate).
- `.github/workflows/` — `test_pull_request.yml` (PR test matrix), `test_unity_plugin.yml` (reusable test
  job), `release.yml`, `bump_version.yml`, `claude.yml` (`@claude` mentions).
- `docs/` — user docs: `Deploy-{OpenUPM,GitHub,npmjs}.md`, `Manual-Package-Rename.md`, `openupm-signing.md`,
  `img/`.

## Commands (PowerShell, from the repo root)

```powershell
./commands/bump-version.ps1 -NewVersion "x.y.z"   # package.json + Installer.cs in lock-step; -WhatIf to preview
./commands/get-version.ps1
./commands/update-ai-game-developer.ps1           # bump the com.ivanmurzak.unity.mcp pin; -WhatIf to preview
./commands/open-all-projects-windows.ps1          # open all Unity projects (or -unix.sh)
./commands/use-local-mcp.ps1 [-Local|-Remote]      # point the Unity-MCP dep at a local checkout / back to OpenUPM
```

Tests run inside the Unity Editor (NUnit + `[UnityTest]`); CI runs them with `game-ci/unity-test-runner` in
EditMode / PlayMode / Standalone for every Unity version. `release.yml` releases on push to `main` only when
the `package.json` version has no tag yet (installer, signed UPM tarball for OpenUPM, test matrix, tag, GitHub
Release); dispatch `bump_version.yml` to bump in CI. Under `pull_request_target`, tests need the
maintainer-applied `ci-ok` label and abort if the PR touches `.github/workflows/`.

## Critical invariants

- **Main thread only.** RQL runs on the main thread (Unity-MCP dispatches the RQL tools there).
- **RQL surface.** Everything is registered by `ParticleSystemModule` (`IRqlModule`, Order 100, found through
  TypeCache) and documented in its `AddGuide("Particle systems", …)` text, which is rendered into the `unity-rql` skill.
- **Module structs are handles.** `ps.main`, `ps.emission` … are get-only struct properties whose setters write to the
  native particle system; `UnityEngine_ParticleSystem_ModuleStruct_Adapter` marks them writes-through so
  `set main.startLifetime = 3` works.
- **Name clash.** Inside `com.IvanMurzak.Unity.MCP.*` namespaces, alias the type inside the namespace block
  (`using ParticleSystem = UnityEngine.ParticleSystem;`).
- Every C# file starts with the copyright box header and `#nullable enable`.
