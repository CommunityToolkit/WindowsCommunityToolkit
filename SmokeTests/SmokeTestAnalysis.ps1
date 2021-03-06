$global:ErrorActionPreference = "Stop"
$global:ProgressPreference="SilentlyContinue"

Write-Host "Running Smoke Test Package Analyis"
Write-Host "----------------------------------"

#####
# This script analyzes the final packaged outputs of  
# There are two things analyzed:
# - The MSIX UPLOAD package has the raw dependencies before .NET Native compilation, 
#   this can help determine the maximum impact of the package if everything were to be included.
# - The MSIX BUNDLE package is the final .NET Native compiled app,
#   this can be used to help determine the minimal impact of the package if only a few things are used.
#   This is also what would end up on a user's machine and be the footprint of the app itself.
#
# Note: The 'minimum impact' can be larger than the 'maximum impact' as there are many
# system dependencies which don't get included by default and are then referenced by a package.
# This may seem counter-intuitive at first, but it is important to remember that when combining
# package use along with the use of other platform APIs smooths these sizes out as they will
# all be shared across all use cases.
#####

# Our script is at our SmokeTest root, we want to look for the AppPackages folder
$PackagePath = $PSScriptRoot + "\AppPackages\"
$FilePattern = "SmokeTest_{0}_x86_bundle.msixupload"
$DirPattern = "SmokeTest_{0}_Test\SmokeTest_{0}_x86.msixbundle"

$BaselineName = $FilePattern -f "UWPBaseline"
$BaselineBundleName = $DirPattern -f "UWPBaseline"
$TempFolder = "ExplodedArchive"
$TempFolder2 = "ExplodedBundle"

function Expand-MsixUploadPackage {
    param (
        [string]$PackageFile,
        [string]$Destination
    )

    $ZipUpload = $PackageFile.Replace("msixupload", "zip")

    Move-Item $PackageFile -Destination $ZipUpload

    Expand-Archive $ZipUpload -DestinationPath $Destination

    Move-Item $ZipUpload -Destination $PackageFile

    Push-Location $Destination

    $Bundle = (Get-ChildItem "*.msixbundle").Name
    $ZipBundle = $Bundle.Replace("msixbundle", "zip")

    Move-Item $Bundle -Destination $ZipBundle

    Expand-Archive $ZipBundle -DestinationPath .

    Remove-Item $ZipBundle

    $msix = (Get-ChildItem "*.msix").Name
    $ZipMSIX = $msix.Replace("msix", "zip")

    Move-Item $msix -Destination $ZipMSIX

    Expand-Archive $ZipMSIX -DestinationPath . -Force # Force here as we have some duplicate file names we don't really care about from parent archives

    Remove-Item $ZipMSIX

    Pop-Location
}

function Expand-MsixBundlePackage {
    param (
        [string]$PackageFile,
        [string]$Destination
    )

    $ZipBundle = $PackageFile.Replace("msixbundle", "zip")

    Move-Item $PackageFile -Destination $ZipBundle

    Expand-Archive $ZipBundle -DestinationPath $Destination

    Move-Item $ZipBundle -Destination $PackageFile

    Push-Location $Destination

    $msix = (Get-ChildItem "*.msix").Name
    $ZipMSIX = $msix.Replace("msix", "zip")

    Move-Item $msix -Destination $ZipMSIX

    Expand-Archive $ZipMSIX -DestinationPath . -Force # Force here as we have some duplicate file names we don't really care about from parent archives

    Remove-Item $ZipMSIX

    Pop-Location
}

if (Test-Path $PackagePath)
{    
    Push-Location $PackagePath

    Write-Host "Extracting Baseline..."

    # TODO: Theoretically we could grab bits from the bin directory instead of having to expand each package, not sure about what we ignore though
    Expand-MsixUploadPackage $BaselineName -Destination $TempFolder
    Expand-MsixBundlePackage $BaselineBundleName -Destination $TempFolder2

    # Get all the base file info only (grab stuff in directories but not the directories themselves)
    $BaselineFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Exclude "SmokeTest*"
    $BaselineFiles2 = Get-ChildItem $TempFolder2 -Recurse -Attributes !Directory -Exclude "SmokeTest*"
    $SmokeTestFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Include "SmokeTest*"
    $SmokeTestFiles2 = Get-ChildItem $TempFolder2 -Recurse -Attributes !Directory -Include "SmokeTest*"

    $BaselineFootprint = ($BaselineFiles | Measure-Object -Property Length -sum).Sum + ($SmokeTestFiles | Measure-Object -Property Length -sum).Sum
    $BaselineFootprint2 = ($BaselineFiles2 | Measure-Object -Property Length -sum).Sum + ($SmokeTestFiles2 | Measure-Object -Property Length -sum).Sum
    Write-Host ("Baseline Max Footprint: {0:n0} bytes" -f $BaselineFootprint)
    Write-Host ("Baseline Min Footprint: {0:n0} bytes" -f $BaselineFootprint2)
    Write-Host "-----------------------------------------"

    $PackageList = Get-ChildItem "$PackagePath*.msixupload" -Exclude $BaselineName

    #$i = 0
    foreach ($Package in $PackageList)
    {
        # Extract the root package name between the initial '_'
        $PackageShortName = ($Package.Name -split '_')[1]

        #Write-Progress -Id 0 -Activity "Comparing Against Baseline..." -Status "Prepping Package" -PercentComplete (($i++ / $PackageList.count)*100) -CurrentOperation $Package.Name

        # Make sure we've cleaned-up the last archive
        Remove-Item $TempFolder -Recurse -Force
        Remove-Item $TempFolder2 -Recurse -Force

        #$ProgressPreference="SilentlyContinue"
        Expand-MsixUploadPackage $Package.Name -Destination $TempFolder
        
        # Also expand the final bundle based on the namespace name within the package.
        Expand-MsixBundlePackage ($DirPattern -f $PackageShortName) -Destination $TempFolder2
        #$ProgressPreference="Continue"

        [System.Collections.ArrayList]$PackageFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Exclude "SmokeTest*"
        $PackageSmokeTestFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Include "SmokeTest*"
        [System.Collections.ArrayList]$PackageFiles2 = Get-ChildItem $TempFolder2 -Recurse -Attributes !Directory -Exclude "SmokeTest*"
        $PackageSmokeTestFiles2 = Get-ChildItem $TempFolder2 -Recurse -Attributes !Directory -Include "SmokeTest*"

        Write-Host ("{0} Additional Max Footprint: {1:n0} bytes" -f $PackageShortName, (($PackageFiles | Measure-Object -Property Length -sum).Sum + ($PackageSmokeTestFiles | Measure-Object -Property Length -sum).Sum - $BaselineFootprint))
        Write-Host ("{0} Additional Min Footprint: {1:n0} bytes" -f $PackageShortName, (($PackageFiles2 | Measure-Object -Property Length -sum).Sum + ($PackageSmokeTestFiles2 | Measure-Object -Property Length -sum).Sum - $BaselineFootprint2))

        # Quick check on the base exe file/symbols differences
        foreach ($file in $SmokeTestFiles)
        {
            $match = $null
            $match = $PackageSmokeTestFiles | Where-Object {$_.Extension -eq $file.Extension}
            if ($null -ne $match)
            {
                Write-Host ("  App Diff: ({0}) = {1:n0}" -f $file.Extension, ($match.Length - $file.Length)) -ForegroundColor DarkCyan
            }
        }

        #$j = 0
        foreach ($file in $BaselineFiles)
        {
            #Write-Progress -Id 1 -ParentId 0 -Activity "Comparing Against Baseline..." -Status "Comparing Package" -PercentComplete (($j++ / $BaselineFiles.count)*100) -CurrentOperation $file.Name

            $match = $null
            $match = $PackageFiles | Where-Object {$_.Name -eq $file.Name}
            if ($null -ne $match)
            {
                # File was in baseline, but has a different size
                if ($match.Length -ne $file.Length)
                {
                    Write-Host ("  Size Diff: {0} = {1:n0}" -f $file.Name, ($match.Length - $file.Length)) -ForegroundColor Magenta
                }

                # Remove checked files (performance) and also remaining are new
                $PackageFiles.Remove($match)
            }
        }

        # List remaining (new) files to this package
        foreach ($file in $PackageFiles)
        {
            if ($file.Name -match $PackageShortName)
            {
                Write-Host ("  Lib (self): {0} = {1:n0}" -f $file.Name, $file.Length) -ForegroundColor White
            }
            else 
            {
                Write-Host ("  Additional: {0} = {1:n0}" -f $file.Name, $file.Length) -ForegroundColor Yellow
            }
        }

        # TODO: Especially if we add comparison to the main branch, we should format as an actual table and colorize via VT: https://stackoverflow.com/a/49038815/8798708

        #Write-Progress -Id 1 -ParentId 0 -Activity "Comparing Against Baseline..." -Completed
        Write-Host "-----------------COMPILED----------------"

        # Quick check on the base exe file/symbols differences
        foreach ($file in $SmokeTestFiles2)
        {
            $match = $null
            $match = $PackageSmokeTestFiles2 | Where-Object {$_.Extension -eq $file.Extension}
            if ($null -ne $match)
            {
                Write-Host ("  App Diff: ({0}) = {1:n0}" -f $file.Extension, ($match.Length - $file.Length)) -ForegroundColor DarkCyan
            }
        }

        #$j = 0
        foreach ($file in $BaselineFiles2)
        {
            #Write-Progress -Id 1 -ParentId 0 -Activity "Comparing Against Baseline..." -Status "Comparing Package" -PercentComplete (($j++ / $BaselineFiles.count)*100) -CurrentOperation $file.Name

            $match = $null
            $match = $PackageFiles2 | Where-Object {$_.Name -eq $file.Name}
            if ($null -ne $match)
            {
                # File was in baseline, but has a different size
                if ($match.Length -ne $file.Length)
                {
                    Write-Host ("  Size Diff: {0} = {1:n0}" -f $file.Name, ($match.Length - $file.Length)) -ForegroundColor Magenta
                }

                # Remove checked files (performance) and also remaining are new
                $PackageFiles2.Remove($match)
            }
        }

        # List remaining (new) files to this package
        foreach ($file in $PackageFiles2)
        {
            if ($file.Name -match $PackageShortName)
            {
                Write-Host ("  Lib (self): {0} = {1:n0}" -f $file.Name, $file.Length) -ForegroundColor White
            }
            else 
            {
                Write-Host ("  Additional: {0} = {1:n0}" -f $file.Name, $file.Length) -ForegroundColor Yellow
            }
        }

        Write-Host "-----------------------------------------"
        Write-Host
    }

    #Write-Progress -Id 0 -Activity "Comparing Against Baseline..." -Completed

    # Clean-up
    Remove-Item $TempFolder -Recurse -Force
    Remove-Item $TempFolder2 -Recurse -Force

    Pop-Location
}
else 
{
    Write-Error "Path $PackagePath not found for analysis!"        
}