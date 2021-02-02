#####
# This script downloads the specified version of NuGet packages for 
# the required smoke tests from NuGet.
# This is useful for performing current analysis on prior builds of the Toolkit.
#
# Pass it a Version Number of the Package you'd like to download, otherwise it'll download the LKG.
# Defaults download to ../bin/nupkg/ directory
####

param (
    [string] $Version = '',
    [string] $DownloadPath = '../bin/nupkg/'
)

$global:ProgressPreference="SilentlyContinue"

Write-Host "Downloading Project NuGets for Version: $Version"

$ProjectFilePath = $PSScriptRoot + "\SmokeTests.proj"
$NuGetDownloadUrl = "https://www.nuget.org/api/v2/package/{0}/{1}" # PackageName, Version

[xml]$ProjXml = Get-Content -Path $ProjectFilePath

$PackageList = $ProjXml.Project.PropertyGroup.ToolkitPackages -split ';'

Push-Location $DownloadPath

# Download each package
foreach ($Package in $PackageList)
{
    $PackageName = $Package.Trim() # Remove extra whitespace depending on proj file format

    $PackageUrl = ($NuGetDownloadUrl -f $PackageName, $Version)

    Write-Host "Downloading $PackageUrl"

    Invoke-WebRequest $PackageUrl -OutFile ("{0}.{1}.nupkg" -f $PackageName, $Version)
}

Pop-Location

Write-Host "Done"
