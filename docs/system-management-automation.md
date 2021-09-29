## System.Management.Automation

September 2021, Roger H. Jörg

### Investigation

After adding several more packages:

```xml
<ItemGroup>
    <PackageReference Include="System.Management.Automation" Version="7.1.4" />
    <PackageReference Include="Microsoft.PowerShell.Commands.Diagnostics" Version="7.1.4" />
    <PackageReference Include="Microsoft.PowerShell.ConsoleHost" Version="7.1.4" />
    <PackageReference Include="Microsoft.PowerShell.Commands.Utility" Version="7.1.4" />
    <PackageReference Include="Microsoft.PowerShell.Commands.Management" Version="7.1.4" />
    <PackageReference Include="Microsoft.WSMan.Management" Version="7.1.4" />
  </ItemGroup>
```

... I was able to call:

```c#
    PowerShell shell = PowerShell.Create();

    shell.AddScript("Get-PackageProvider");
    //shell.AddScript("Get-PackageSource -ProviderName 'PowerShellGet'");

    Collection<PSObject> result = shell.Invoke();
```

... just to get an empty array as a result.

### Findings

This library supports the ```net5.0``` target framework only. Therefore it will not be available in custom tasks.

I haven't found yet an access point to the required information.

### References

- [System.Management.Automation Namespace](https://docs.microsoft.com/en-us/dotnet/api/system.management.automation)

