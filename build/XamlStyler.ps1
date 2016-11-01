$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path

$slndir = (get-item $directorypath).Parent.FullName

$Dir = Get-ChildItem $slndir -recurse
$List = $Dir | where {$_.Extension -eq ".xaml"}
$List | Format-Table name