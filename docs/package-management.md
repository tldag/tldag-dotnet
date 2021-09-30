## PackageManagement

September 2021, Roger H. Jörg

### Finding the Module

I found the module installed locally at ```C:\Program Files\PowerShell\7\Modules\PackageManagement```. I didn't look for
the online source of it.

### Inspection

The module is mixed this time. It's ```RootModule``` is ```PackageManagement.psm1``` but it contains some DLLs not mentioned
in the ```psd1``` file.

It exports the Cmdlets ```Find-Package```, ```Get-Package```, ```Get-PackageProvider```, ```Get-PackageSource```,
```Install-Package```, ```Import-PackageProvider```, ```Find-PackageProvider```, ```Install-PackageProvider```,
```Register-PackageSource```, ```Set-PackageSource```, ```Unregister-PackageSource```, ```Uninstall-Package``` and ```Save-Package```.

The root module (```PackageManagement.psm1```) detects which binaries to use and imports two of them via ```Import-Module```.
These modules are:
- Microsoft.PackageManagement.dll
- Microsoft.PowerShell.PackageManagement.dll

### Conclusions

Further inspection of the above two modules is required.
