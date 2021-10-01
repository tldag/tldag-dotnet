## Where are NuGet.Config

October 2021, Roger H. Jörg

[FileSearch]: https://github.com/tldag/tldag-dotnet/blob/main/TLDAG.Libraries.Core/IO/FileSearch.cs
[FindAllNugetConfigs]: https://github.com/tldag/tldag-dotnet/blob/main/TLDAG.Libraries.NuGet.Tests/FindAllNugetConfigs.cs

### Introduction

To find where NuGet and PowerShell get there information, I needed to find all occurences of ```NuGet.Config``` on my machine.

I wrote two small classes ([FileSearch][FileSearch] and [FindAllNugetConfigs][FindAllNugetConfigs]). The test runs for about
one minute and therefore is (to be) excluded from the release build.

### Conclusions

Besides occurrences within the ```source``` tree of my local development, the only NuGet.Config on my machine was:

```
C:\Users\rhjoe\AppData\Roaming\NuGet\NuGet.Config
```

On windows, this is equivalent to:

```
%appdata%\NuGet\NuGet.Config
```
