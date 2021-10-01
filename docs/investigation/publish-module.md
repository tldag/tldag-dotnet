## Publish-Module

September 2021, Roger H. Jörg

[ps-gh]: https://github.com/PowerShell
[psps-gh]: https://github.com/PowerShell/PowerShell
[psg3-gh]: https://github.com/PowerShell/PowerShellGet
[pumo-src]: https://github.com/PowerShell/PowerShellGetv2/blob/master/src/PowerShellGet/public/psgetfunctions/Publish-Module.ps1
[puau-src]: https://github.com/PowerShell/PowerShellGetv2/blob/master/src/PowerShellGet/private/functions/Publish-PSArtifactUtility.ps1
[punp-src]: https://github.com/PowerShell/PowerShellGetv2/blob/master/src/PowerShellGet/private/functions/Publish-NugetPackage.ps1

### Find the source

The call...

```powershell
Get-Command Publish-Module
```

... returns ...

```
CommandType     Name                                               Version    Source
-----------     ----                                               -------    ------
Function        Publish-Module                                     2.2.5      PowerShellGet
```

... revealing that ```Publish-Module``` is actually a function and part of the ```PowerShellGet``` module.

I soon found the source of PowerShell on GitHub, but it doesn't contain ```Publish-Module```.

Further investigation of the [PowerShell Repositories][ps-gh] showed the repository for
[PowerShellGet][psg3-gh]. But that source is for version 3 and doesn't contain ```Publish-Module``` either.

Reading its readme I eventually found the [source][pumo-src].

What a disappointement: that's a script!

### Inner Workings

#### Publish-Module.ps1

- The script is concerned about using ```TLS 1.2```.
- It performs a plethora of validation checks on the parameters and the module manifest.
- It eventually passes the job to the private script [Publish-PSArtifactUtility][puau-src].

#### Publish-PSArtifactUtility.ps1

- Installs NuGet binaries.
- It performs some validation checks.
- It creates the NuGet package based on the manifest file (```.psd1``` file).
- It eventually passes the job to the private script [Publish-NugetPackage][punp-src].

#### Publish-NugetPackage.ps1

- Decides whether to use ```nuget push``` or ```dotnet nuget push```.
- Starts the external process accordingly.

### Conclusions

- A script is a no-go.
- Validating the manifest isn't needed, if the manifest is auto-generated.
- Creating a NuGet package can be done as part of the build process.
- Installing new software on-the-fly is a no-go.
- Calling an external process may be avoided.

### References

- [PowerShell Source][psps-gh]
- [PowerShellGet Source (V2)](https://github.com/PowerShell/PowerShellGetv2)
- [PowerShellGet Source (V3)](https://github.com/PowerShell/PowerShellGet)
