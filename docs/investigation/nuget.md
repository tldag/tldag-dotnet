## NuGet Libraries

October 2021, Roger H. Jörg

### Introduction

The two packages ```NuGet.Packaging``` and ```NuGet.Protocol``` seem to fulfill the needs for my tasks and commands.

### Some Sample Code

The current (as of this writing) ```NuGet.config``` file contains the following section:

```xml
  <packageSources>
    <clear />
    <add key="Nuget" value="https://api.nuget.org/v3/index.json" validated="True" trusted="True" />
    <add key="Packages" value="file:///C:/source/tldag-dotnet/packages" />
  </packageSources>
```

The following code snippet successfully found the ```TLDAG.Sdk``` package that is created during the build of the project.

```c#
    ILogger logger = NullLogger.Instance;
    CancellationToken cancel = CancellationToken.None;
    SourceCacheContext cache = new SourceCacheContext();
    string root = Environment.CurrentDirectory;
    ISettings settings = Settings.LoadDefaultSettings(root);
    PackageSourceProvider provider = new(settings);
    PackageSource packageSource = provider.GetPackageSourceByName("Packages");
    SourceRepository repository = Repository.Factory.GetCoreV3(packageSource.Source);
    FindPackageByIdResource resource = repository.GetResource<FindPackageByIdResource>();
    IEnumerable<NuGetVersion> versions = resource.GetAllVersionsAsync("TLDAG.Sdk", cache, logger, cancel).Result;

    foreach (NuGetVersion version in versions) Debug.WriteLine(version);
```

### Conclusions

- Works as expected.
- Supports the required target frameworks
- Comes with a clean SemVer implementation

### References

- [NuGet.Protocol documentation](https://docs.microsoft.com/en-us/nuget/reference/nuget-client-sdk)

