set /p ver=<VERSION
set sourceUrl=-s https://www.nuget.org/api/v2/package

dotnet nuget push artifacts/IPSearcher.%ver%.nupkg %sourceUrl%
dotnet nuget push artifacts/IPSearcher.Data.%ver%.nupkg %sourceUrl%
