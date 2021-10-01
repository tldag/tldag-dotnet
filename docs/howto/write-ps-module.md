## Write a binary PowerShell module

[release]: https://github.com/tldag/tldag-dotnet/releases/tag/v0.0.0-alpha
[gallery]: https://www.powershellgallery.com/packages/TLDAG.PS/0.0.1-alpha

### Introduction

> The scripts and examples are based on the ```TLDAG.PS``` project

> You must, of course, adjust certain values to your own project

The following step-by-step instructions have some prerequisites:
- It targets PowerShell 7.1+
- It targets the ```<TargetFramework>net5.0</TargetFramework>```
- It creates an empty but publishable binary module

The code is taken from the [v0.0.0-alpha Release][release]

### 1. Create a project

Create a new C# library project targetting ```net5.0```.

This initial project file looks like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

</Project>
```

Delete the Class1.cs from the project. We create an empty(!) module.

### 2. Version Setup

We define the required version properties to be version ```0.0.1-alpha```. The above property group looks like this:

```xml
  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
    <VersionPrefix>0.0.1.0</VersionPrefix>
    <VersionSuffix>alpha</VersionSuffix>
  </PropertyGroup>
```

The final ```<Version>``` property is derived from ```<VersionPrefix>``` and ```<VersionSuffix>```. 

### 3. Create the manifest

PowerShell has a command to create an empty manifest.

```powershell
New-ModuleManifest TLDAG.PS.psd1
```

Move the generated manifest into the project folder.

The manifest must become part of the project output. Add the following item group to the project file:

```xml
  <ItemGroup>
    <None Include="TLDAG.PS.psd1" CopyToOutputDirectory="Always" />
  </ItemGroup>
```

### 4. Populate the manifest

The following properties in the manifest file need some love:

```powershell
RootModule = 'TLDAG.PS.dll'
ModuleVersion = '0.0.1'
Author = 'tldag.ch@gmail.com'
CompanyName = 'The TLDAG Project'
Copyright = 'Copyright (c) 2021 The TLDAG Project. All rights reserved.'
Description = 'Empty binary module. Proof-of-concept.'
PowerShellVersion = '7.1'
VariablesToExport = @()
PrivateData = @{
    Tags = @()
    LicenseUri = 'https://tldag.github.io/tldag-license.html'
    ProjectUri = 'https://tldag.github.io/'
    ReleaseNotes = 'Empty binary module. Proof-of-concept.'
    Prerelease = 'alpha'
    RequireLicenseAcceptance = $false
    ExternalModuleDependencies = @()
}
```

Properties that need no modification or stay commented are not shown in the example above.

### 5. Build the project

Use your favorite tool to build the project. The following instructions expect the project output to be
in the ```bin\Debug\net5.0``` directory (relative to the project file's directory).


### 6. Prepare the output for publishing

Startup a PowerShell instance and navigate to the directory above the project (thats usually the directory with the solution file in it).

In the case of the ```TLDAG.PS``` project, this is the ```C:\source\tldag-dotnet``` directory.

Test the validity of the manifest:

```powershell
Test-ModuleManifest C:\source\tldag-dotnet\TLDAG.PS\bin\Debug\net5.0\TLDAG.PS.psd1
```

> It took me hours and gave me a lot of frustration to find out the following fact:

To invoke ```Publish-Module``` with a ```Path``` parameter, the name of directory containing the module to publish MUST match the module name!

Make a copy of the output directory ```bin\Debug\net5.0```. Name it ```bin\Debug\TLDAG.PS```.

Create a ```modules``` directory within the solution directory. This is ```C:\source\tldag-dotnet\modules``` in the example we're looking at.

Designate the ```modules``` directory as valid target for publishing:

```powershell
Register-PSRepository -Name 'Modules' -InstallationPolicy Trusted -SourceLocation 'C:\source\tldag-dotnet\modules' -PublishLocation 'C:\source\tldag-dotnet\modules'
```

Check the repositories:

```powershell
Get-PSRepository
```

The output looks like this in my case:

```
Name                      InstallationPolicy   SourceLocation
----                      ------------------   --------------
Modules                   Trusted              C:\source\tldag-dotnet\modules
PSGallery                 Trusted              https://www.powershellgallery.com/api/v2
```

### 7. Publish the module to the local repository

```powershell
Publish-Module -Path 'C:\source\tldag-dotnet\TLDAG.PS\bin\Debug\TLDAG.PS' -Repository 'Modules' -NuGetApiKey '12345' -Force
```

The ```-NuGetApiKey``` must be given, but may contain any non-empty string.

The ```-Force``` switch is used to overwrite a previously published module of the same name

The command performs some tests and finally creates the ```TLDAG.PS.0.0.1-alpha.nupkg``` package file in the ```modules``` directory.
A simple ```dotnet publish``` would probably suffice.

### 8. Uninstall existing module

If you have an older version of your module installed, the following sample uninstalls it. Without uninstalling step 9 will fail.

```powershell
Uninstall-Module 'TLDAG.PS'
```

### 9. Install the module locally

```powershell
Install-Module -Name 'TLDAG.PS' -Repository 'Modules' -Scope CurrentUser
```

Installs your new module for the current user.

### 10. Publish to PSGallery

```powershell
Publish-Module -Name 'TLDAG.PS' -Repository PSGallery -NuGetApiKey $env:TLDAG_PSGALLERY_KEY
```

Installs this empty module in the PowerShell Gallery. See the [Gallery Entry][gallery] of our empty sample.

Of course you have to have or create an account at the PowerShell Gallery, create an appropriate API key and store this
key as an environment variable (```TLDAG_PSGALLERY_KEY``` in our example).
