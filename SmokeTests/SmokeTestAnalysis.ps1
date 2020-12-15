$ErrorActionPreference = "Stop"
$ProgressPreference="SilentlyContinue"

Write-Host "Running Smoke Test Package Analyis"
Write-Host "----------------------------------"

# Our script is at our SmokeTest root, we want to look for the AppPackages folder
$PackagePath = $PSScriptRoot + "\AppPackages\"
$FilePattern = "SmokeTest_{0}_x86_bundle.msixupload"
$BaselineName = $FilePattern -f "UWPBaseline"
$TempFolder = "ExplodedArchive"

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

if (Test-Path $PackagePath)
{    
    Push-Location $PackagePath

    Write-Host "Extracting Baseline..."

    # TODO: Theoretically we could grab bits from the bin directory instead of having to expand each package, not sure about what we ignore though
    Expand-MsixUploadPackage $BaselineName -Destination $TempFolder

    # Get all the base file info only (grab stuff in directories but not the directories themselves)
    $BaselineFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Exclude "SmokeTest*"
    $SmokeTestFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Include "SmokeTest*"

    $BaselineFootprint = ($BaselineFiles | Measure-Object -Property Length -sum).Sum + ($SmokeTestFiles | Measure-Object -Property Length -sum).Sum
    Write-Host ("Baseline Footprint: {0:n0} bytes" -f $BaselineFootprint)
    Write-Host "-----------------------------------------"

    $PackageList = Get-ChildItem "$PackagePath*.msixupload" -Exclude $BaselineName

    #$i = 0
    foreach ($Package in $PackageList)
    {
        #Write-Progress -Id 0 -Activity "Comparing Against Baseline..." -Status "Prepping Package" -PercentComplete (($i++ / $PackageList.count)*100) -CurrentOperation $Package.Name

        # Make sure we've cleaned-up the last archive
        Remove-Item $TempFolder -Recurse -Force

        #$ProgressPreference="SilentlyContinue"
        Expand-MsixUploadPackage $Package.Name -Destination $TempFolder
        #$ProgressPreference="Continue"

        [System.Collections.ArrayList]$PackageFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Exclude "SmokeTest*"
        $PackageSmokeTestFiles = Get-ChildItem $TempFolder -Recurse -Attributes !Directory -Include "SmokeTest*"

        # TODO: Make function or regex better to extra package name more easily based on a template string at the top or something...
        $PackageShortName = $Package.Name.substring(10, $Package.Name.Length - 32)
        Write-Host ("{0} Additional Footprint: {1:n0} bytes" -f $PackageShortName, (($PackageFiles | Measure-Object -Property Length -sum).Sum + ($PackageSmokeTestFiles | Measure-Object -Property Length -sum).Sum - $BaselineFootprint))

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
        Write-Host "-----------------------------------------"
        Write-Host
    }

    #Write-Progress -Id 0 -Activity "Comparing Against Baseline..." -Completed

    # Clean-up
    Remove-Item $TempFolder -Recurse -Force

    Pop-Location
}
else 
{
    Write-Error "Path $PackagePath not found for analysis!"        
}