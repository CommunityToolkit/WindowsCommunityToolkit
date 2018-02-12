#===========================================================================
# UWP Community Toolkit Sample Copier
# -----------------------------------
# Run script using PowerShell to copy a sample app folder with a new name
# to use as a template for a new sample.
#
# Will automatically update name of classes to new name.
#
# Tool will pop-up a window which lets you select an existing sample.
# Enter a new sample name and a category, then click Copy.
#===========================================================================

# Reference: https://foxdeploy.com/2015/04/16/part-ii-deploying-powershell-guis-in-minutes-using-visual-studio/

# Load Required Reference
[void][System.Reflection.Assembly]::LoadWithPartialName('presentationframework')

# Get Current Sample List
$samples = Get-ChildItem -Directory | Get-ItemPropertyValue -Name Name

$sampletxt = ""

# Create ComboBox Item text for UI injection
$samples | ForEach-Object {
    $sampletxt = $sampletxt + "<ComboBoxItem>" + $_ + "</ComboBoxItem>`n"
}

#===========================================================================
# WPF XAML Template
#===========================================================================

$wpfxaml = @"
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:sys="clr-namespace:System;assembly=mscorlib" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Copy Sample"
        Width="300" Height="282"
        ResizeMode="NoResize"
        WindowStyle="ToolWindow"
        WindowStartupLocation="CenterScreen"
        Topmost="True"
        FocusManager.FocusedElement="{Binding ElementName=CurrentSample}"
        >
  <StackPanel Orientation="Vertical" Margin="12,8,12,12">
  	<Label>Template From Sample:</Label>
	<ComboBox x:Name="CurrentSample">
"@ + $sampletxt + @"
	</ComboBox>
	<Label Margin="0,4,0,0">New Sample Name:</Label>
	<TextBox x:Name="NewSample"/>
    <Label Margin="0,4,0,0">New Sample Description:</Label>
	<TextBox x:Name="NewDescription"/>
	<Label>New Sample Category:</Label>
	<ComboBox x:Name="NewCategory">
		<ComboBoxItem>Animations</ComboBoxItem>
		<ComboBoxItem IsSelected="True">Controls</ComboBoxItem>
		<ComboBoxItem>Developer tools</ComboBoxItem>
		<ComboBoxItem>Extensions</ComboBoxItem>
		<ComboBoxItem>Helpers</ComboBoxItem>
		<ComboBoxItem>Notifications</ComboBoxItem>
		<ComboBoxItem>Services</ComboBoxItem>
	</ComboBox>
	<Button x:Name="ButtonStart" Margin="0,12,0,0">Copy</Button>
  </StackPanel>
</Window>
"@

#===========================================================================
# Load XAML from Template
#===========================================================================

[xml]$xml = $wpfxaml -replace "x:N",'N'

$reader=(New-Object System.Xml.XmlNodeReader $xml)
[System.Windows.Window]$Window=[Windows.Markup.XamlReader]::Load( $reader )

#===========================================================================
# Load XAML Objects In PowerShell
#===========================================================================
  
$xml.SelectNodes("//*[@Name]") | %{ 
    Write-Debug "found item $($_.Name)";
    try {Set-Variable -Name "Xaml$($_.Name)" -Value $Window.FindName($_.Name) -ErrorAction Stop} 
    catch{throw}
}
 
Function Get-FormVariables{
    if ($global:ReadmeDisplay -ne $true) {
        Write-host "If you need to reference this display again, run Get-FormVariables" -ForegroundColor Yellow;$global:ReadmeDisplay=$true
    }
    write-host "Found the following interactable elements from our form" -ForegroundColor Cyan
    get-variable Xaml*
}
 
####Get-FormVariables
 
#===========================================================================
# Setup WPF Window Logic
#===========================================================================
 
$XamlCurrentSample.SelectedIndex = 0

[bool]$Perform = $false

$XamlButtonStart.Add_Click({
    IF([string]::IsNullOrEmpty($XamlNewSample.Text)) {
        [System.Windows.MessageBox]::Show("Please give the new sample a name.")
    }
    ELSEIF ($samples.Contains($XamlNewSample.Text))
    {
        [System.Windows.MessageBox]::Show("New name already exists.")
    } 
    ELSE 
    {
        $script:Perform = $true
        $Window.Close()
    }
})
 
#===========================================================================
# Show the Window
#===========================================================================
$Window.ShowDialog() | Out-Null

#===========================================================================
# Do Copy Sample Work
#===========================================================================

IF ($Perform) {
    Write-Host

    $Source = $XamlCurrentSample.SelectedValue.Content
    $Destination = $XamlNewSample.Text

    # Copy Folder
    Write-Host "Copying" $Source "to" $Destination
    
    Copy-Item $Source -Destination $Destination -Recurse

    Set-Location -Path $Destination

    $image = ""

    # Setup New Files
    Get-ChildItem -File | ForEach-Object {
        # Rename File to New Name
        $newname = $_.Name -replace $Source, $Destination
        Rename-Item $_.Name $newname

        # Replace occurances of original sample name with new sample name
        IF ($_.Extension -ne ".bind" -and $_.Extension -ne ".png") {
            $text = [System.IO.File]::ReadAllText($Destination + "\" + $newname)

            $text = $text -replace $Source, $Destination

            [System.IO.File]::WriteAllText($Destination + "\" + $newname, $text)
        }
        ELSEIF ($_.Extension -eq ".png")
        {
            $script:image = $newname
        }
    }

    Set-Location -Path ..

    # Append samples.json
    $samples = [System.IO.File]::ReadAllText("samples.json") | ConvertFrom-Json

    $samples | ForEach-Object {
        IF ($_.Name -eq $XamlNewCategory.Text) {
            # Setup array so we can mutate it
            $_.Samples = New-Object System.Collections.ArrayList(, $_.Samples)

            # Add new Sample Info
            # TODO: Future - Be smarter about grabbing existing file paths?
            $sample = @{
              "Name" = $Destination;
              "Type" = $Destination + "Page";
              "About" = $XamlNewDescription.Text;
              "CodeUrl" = "https://github.com/Microsoft/UWPCommunitToolkit/tree/master/ !TODO";
              "XamlCodeFile" = "!TODO";
              "CodeFile" = "!TODO";
              "Icon" = "/SamplePages/" + $Destination + "/" + $image;
              "DocumentationUrl" = "https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/master/docs/ !TODO";
            }

            $new = New-Object -TypeName PSObject -Property $sample

            $_.Samples.Add($new)
        }
    }

    $text = $samples | ConvertTo-Json -Depth 4

    [System.IO.File]::WriteAllText("samples.json", $text)

    Write-Host "Done"
}
