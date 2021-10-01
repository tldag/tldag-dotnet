## Get-PSRepository

[get-psrepository-src]: https://github.com/PowerShell/PowerShellGetv2/blob/master/src/PowerShellGet/public/psgetfunctions/Get-PSRepository.ps1
[partone-src]: https://github.com/PowerShell/PowerShellGetv2/blob/master/src/PowerShellGet/private/modulefile/PartOne.ps1
[get-packagesource-doc]: https://docs.microsoft.com/en-us/powershell/module/packagemanagement/get-packagesource?view=powershell-7.1
[psg-psd1]: https://github.com/PowerShell/PowerShellGetv2/blob/master/src/PowerShellGet/PowerShellGet.psd1

September 2021, Roger H. Jörg

The [source][get-psrepository-src] of this command is again a script.

### Inner Workings

After setting some parameters, it forwards the job to ```Get-PackageSource```.

The parameters passed to ```Get-PackageSource``` are read from a private [script][partone-src].
The values are:

```powershell
$script:PSModuleProviderName = 'PowerShellGet'
$script:PackageManagementMessageResolverScriptBlock = {
    param($i, $Message)
    return (PackageManagementMessageResolver -MsgId $i, -Message $Message)
}
```

These values are passed as parameters ```Provider``` and ```MessageResolver``` to ```Get-PackageSource```

A quick look at the [documentation][get-packagesource-doc] of the current ```Get-PackageSource``` shows, that the parameter names do not match.

Inspection of the [manifest][psg-psd1] shows that ```PackageManagement``` version ```1.4.4``` is used. Most probably an out-dated version.

### Conclusions

Further investigation of ```PackageManagement/1.4.4``` is required. Maybe the PowerShell package ```System.Management.Automation``` can help us out.

### References

- [PowerShellGet Source (V2)](https://github.com/PowerShell/PowerShellGetv2)
- [System.Management.Automation](https://docs.microsoft.com/en-us/dotnet/api/system.management.automation)

