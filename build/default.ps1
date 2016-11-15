$psake.use_exit_on_error = $true

properties {
  $baseDir  = Resolve-Path ..
  $buildDir = "$baseDir\build"
  $sourceDir = "$baseDir"
  $toolsDir = "$baseDir\build\tools"
  $binDir = "$baseDir\bin"
  
  $isAppVeyor = Test-Path -Path env:\APPVEYOR
  
  $tempDir = "$binDir\temp"
  $binariesDir = "$binDir\binaries"
  $nupkgDir = "$binDir\nupkg"
  
  $nuget = "$toolsDir\nuget\nuget.exe"
  $gitversion = "$toolsDir\gitversion\gitversion.exe"
  
  $signClientSettings = "$buildDir\SignClientSettings.json"
  $hasSignClientSecret = !([string]::IsNullOrEmpty($env:SignClientSecret))
  $signClientAppPath = "$tempDir\SignClient\Tools\SignClient.dll"
}

Framework "4.6x86"

task default -depends ?

task UpdateHeaders -description "Updates the headers in *.cs files" {
  $header = [System.IO.File]::ReadAllText("$buildDir\header.txt")

  Get-ChildItem -Path $sourceDir -Filter *.cs -Exclude *generated* -Recurse | % {
    $fullFilename = $_.FullName
    $filename = $_.Name
    
    $oldContent = [System.IO.File]::ReadAllText($fullFilename)
    
    $newContent = "$header`r`n" + ($oldContent -Replace "^(//.*\r?\n|\r?\n)*", "")
    
    if ($newContent -ne $oldContent) {
      WriteColoredOutput -ForegroundColor Green "Updating '$fullFilename' header...`n"
      
      [System.IO.File]::WriteAllText($fullFilename, $newContent, [System.Text.Encoding]::UTF8)
    }
  }
}

task Clean -description "Clean the output folder" {
  if (Test-Path -path $binDir) {
    WriteColoredOutput -ForegroundColor Green "Deleting Working Directory...`n"
    
    Remove-Item $binDir -Recurse -Force
  }
  
  New-Item -Path $binDir -ItemType Directory | Out-Null
}

task Setup -description "Setup environment" {
  WriteColoredOutput -ForegroundColor Green "Restoring NuGet packages...`n"
  
  Exec { .$nuget restore $packagesConfig "$sourceDir\UWP Community Toolkit.sln" } "Error pre-installing NuGet packages"
}

task Verify -description "Run pre-build verifications" {
  $header = [System.IO.File]::ReadAllText("$buildDir\header.txt")

  Get-ChildItem -Path $sourceDir -Filter *.cs -Exclude *generated* -Recurse | % {
    $fullFilename = $_.FullName
    $filename = $_.Name
    
    $oldContent = [System.IO.File]::ReadAllText($fullFilename)
    
    $newContent = "$header`r`n" + ($oldContent -Replace "^(//.*\r?\n|\r?\n)*", "")
    
    if ($newContent -ne $oldContent) {
      WriteColoredOutput -ForegroundColor Yellow "Wrong/missing header on '$fullFilename'"
      
      $raiseError = $true
    }
  }
  
  if ($raiseError) {
    throw "Please run '.\build.ps1 UpdateHeaders' and commit the changes."
  }
}

task Version -description "Updates the version entries in AssemblyInfo.cs files" {
  WriteColoredOutput -ForegroundColor Green "Downloading GitVersion...`n"
  
  Exec { .$nuget install -excludeversion gitversion.commandline -outputdirectory $tempDir } "Error downloading GitVersion"
  
  WriteColoredOutput -ForegroundColor Green "Restoring AssemblyInfo.cs files...`n"
  
  Get-ChildItem $sourceDir -re -in AssemblyInfo.cs | % {
    git checkout $_
  }
  
  WriteColoredOutput -ForegroundColor Green "Updating AssemblyInfo.cs files...`n"
  
  Exec { .$tempDir\gitversion.commandline\tools\gitversion.exe $sourceDir /l console /output buildserver /updateassemblyinfo /nofetch } "Error updating GitVersion"
  
  WriteColoredOutput -ForegroundColor Green "Retrieving version...`n"

  $versionObj = .$tempDir\gitversion.commandline\tools\gitversion.exe /nofetch | ConvertFrom-Json

  $script:version = $versionObj.NuGetVersionV2
  
  WriteColoredOutput -ForegroundColor Green "Build version: $script:version`n"
}

task Build -depends Clean, Setup, Verify, Version -description "Build all projects and get the assemblies" {
  New-Item -Path $binariesDir -ItemType Directory | Out-Null
  
  Exec { msbuild "/t:Clean;Build" /p:Configuration=Release "/p:OutDir=$binariesDir" /p:GenerateProjectSpecificOutputFolder=true /p:TreatWarningsAsErrors=false /p:GenerateLibraryLayout=true /m "$sourceDir\UWP Community Toolkit.sln" } "Error building $solutionFile"
  
  WriteColoredOutput -ForegroundColor Green "Restoring AssemblyInfo.cs files...`n"

  Get-ChildItem $sourceDir -re -in AssemblyInfo.cs | % {
    git checkout $_
  }
}

task PackNuGet -depends Build -description "Create the NuGet packages" {
  New-Item -Path $nupkgDir -ItemType Directory | Out-Null
  
  Get-ChildItem $buildDir\*.nuspec | % {
    $fullFilename = $_.FullName
    
    Exec { .$nuget pack "$fullFilename" -Version "$script:version" -Properties "binaries=$binariesDir" -Output "$nupkgDir" } "Error packaging $projectName"
  }
}

task PackNuGetNoBuild -description "Create the NuGet packages with existing binaries" {
  New-Item -Path $nupkgDir -ItemType Directory | Out-Null
  
  $versionObj = .$tempDir\gitversion.commandline\tools\gitversion.exe | ConvertFrom-Json

  $version = $versionObj.NuGetVersionV2
  
  Get-ChildItem $buildDir\*.nuspec | % {
    $fullFilename = $_.FullName
    
    Exec { .$nuget pack "$fullFilename" -symbols -Version "$version" -Properties "binaries=$binariesDir" -Output "$nupkgDir" } "Error packaging $projectName"
  }
}

task SignNuGet -depends PackNuGet -description "Sign the NuGet packages with the Code Signing service" {

  if($hasSignClientSecret) {

    WriteColoredOutput -ForegroundColor Green "Downloading Sign Client...`n"
    
    Exec { .$nuget install -excludeversion SignClient -Version 0.5.0-beta4 -pre -outputdirectory $tempDir } "Error downloading Sign Client"
   
    WriteColoredOutput -ForegroundColor Green "Signing NuPkg files...`n"

    Get-ChildItem $nupkgDir\*.nupkg | % {
      $nupkg = $_.FullName
      
      WriteColoredOutput -ForegroundColor Green "Submitting '$nupkg' for signing...`n"
      
      dotnet $signClientAppPath 'zip' -c $signClientSettings -i $nupkg -s $env:SignClientSecret -n 'UWP Community Toolkit' -d 'UWP Community Toolkit' -u 'https://developer.microsoft.com/en-us/windows/uwp-community-toolkit' 
  
      WriteColoredOutput -ForegroundColor Green "Finished signing '$nupkg'`n"
    }
  
  } else {
    WriteColoredOutput -ForegroundColor Yellow "Client Secret not found, not signing packages...`n"
  }
  
}

task ? -description "Show the help screen" {
  WriteDocumentation
}