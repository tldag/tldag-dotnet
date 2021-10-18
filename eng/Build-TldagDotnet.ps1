
Get-Process -Name dotnet | Stop-Process

dotnet clean tldag-dotnet.sln -c Release -v m -noLogo
dotnet clean tldag-dotnet.sln -c Debug -v m -noLogo

dotnet build tldag-dotnet.sln -c Release -noLogo
dotnet test tldag-dotnet.sln -c Release -r:False -noLogo
dotnet build tldag-dotnet-samples.sln -c Release -noLogo
