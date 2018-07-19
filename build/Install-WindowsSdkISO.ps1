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

function Get-ISODriveLetter
{
    param ([string] $isoPath)

    $diskImage = Get-DiskImage -ImagePath $isoPath
    if ($diskImage)
    {
        $volume = Get-Volume -DiskImage $diskImage

        if ($volume)
        {
            $driveLetter = $volume.DriveLetter
            if ($driveLetter)
            {
                $driveLetter += ":"
                return $driveLetter
            }
        }
    }

    return $null
}

function Mount-ISO
{
    param ([string] $isoPath)

    # Check if image is already mounted
    $isoDrive = Get-ISODriveLetter $isoPath

    if (!$isoDrive)
    {
        Mount-DiskImage -ImagePath $isoPath -StorageType ISO | Out-Null
    }

    $isoDrive = Get-ISODriveLetter $isoPath
    Write-Verbose "$isoPath mounted to ${isoDrive}:"
}

function Dismount-ISO
{
    param ([string] $isoPath)

    $isoDrive = (Get-DiskImage -ImagePath $isoPath | Get-Volume).DriveLetter

    if ($isoDrive)
    {
        Write-Verbose "$isoPath dismounted"
        Dismount-DiskImage -ImagePath $isoPath | Out-Null
    }
}

function Disable-StrongName
{
    param ([string] $publicKeyToken = "*")

    reg ADD "HKLM\SOFTWARE\Microsoft\StrongName\Verification\*,$publicKeyToken" /f | Out-Null
    if ($env:PROCESSOR_ARCHITECTURE -eq "AMD64")
    {
        reg ADD "HKLM\SOFTWARE\Wow6432Node\Microsoft\StrongName\Verification\*,$publicKeyToken" /f | Out-Null
    }
}

function Test-Admin
{
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal $identity
    $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
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

$file = "winsdk_$buildNumber.iso"

Write-Verbose "Getting WinSDK from $uri"
$downloadFile = Download-File $winsdkTempDir $uri $file

# TODO Check if zip, exe, iso, etc.


try
{
    Write-Host -NoNewline "Mounting ISO $file..."
    Mount-ISO $downloadFile
    Write-Host "Done"

    $isoDrive = Get-ISODriveLetter $downloadFile

    if (Test-Path $isoDrive)
    {
        Write-Host -NoNewLine "Installing WinSDK..."

        $setupPath = Join-Path "$isoDrive" "WinSDKSetup.exe"
        Start-Process -Wait $setupPath "/features OptionId.UWPCpp /q"
        Write-Host "Done"
    }
    else
    {
        throw "Could not find mounted ISO at ${isoDrive}"
    }

    Write-Host -NoNewline "Disabling StrongName for Windows SDK..."
    if (Test-Admin)
    {
        Disable-StrongName "31bf3856ad364e35"
        Write-Host "Done"
    }
    else
    {
        Write-Host
        throw "ERROR: Need elevation to edit registry to disable StrongName"
    }
}
finally
{
    Write-Host -NoNewline "Dismounting ISO $file..."
    Dismount-ISO $downloadFile
    Write-Host "Done"
}