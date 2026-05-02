# CI/CD

**`release.yml`** — triggers on push to `main`:
1. Reads version from `Unity-Package/Assets/root/package.json`
2. Skips if git tag for that version already exists
3. Builds the `.unitypackage` installer via `com.IvanMurzak.Unity.MCP.ParticleSystem.Installer.PackageExporter.ExportPackage`
4. Runs Unity tests (editmode, playmode, standalone) across Unity 2022.3.62f3, 2023.2.22f1, and 6000.3.1f1
5. Creates a GitHub release and uploads `AI-ParticleSystem-Installer.unitypackage`

**`test_unity_plugin.yml`** — reusable workflow; requires `ci-ok` label on PRs before running with secrets.
