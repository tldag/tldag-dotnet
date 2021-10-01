## Configure NuGet

September 2021, Roger H. Jörg

### Introduction

To have a clean cache of NuGet packages on a build server, it is better to store the packages in a directory local to the solution.

This also allows to easily inspect the contents of downloaded or self-created packages.

### Solution

Create a ```NuGet.config``` file at the solution level with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="globalPackagesFolder" value="packages" />
    <add key="repositoryPath" value="packages" />
  </config>
  <packageSources>
    <clear />
    <add key="Nuget" value="https://api.nuget.org/v3/index.json" validated="True" trusted="True" />
  </packageSources>
  <fallbackPackageFolders>
    <clear />
  </fallbackPackageFolders>
  <disabledPackageSources>
    <clear />
  </disabledPackageSources>
</configuration>
```

The ```<config>``` element controls both, the location of the "gobal packages" as well as the the location where to create the packages in.

The ```<clear />``` elements within the other elements assure that you don't inherit any settings of some user or machine level settings file.
