
Get-Process -Name dotnet -ErrorAction Ignore | Stop-Process
Get-Process -Name msbuild -ErrorAction Ignore | Stop-Process

Get-ChildItem "TestOutput" -ErrorAction Ignore | Remove-Item -Recurse
Get-ChildItem "TestResults" -ErrorAction Ignore | Remove-Item -Recurse

dotnet clean tldag-dotnet.sln -c Release -v m -noLogo
dotnet clean tldag-dotnet.sln -c Debug -v m -noLogo

dotnet build tldag-dotnet.sln -c Release -noLogo
dotnet test tldag-dotnet.sln -c Release -r:False -noLogo
dotnet build tldag-dotnet-samples.sln -c Release -noLogo

Get-Process -Name dotnet -ErrorAction Ignore | Stop-Process
Get-Process -Name msbuild -ErrorAction Ignore | Stop-Process
