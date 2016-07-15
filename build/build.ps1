param(
  [string]$Task = "default",
  [hashtable] $Parameters = @{},
  [hashtable] $Properties = @{},
  [switch]$Help
)

$path = Split-Path -Parent $MyInvocation.MyCommand.Definition

Import-Module "$path\tools\psake\psake.psm1"

try
{
  if ($Help){ 
	  try {
      Get-Help $MyInvocation.MyCommand.Definition -full
      
      Write-Host "Available build tasks:"
      
      Invoke-psake -nologo -docs
  	}
  	catch {
  	}
  	
	  return
  }
  
  Invoke-psake "$path/default.ps1" -task $Task -properties $Properties -parameters $Parameters -nologo

  if ($psake.build_success -eq $false)
  {
    exit 1
  }
  else
  {
    exit 0
  }
}
finally
{
  Remove-Module psake
}