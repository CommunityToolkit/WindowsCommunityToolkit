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
}

Framework "4.6x86"

task default -depends ?

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

task Build -depends Clean, Setup, Version -description "Build all projects and get the assemblies" {
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
    
    Exec { .$nuget pack "$fullFilename" -Version "$version" -Properties "binaries=$binariesDir" -Output "$nupkgDir" } "Error packaging $projectName"
  }
}

task PublishNuget -depends PackNuGet -description "Publish the NuGet packages to the remote repositories" {
  Get-ChildItem $nupkgDir\*.nupkg | % {
    $nupkg = $_.FullName
    
    if ($isAppVeyor) {
      WriteColoredOutput -ForegroundColor Green "Archiving '$nupkg' artifact...`n"
      
      Push-AppveyorArtifact $nupkg
    }
    else {
      WriteColoredOutput -ForegroundColor Green "Publishing '$nupkg'...`n"
      
      Exec { .$nuget push "$nupkg" } "Error publishing '$nupkg'"
    }
  }
}

task ? -description "Show the help screen" {
  WriteDocumentation
}