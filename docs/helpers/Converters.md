---
title: Converters
author: nmetulev
description: Commonly used converters that allow the data to be modified as it passes through the binding engine.
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, BoolToObjectConverter, BoolToVisibilityConverter, CollectionVisibilityConverter, EmptyCollectionToObjectConverter, EmptyStringToObjectConverter, StringFormatConverter, StringVisibilityConverter
---

# Converters

Commonly used **converters** that allow the data to be modified as it passes through the binding engine.

| Converter | Purpose |
| --- | --- |
| [BoolNegationConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.boolnegationconverter?view=uwp-toolkit-dotnet) | Converts a boolean to the inverse value (True to False and vice versa) |
| [BoolToObjectConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.booltoobjectconverter?view=uwp-toolkit-dotnet) | Converts a boolean value into an object. The converted value is selected between the values of TrueValue and FalseValue properties |
| [BoolToVisibilityConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.booltovisibilityconverter?view=uwp-toolkit-dotnet) | Converts a boolean value into a Visibility enumeration |
| [CollectionVisibilityConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.collectionvisibilityconverter?view=uwp-toolkit-dotnet) | Converts a collection into a Visibility enumeration (Collapsed if the given collection is empty or null) |
| [EmptyCollectionToObjectConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptycollectiontoobjectconverter?view=uwp-toolkit-dotnet) | Converts a collection into an object. The converted value is selected between the values of EmptyValue and NotEmptyValue properties |
| [EmptyObjectToObjectConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptyobjecttoobjectconverter?view=uwp-toolkit-dotnet) | Converts a check on a null value into an object. The converted value is selected between the values of EmptyValue and NonEmptyValue properties | 
| [EmptyStringToObjectConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptystringtoobjectconverter?view=uwp-toolkit-dotnet) | Converts a string into an object. The converted value is selected between the values of EmptyValue and NotEmptyValue properties |
| [FormatStringConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.formatstringconverter?view=uwp-toolkit-dotnet) | Converts an [IFormattable](https://docs.microsoft.com/dotnet/api/system.string.format?view=netframework-4.7) value into a string. The ConverterParameter provides the string format |
| [ResourceNameToResourceStringConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.resourcenametoresourcestringconverter?view=uwp-toolkit-dotnet) | Converter to look up the source string in the App Resources strings and returns its value, if found |
| [StringFormatConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.stringformatconverter?view=uwp-toolkit-dotnet) | Converts a source object to the formatted string version using [string.Format](https://docs.microsoft.com/dotnet/api/system.string.format?view=netframework-4.7). The ConverterParameter provides the string format |
| [StringVisibilityConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.stringvisibilityconverter?view=uwp-toolkit-dotnet) | Converts a string value into a Visibility enumeration (if the value is null or empty returns a collapsed value) |
| [ToolbarFormatActiveConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.toolbarformatactiveconverter?view=uwp-toolkit-dotnet) | Compares if Formats are equal and returns bool |
| [VisibilityToBoolConverter](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.converters.visibilitytoboolconverter?view=uwp-toolkit-dotnet) | This class converts a Visibility enumeration to a boolean value |

## BoolToObjectConverter Examples

`BoolToObjectConverter` can be used to generalize the behavior of `BoolToVisibilityConverter` by allowing to pass the two values it can return.
You can use it to switch Visibility by declaring it :

```xaml
<Page ...
     xmlns:converters="using:Microsoft.Toolkit.Uwp.UI"/>

<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToVisibilityConverter" TrueValue="Visible" FalseValue="Collapsed"/>
</Page.Resources>
```

and using it like that :

```xaml
<Image Visibility="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}}" />
```

It can also be used to switch between two values of brush.

Note : you can use a resource for the brush or pass the color string and have it converted to a brush automatically.

```xaml
<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToBrushConverter" TrueValue="Green" FalseValue="{StaticResource NopeBrush}" />
</Page.Resources>
```

and using it like that :

```xaml
<Border Background="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToBrushConverter}}" />
```

An other example is to switch between two images by specifying their source :

```xaml
<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToImageConverter" TrueValue="ms-appx:///Assets/Yes.png" FalseValue="ms-appx:///Assets/No.png" />
</Page.Resources>
```

and using it like that :

```xaml
<Image Source="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToImageConverter}}" />
```


## BoolToVisibilityConverter Examples

`BoolToVisibilityConverter` can be used to easily change a boolean value to a Visibility based one. 
If targeting 14393 or later, this is done automatically through [x:Bind](https://docs.microsoft.com/windows/uwp/xaml-platform/x-bind-markup-extension).  First, declare the converter in your resources:

```xaml
<Page.Resources>
    <converters:BoolToVisibilityConverter x:Key="BoolToVisibilityConverter"/>
</Page.Resources>
```

and use it like this :

```xaml
<Image Visibility="{Binding Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}}" />
```

you can also invert the boolean as a ConverterParameter:

```xaml
<Image Visibility="{Binding Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}, ConverterParameter=True}" />
```

or if you want to not pass a parameter, you can use `BoolToObjectConverter` to create an `InverseBoolToVisibilityConveter`:

```xaml
<Page.Resources>
    <converters:BoolToObjectConverter x:Key="InverseBoolToVisibilityConverter" TrueValue="Collapsed" FalseValue="Visible"/>
</Page.Resources>
```


## EmptyObjectToObjectConverter Examples

`EmptyObjectToObjectConverter`, `EmptyCollectionToObjectConverter`, and `EmptyStringToObjectConverter` work similarly to the `BoolToObjectConverter` except using `EmptyValue` and `NotEmptyValue` instead of `TrueValue`/`FalseValue`.

They inspect the type of 'empty'/null object value and return the specific value `EmptyValue` or `NotEmptyValue` based on the result.
That result can also be inverted withe a ConverterParameter.

For instance you can generalize the `CollectionVisibilityConverter` using the `EmptyCollectionToObjectConverter`:

```xaml
<Page.Resources>
    <converters:EmptyCollectionToObjectConverter x:Key="CollectionVisibilityConverter" EmptyValue="Collapsed" NotEmptyValue="Visible"/>
</Page.Resources>
```

this can be used as follows to hide a list with no items and instead show text through inversion with the ConverterParameter:

```xaml
<ListView Visibility="{Binding Path=MyCollectionValue, Converter={StaticResource CollectionVisibilityConverter}}" />

<TextBlock Text="No Items." Visibility="{Binding Path=MyCollectionValue, Converter={StaticResource CollectionVisibilityConverter}, ConverterParameter=True}">
```


## StringFormatConverter Examples

`StringFormatConverter` allows you to format a string property upon binding wrapping [string.Format](https://docs.microsoft.com/dotnet/api/system.string.format?view=netframework-4.7).  
It only allows for a single input value (the binding string), but can be formatted with the regular string.Format
methods.  First, add it to your page resources:

```xaml
<Page.Resources>
    <converters:StringFormatConverter x:Key="StringFormatConverter"/>
</Page.Resources>
```

then use it like so:

```xaml
<TestBlock Text="{Binding IsLoading, Converter={StaticResource StringFormatConverter}, ConverterParameter='Is Loading: {0}'}" />

<TextBlock Text="{Binding RangeMin, ElementName=RangeSelector, Converter={StaticResource StringFormatConverter}, ConverterParameter='{}{0:0.##}'}" 
```

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Converters |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [Converters source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI/Converters)
