@echo off
cd %~dp0

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe
echo Set DNX feed to NuGet v3 (stable)
SET DNX_FEED=https://www.nuget.org/api/v2

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
IF EXIST packages\Sake goto getdnx
.nuget\NuGet.exe install KoreBuild -Source https://www.myget.org/F/aspnetmaster/api/v2 -o packages -nocache -pre -ExcludeVersion
.nuget\NuGet.exe install Sake -Source https://www.nuget.org/api/v2 -o packages -ExcludeVersion

:getdnx
CALL packages\KoreBuild\build\dnvm install latest -runtime CoreCLR -arch x86 -alias default
CALL packages\KoreBuild\build\dnvm install default -runtime CLR -arch x86 -alias default

packages\Sake\tools\Sake.exe -I packages\KoreBuild\build -f makefile.shade %*
