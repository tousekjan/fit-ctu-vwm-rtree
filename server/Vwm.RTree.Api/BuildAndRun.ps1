param(
[switch] $noBuild)

if(!$noBuild)
{
   dotnet build --configuration Release
}
dotnet exec .\bin\Release\netcoreapp2.2\Vwm.RTree.Api.dll