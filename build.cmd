set artifacts=%~dp0artifacts

if exist %artifacts%  rd /q /s %artifacts%

call dotnet restore src/IPSearcher

call dotnet build src/IPSearcher -f net40 -c Release -o %artifacts%\net40
call dotnet build src/IPSearcher -f netstandard2.0 -c Release -o %artifacts%\netstandard2.0


call dotnet build src/IPSearcher.Data -f net40 -c Release -o %artifacts%\net40
call dotnet build src/IPSearcher.Data -f netstandard2.0 -c Release -o %artifacts%\netstandard2.0

call dotnet pack src/IPSearcher -c release -o %artifacts%
call dotnet pack src/IPSearcher.Data -c release -o %artifacts%
