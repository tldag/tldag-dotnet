@{
    RootModule = 'lib/net5.0/TLDAG.Commands.dll'
    ModuleVersion = '0.1.0'
    # CompatiblePSEditions = @()
    GUID = 'B0EAFE79-4DCC-4464-BE7B-B8B47A03E123'
    Author = 'tldag.ch@gmail.com'
    CompanyName = 'The TLDAG Project'
    Copyright = 'Copyright (c) 2021 The TLDAG Project. All rights reserved.'
    Description = 'Various commands.'
    PowerShellVersion = '7.1'
    # PowerShellHostName = ''
    # PowerShellHostVersion = ''
    # DotNetFrameworkVersion = ''
    # ClrVersion = ''
    # ProcessorArchitecture = ''
    # RequiredModules = @()
    # RequiredAssemblies = @()
    # ScriptsToProcess = @()
    # Type files (.ps1xml) to be loaded when importing this module
    # TypesToProcess = @()
    # Format files (.ps1xml) to be loaded when importing this module
    # FormatsToProcess = @()
    # NestedModules = @()
    FunctionsToExport = @()
    CmdletsToExport = @(
        "New-Password",
        "New-Solution"
    )
    VariablesToExport = @()
    AliasesToExport = @()
    # DscResourcesToExport = @()
    # ModuleList = @()
    # FileList = @()
    # HelpInfoURI = ''
    # DefaultCommandPrefix = ''
    PrivateData = @{
        PSData = @{
            Tags = @()
            LicenseUri = 'https://tldag.github.io/tldag-license.html'
            ProjectUri = 'https://tldag.github.io/'
            # IconUri = ''
            Prerelease = ''
            RequireLicenseAcceptance = $false
            ExternalModuleDependencies = @()
            ReleaseNotes = '
0.1.0
- Added command New-Password
- Added command New-Solution
'
        }
    }
}