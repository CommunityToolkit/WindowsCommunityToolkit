$psake.use_exit_on_error = $true

properties {
  $baseDir  = Resolve-Path ..
  $buildDir = "$baseDir\build"
  $sourceDir = "$baseDir"
  $toolsDir = "$baseDir\build\tools"
  $binDir = "$baseDir\bin"
  
  $isAppVeyor = Test-Path -Path env:\APPVEYOR
  
  $version = "2.2.1"
  
  $tempDir = "$binDir\temp"
  $binariesDir = "$binDir\binaries"
  $nupkgDir = "$binDir\nupkg"
  
  $nuget = "$toolsDir\nuget\nuget.exe"
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
  WriteColoredOutput -ForegroundColor Green "Setup environment...`n"
  
  if ($isAppVeyor) {
    $script:version = $version -replace '([0-9]+(\.[0-9]+){2}).*', ('$1-dev' + $env:APPVEYOR_BUILD_NUMBER)
    
    Update-AppveyorBuild -Version $script:version
  }
  else {
    $script:version = $version
  }
  
  Exec { .$nuget restore $packagesConfig "$sourceDir\UWP Community Toolkit.sln" } "Error pre-installing NuGet packages"
}

task Build -depends Clean, Setup -description "Build all projects and get the assemblies" {
  New-Item -Path $binariesDir -ItemType Directory | Out-Null
  
  Exec { msbuild "/t:Clean;Build" /p:Configuration=Release "/p:OutDir=$binariesDir" /p:GenerateProjectSpecificOutputFolder=true /p:TreatWarningsAsErrors=false /p:GenerateLibraryLayout=true /m "$sourceDir\UWP Community Toolkit.sln" } "Error building $solutionFile"
}

task PackNuGet -depends Build -description "Create the NuGet packages" {
  New-Item -Path $nupkgDir -ItemType Directory | Out-Null
  
  Get-ChildItem $buildDir\*.nuspec | % {
    $fullFilename = $_.FullName
    
    Exec { .$nuget pack "$fullFilename" -Version "$script:version" -Output "$nupkgDir" } "Error packaging $projectName"
  }
}

task ? -description "Show the help screen" {
  WriteDocumentation
}