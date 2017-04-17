$baseDir = Resolve-Path ..
$buildDir = "$baseDir\build"
$toolsDir = "$baseDir\build\tools"
$binDir = "$baseDir\bin"

$tempDir = "$binDir\temp"
$binariesDir = "$binDir\binaries"
$nupkgDir = "$binDir\nupkg"
$nuget = "$toolsDir\nuget\nuget.exe"

$xamlstyler = "$tempDir\XamlStyler.Console\tools\xstyler.exe"

if(!(Test-Path $xamlstyler)){
.$nuget install -excludeversion xamlstyler.console -outputdirectory $tempDir 
}

$stylerfile = "$baseDir\settings.xamlstyler"

$Dir = Get-ChildItem -Path $baseDir -recurse 
$List = $Dir | where {$_.Extension -eq ".xaml" -and $_.FullName -notmatch 'obj' }

$List | ForEach-Object {
    $filePath = $_.FullName

    & $xamlstyler -f "$filePath" -c "$stylerfile"
}