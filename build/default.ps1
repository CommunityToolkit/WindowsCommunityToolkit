$psake.use_exit_on_error = $true

properties {
  $baseDir  = Resolve-Path ..
  $buildDir = "$baseDir\build"
  $sourceDir = "$baseDir"
  $toolsDir = "$baseDir\build\tools"
  $binDir = "$baseDir\bin"
  
  $isAppVeyor = Test-Path -Path env:\APPVEYOR
  
  $tempDir = "$binDir\temp"
  $nupkgDir = "$binDir\nupkg"
  
  $nuget = "$toolsDir\nuget\nuget.exe"
  
  $signClientSettings = "$buildDir\SignClientSettings.json"
  $hasSignClientSecret = !([string]::IsNullOrEmpty($env:SignClientSecret))
  $signClientAppPath = "$tempDir\SignClient\Tools\netcoreapp1.1\SignClient.dll"
}

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
  New-Item -Path "$tempDir" -ItemType Directory | Out-Null
  
  WriteColoredOutput -ForegroundColor Green "Installing VSWhere NuGet package...`n"
  
  Exec { .$nuget install -excludeversion vswhere -outputdirectory $tempDir } "Error installing VSWhere NuGet package"
  
  WriteColoredOutput -ForegroundColor Green "Executing VSWhere...`n"
  
  [xml]$vsResult = Exec { .$tempDir\vswhere\tools\vswhere.exe -latest -requires Microsoft.Component.MSBuild -format xml } "Error executing VSWhere"
  
  $vsPath = $vsResult.instances.instance.installationPath
  $buildToolsPath = "$vsPath\MSBuild\15.0\Bin\"
  
  WriteColoredOutput -ForegroundColor Green "Updating build tools path...`n"
  
  if (-not (Test-Path -path "$buildToolsPath")) {
    throw "Path not found: $buildToolsPath"
  }
  
  $env:path = "$buildToolsPath;$env:path"
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
  
  Exec { .$nuget install -excludeversion nerdbank.gitversioning -pre -Version 2.0.37-beta  -outputdirectory $tempDir } "Error downloading Nerdbank.GitVersion"
  

  WriteColoredOutput -ForegroundColor Green "Retrieving version...`n"

  $versionObj = .$tempDir\nerdbank.gitversioning\tools\Get-Version.ps1

  $script:version = $versionObj.NuGetPackageVersion
  
  WriteColoredOutput -ForegroundColor Green "Build version: $script:version`n"
}

task Build -depends Clean, Setup, Verify, Version -description "Build all projects and get the assemblies" {
    
  # Force a restore again to get proper version numbers https://github.com/NuGet/Home/issues/4337
  Exec { msbuild "/t:Restore" /p:Configuration=Release /m "$sourceDir\UWP Community Toolkit.sln" } "Error restoring $solutionFile"
  Exec { msbuild "/t:Restore" /p:Configuration=Release /m "$sourceDir\UWP Community Toolkit.sln" } "Error restoring $solutionFile"

  Exec { msbuild "/t:Build" /p:Configuration=Release "/p:PackageOutputPath=$nupkgDir" /p:GeneratePackageOnBuild=true /p:TreatWarningsAsErrors=false /p:GenerateLibraryLayout=true /m "$sourceDir\UWP Community Toolkit.sln" } "Error building $solutionFile"
 
}


task SignNuGet -depends Build -description "Sign the NuGet packages with the Code Signing service" {

  if($hasSignClientSecret) {

    WriteColoredOutput -ForegroundColor Green "Downloading Sign Client...`n"
    
    Exec { .$nuget install -excludeversion SignClient -Version 0.8.0 -outputdirectory $tempDir } "Error downloading Sign Client"
   
    WriteColoredOutput -ForegroundColor Green "Signing NuPkg files...`n"

    Get-ChildItem $nupkgDir\*.nupkg | % {
      $nupkg = $_.FullName
      
      WriteColoredOutput -ForegroundColor Green "Submitting '$nupkg' for signing...`n"
      
      dotnet $signClientAppPath 'sign' -c $signClientSettings -i $nupkg -s $env:SignClientSecret -n 'UWP Community Toolkit' -d 'UWP Community Toolkit' -u 'https://developer.microsoft.com/en-us/windows/uwp-community-toolkit' 
  
      WriteColoredOutput -ForegroundColor Green "Finished signing '$nupkg'`n"
    }
  
  } else {
    WriteColoredOutput -ForegroundColor Yellow "Client Secret not found, not signing packages...`n"
  }
  
}

task ? -description "Show the help screen" {
  WriteDocumentation
}