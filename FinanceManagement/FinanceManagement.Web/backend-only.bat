::===============================================================

:: backend-only.bat [--configuration <CONFIGURATION>] [--enviroment <ENVIROMENT>]
::    [--no-build]
:: ENVIROMENT: Development, RC, Production
:: CONFIGURATION: Debug, Release, Debug Backend Only, Release Backend Only
	
::===============================================================
@echo off

set willBuild=1
set configuration="Debug Backend Only"
set enviroment="Development"

:Loop

IF "%~1"=="" (
    GOTO Continue
) ELSE IF "%~1"=="--enviroment" (
    IF "%~2"=="" (
		echo Error: Required argument missing for option: --enviroment
        GOTO endparse
    ) ELSE (
        set enviroment="%~2"
    )
) ELSE IF "%~1"=="--configuration" (
    IF "%~2"=="" (
		echo Error: Required argument missing for option: --configuration
        GOTO endparse
    ) ELSE (
        set configuration="%~2"
    )
) ELSE IF "%~1"=="--no-build" (
    set willBuild=0
)
SHIFT
GOTO Loop

:Continue

set enviroment=%enviroment:"=%

IF %willBuild%==1 (
   dotnet build --configuration %configuration% -nowarn:NU1701
)

dotnet run --configuration %configuration% --no-build --launch-profile "dotnet run"

:endparse