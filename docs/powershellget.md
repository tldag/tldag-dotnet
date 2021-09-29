## PowerShellGet

September 2021, Roger H. Jörg

### Introduction

In addition to the ```PowerShellGet``` modules found earlier on GitHub, there is a module with the same name
installed with PowerShell 7.1. It is located at ```C:\Program Files\PowerShell\7\Modules\PowerShellGet``` on my
computer.

### Inspection

This version of ```PowerShellGet``` is (again) one large PowerShell script.

The ```PowerShellGet.psd1``` file reveals the following functions to be exported:

```Find-Command```, ```Find-DSCResource```, ```Find-Module```, ```Find-RoleCapability```, ```Find-Script```, ```Get-CredsFromCredentialProvider```,
```Get-InstalledModule```, ```Get-InstalledScript```, ```Get-PSRepository```, ```Install-Module```, ```Install-Script```,
```New-ScriptFileInfo```, ```Publish-Module```, ```Publish-Script```, ```Register-PSRepository```, ```Save-Module```, ```Save-Script```,
```Set-PSRepository```, ```Test-ScriptFileInfo```, ```Uninstall-Module```, ```Uninstall-Script```, ```Unregister-PSRepository```,
```Update-Module```, ```Update-ModuleManifest```, ```Update-Script```, ```Update-ScriptFileInfo```.

Looking at this list, this somehow interferes with the previously inspected ```PowerShellGet``` modules found on GitHub.

The main implementation of the module is in the ```PSModule.psm1``` file. This file is 16'000+ lines of code!

#### Gallery Info

```powerhell
$Script:PSGalleryModuleSource = "PSGallery"
$Script:PSGallerySourceUri = 'https://www.powershellgallery.com/api/v2'
$Script:PSGalleryPublishUri = 'https://www.powershellgallery.com/api/v2/package/'
$Script:PSGalleryScriptSourceUri = 'https://www.powershellgallery.com/api/v2/items/psscript'
$Script:PSGalleryV3SourceUri = 'https://www.powershellgallery.com/api/v3'
```

The gallery supports NuGet V3 API.

### Get-PSRepository

We end up calling ```PackageManagement\Get-PackageSource```, so that package needs to be investigated.
