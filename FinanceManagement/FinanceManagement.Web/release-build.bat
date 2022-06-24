@echo off

set enviroment=%enviroment:"=%

dotnet build --configuration Release

:endparse