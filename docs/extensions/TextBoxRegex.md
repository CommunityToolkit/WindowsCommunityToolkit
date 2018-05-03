---
title: TextBoxRegex XAML Property
author: nmetulev
description: The TextBoxRegex Property allows text validation using a regular expression or using built in validation types.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, TextBoxRegex, XAML
---

# TextBoxRegex XAML Property

The [TextBoxRegex Property](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.textboxregex) allows text validation using a regular expression or using built in validation types.

The developer adds a regular expression to validate the TextBox Text against the regular expression throw Regex property or from selecting ValidationType property on the TextBox.

The validation has 2 modes (`ValidationMode`):
1) Normal (Default) : This type will set TextBox IsValid attached property to false or true whether the TextBox text is a valid or not against the Regex property.
2) Forced : This type sets the IsValid property and remove the TextBox text if not valid when the TextBox lose focus.   

## Syntax

```xaml
<TextBox extensions:TextBoxRegex.Regex="^\s*\+?\s*([0-9][\s-]*){9,}$" />

<TextBox extensions:TextBoxRegex.ValidationMode="Forced"
            extensions:TextBoxRegex.ValidationType="PhoneNumber"
            Text="+61616161611" />

<TextBox extensions:TextBoxRegex.ValidationType="Email"   />

<TextBox extensions:TextBoxRegex.ValidationMode="Forced"
            extensions:TextBoxRegex.ValidationType="Decimal" />
```

## Sample Output

Text box with ValidationType=Email, validation occurs on TextChanged

![TextBoxRegex animation](../resources/images/Extensions/TextBoxRegex.gif)

## Attached Properties

| Property | Type | Description |
| -- | -- | -- |
| IsValid | bool | Represents the custom mask that the user can create to add his own variable characters based on regex expression |
| Regex | string | Set the regular expression that will be used to validate the TextBox |
| ValidationMode | [ValidationMode](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.textboxregex.validationmode) | Set validation mode. Normal or Forced |
| ValidationType | [ValidationType](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.textboxregex.validationtype) | Set a built in predefined validation types Email, Decimal, Phone Number, Character or Number |

## Examples

The following sample demonstrates how to add TextBoxRegex property.

```xaml
<Page x:Class="Microsoft.Toolkit.Uwp.SampleApp.SamplePages.TextBoxRegexPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:common="using:Microsoft.Toolkit.Uwp.SampleApp.Common"
      xmlns:extensions="using:Microsoft.Toolkit.Uwp.UI.Extenions"
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      mc:Ignorable="d">

    <Page.Resources>
        <common:BoolStringConverter x:Key="StringFormatConverter" />
        <Style x:Key="TextBoxRegexStyle"
               TargetType="TextBox">
            <Setter Property="VerticalAlignment" Value="Top" />
            <Setter Property="TextWrapping" Value="Wrap" />
        </Style>
        <DataTemplate x:Key="HeaderTemplate">
            <StackPanel>
                <TextBlock Text="{Binding}"
                           TextWrapping="WrapWholeWords" />
            </StackPanel>
        </DataTemplate>
    </Page.Resources>

    <Grid Background="{StaticResource Brush-Grey-05}">
        <Grid Margin="30">
            <Grid.RowDefinitions>
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
            </Grid.RowDefinitions>

            <StackPanel Margin="10,0,10,0">
                <TextBox Name="PhoneNumberValidator"
                         extensions:TextBoxRegex.Regex="^\s*\+?\s*([0-9][\s-]*){9,}$"
                         Header="Text box with Regex extension for phone number, validation occurs on TextChanged"
                         HeaderTemplate="{StaticResource HeaderTemplate}"
                         Style="{StaticResource TextBoxRegexStyle}" />
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="Is Valid: " />
                    <TextBlock Text="{Binding (extensions:TextBoxRegex.IsValid), ElementName=PhoneNumberValidator, Converter={StaticResource StringFormatConverter}}" />
                </StackPanel>

            </StackPanel>

            <StackPanel Grid.Row="1"
                        Margin="10,0,10,0">
                <TextBox Name="PhoneNumberValidatorForce"
                         extensions:TextBoxRegex.ValidationMode="Forced"
                         extensions:TextBoxRegex.ValidationType="PhoneNumber"
                         Header="Text box with ValidationType=PhoneNumber, validation occurs on TextChanged and force occurs on lose focus with ValidationMode=Force"
                         HeaderTemplate="{StaticResource HeaderTemplate}"
                         Style="{StaticResource TextBoxRegexStyle}"
                         Text="+61616161611" />
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="Is Valid: " />
                    <TextBlock Text="{Binding (extensions:TextBoxRegex.IsValid), ElementName=PhoneNumberValidatorForce, Converter={StaticResource StringFormatConverter}}" />
                </StackPanel>
            </StackPanel>

            <StackPanel Grid.Row="2"
                        Margin="10,0,10,0">
                <TextBox Name="EmailValidatorForce"
                         extensions:TextBoxRegex.ValidationType="Email"
                         Header="Text box with ValidationType=Email, validation occurs on TextChanged"
                         HeaderTemplate="{StaticResource HeaderTemplate}"
                         Style="{StaticResource TextBoxRegexStyle}" />
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="Is Valid: " />
                    <TextBlock Text="{Binding (extensions:TextBoxRegex.IsValid), ElementName=EmailValidatorForce, Converter={StaticResource StringFormatConverter}}" />
                </StackPanel>
            </StackPanel>

            <StackPanel Grid.Row="3"
                        Margin="10,0,10,0">
                <TextBox Name="DecimalValidatorForce"
                         extensions:TextBoxRegex.ValidationMode="Forced"
                         extensions:TextBoxRegex.ValidationType="Decimal"
                         Header="Text box with ValidationType=Decimal, validation occurs on TextChanged and force occurs on lose focus with ValidationMode=Force (333,111 or 333.111)"
                         HeaderTemplate="{StaticResource HeaderTemplate}"
                         Style="{StaticResource TextBoxRegexStyle}" />
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="Is Valid: " />
                    <TextBlock Text="{Binding (extensions:TextBoxRegex.IsValid), ElementName=DecimalValidatorForce, Converter={StaticResource StringFormatConverter}}" />
                </StackPanel>
            </StackPanel>

        </Grid>
    </Grid>
</Page>
```

## Sample Code

[TextBoxRegex Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/TextBoxRegex). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [TextBoxRegex source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Extensions/TextBoxRegEx)
