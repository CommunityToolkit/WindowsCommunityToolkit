# Input is a hashtable.

# The hashtable input is in #info

function ValueOr
{  param($value, $or)
   if ([bool]$value)
   { $value }
   else
   { $or }
}

function CreateRow
{
   param($data)
   $inputPath = $data['input']
   $obj = new-object PsObject
  Add-Member -in $obj -notepropertyname 'Key' -notepropertyvalue $inputPath
  Add-Member -in $obj -notepropertyname 'Version' -notepropertyvalue $data['version']
  Add-Member -in $obj -notepropertyname 'FileName' -notepropertyvalue (split-path -leaf $inputPath)
  Add-Member -in $obj -notepropertyname 'Source' -notepropertyvalue (split-path -leaf (split-path -parent $inputPath))
  Add-Member -in $obj -notepropertyname 'DurationSeconds' -notepropertyvalue $data['durationSeconds']
  Add-Member -in $obj -notepropertyname 'Precomps' -notepropertyvalue (ValueOr $data['Precomps'] 0)
  Add-Member -in $obj -notepropertyname 'Masks' -notepropertyvalue (ValueOr $data['Masks'] 0)
  Add-Member -in $obj -notepropertyname 'MaskAdditive' -notepropertyvalue (ValueOr $data['MaskAdditive'] 0)
  Add-Member -in $obj -notepropertyname 'MaskDifference' -notepropertyvalue (ValueOr $data['MaskDifference'] 0)
  Add-Member -in $obj -notepropertyname 'MaskIntersect' -notepropertyvalue (ValueOr $data['MaskIntersect'] 0)
  Add-Member -in $obj -notepropertyname 'MaskSubtract' -notepropertyvalue (ValueOr $data['MaskSubtract'] 0)
  Add-Member -in $obj -notepropertyname 'ContainerShape' -notepropertyvalue (ValueOr $data['ContainerShape'] 0)
  Add-Member -in $obj -notepropertyname 'ContainerVisual' -notepropertyvalue (ValueOr $data['ContainerVisual'] 0)
  Add-Member -in $obj -notepropertyname 'SpriteShape' -notepropertyvalue (ValueOr $data['SpriteShape'] 0)
  Add-Member -in $obj -notepropertyname 'ExpressionAnimation' -notepropertyvalue (ValueOr $data['ExpressionAnimation'] 0)
  Add-Member -in $obj -notepropertyname 'CompositionPath' -notepropertyvalue (ValueOr $data['CompositionPath'] 0)
  $obj
}

function CreateErrorRows
{
  param($data)
  $inputPath = $data['input']
  $data | %{$_.errors} | %{
     $obj = new-object PsObject
     Add-Member -in $obj -notepropertyname 'Key' -notepropertyvalue $inputPath
     Add-Member -in $obj -notepropertyname 'Error' -notepropertyvalue $_
     $obj
  }
}




$keys = ($info | %{CreateRow $_})
$errors = ($info | %{CreateErrorRows $_})

$keys | export-csv keys.csv -notype
$errors | export-csv errors.csv -notype

