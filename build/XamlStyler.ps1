$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path

$slndir = (get-item $directorypath).Parent.FullName
$stylerfile = $slndir + '\settings.xamlstyler'
$stylerexe = $slndir + '\build\tools\xamlstyler\xstyler.exe'

$Dir = Get-ChildItem -Path $slndir -recurse 
$List = $Dir | where {$_.Extension -eq ".xaml" -and $_.FullName -notmatch 'obj' }

$List | ForEach-Object {
    $filePath = $_.FullName

    & $stylerexe -f "$filePath" -c "$stylerfile"
}