[CmdletBinding()]
param([Parameter(Mandatory=$true)]
      [string]$buildNumber)

# Constants

# Ensure the error action preference is set to the default for PowerShell3, 'Stop'
$ErrorActionPreference = 'Stop'

function Download-File
{
    param ([string] $outDir,
           [string] $downloadUrl,
           [string] $downloadName)

    # The ISO file can be large (800 MB), we want to use smart caching if we can to avoid long delays
    # Note that some sources explicitly disable caching, so this may not be possible
    $etagFile = Join-path $outDir "$downloadName.ETag"
    $downloadPath = Join-Path $outDir "$downloadName.download"
    $downloadDest = Join-Path $outDir $downloadName
    $downloadDestTemp = Join-Path $outDir "$downloadName.tmp"
    $headers = @{}

    Write-Host -NoNewline "Ensuring that $downloadName is up to date..."

    # if the destination folder doesn't exist, delete the ETAG file if it exists
    if (!(Test-Path -PathType Container $downloadDest) -and (Test-Path -PathType Container $etagFile))
    {
        Remove-Item -Force $etagFile
    }

    if (Test-Path $etagFile)
    {
        $headers.Add("If-None-Match", [System.IO.File]::ReadAllText($etagFile))
    }

    try
    {
        # Dramatically speeds up download performance
        $ProgressPreference = 'SilentlyContinue'
        $response = Invoke-WebRequest -Headers $headers -Uri $downloadUrl -PassThru -OutFile $downloadPath -UseBasicParsing
    }
    catch [System.Net.WebException]
    {
        $response = $_.Exception.Response
    }

	if ($response.StatusCode -eq 200)
	{
		Unblock-File $downloadPath
		[System.IO.File]::WriteAllText($etagFile, $response.Headers["ETag"])

		$downloadDestTemp = $downloadPath;

		# Delete and rename to final dest
		if (Test-Path -PathType Container $downloadDest)
		{
			[System.IO.Directory]::Delete($downloadDest, $true)
		}

		Move-Item -Force $downloadDestTemp $downloadDest
		Write-Host "Updated $downloadName"
	}
	elseif ($response.StatusCode -eq 304)
	{
		Write-Host "Done"
	}
	else
	{
		Write-Host
		Write-Warning "Failed to fetch updated file from $downloadUrl"
		if (!(Test-Path $downloadDest))
		{
			throw "$downloadName was not found at $downloadDest"
		}
		else
		{
			Write-Warning "$downloadName may be out of date"
		}
	}

	return $downloadDest
}

function Mount-ISO
{
	param ([string] $isoPath)

	# Check if image is already mounted
	$isoDrive = (Get-DiskImage -ImagePath $isoPath | Get-Volume).DriveLetter

	if (!$isoDrive)
	{
		Mount-DiskImage -ImagePath $isoPath -StorageType ISO
	}

	$isoVolume = (Get-DiskImage -ImagePath $isoPath | Get-Volume)
	$isoDrive = $isoVolume.DriveLetter + ":"

	Write-Host "$isoPath mounted to $isoDrive"

	return $isoDrive
}

function Dismount-ISO
{
	param ([string] $isoPath)

	$isoDrive = (Get-DiskImage -ImagePath $isoPath | Get-Volume).DriveLetter
	if ($isoDrive)
	{
		Dismount-DiskImage -ImagePath $isoPath
	}
}

# Static(ish) link for Windows SDK
# Note: there is a delay from Windows SDK announcements to availability via the static link
$uri = "https://go.microsoft.com/fwlink/?prd=11966&pver=1.0&plcid=0x409&clcid=0x409&ar=Flight&sar=Sdsurl&o1=$buildNumber"

if ($env:TEMP -eq $null)
{
  $env:TEMP = Join-Path $env:SystemDrive 'temp'
}

$winsdkTempDir = Join-Path $env:TEMP "WindowsSDK"

if (![System.IO.Directory]::Exists($winsdkTempDir))
{
  [void][System.IO.Directory]::CreateDirectory($winsdkTempDir)
}

$file = "winsdk.iso"

Write-Output "Getting WinSDK from $uri"
$downloadFile = Download-File $winsdkTempDir $uri $file

# TODO Check if zip, exe, iso, etc.
Write-Output "Mounting ISO $file..."
$isoDrive = Mount-ISO $downloadFile


Write-Host "Installing WinSDK"
$setupPath = Join-Path $isoDrive "WinSDKSetup.exe"
Start-Process -Wait $setupPath "/features OptionId.UWPCpp /q"

Dismount-ISO $downloadFile