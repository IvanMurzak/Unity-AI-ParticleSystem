# Version Management

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
