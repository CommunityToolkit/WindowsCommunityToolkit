---
title: Converters
author: nmetulev
ms.date: 08/20/2017
description: Commonly used converters that allow the data to be modified as it passes through the binding engine.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, BoolToObjectConverter, BoolToVisibilityConverter, CollectionVisibilityConverter, EmptyCollectionToObjectConverter, EmptyStringToObjectConverter, StringFormatConverter, StringVisibilityConverter
---

# Converters

Commonly used **converters** that allow the data to be modified as it passes through the binding engine.

| Converter | Purpose |
| --- | --- |
|BoolNegationConverter | Converts a boolean to the inverse value (True to False and vice versa) |
|BoolToObjectConverter | Converts a boolean value into an object. The converted value is selected between the values of TrueValue and FalseValue properties |
|BoolToVisibilityConverter | Converts a boolean value into a Visibility enumeration |
|CollectionVisibilityConverter | Converts a collection into a Visibility enumeration (Collapsed if the given collection is empty or null) |
|EmptyCollectionToObjectConverter | Converts a collection into an object. The converted value is selected between the values of EmptyValue and NotEmptyValue properties |
|EmptyObjectToObjectConverter | Converts a check on a null value into an object. The converted value is selected between the values of EmptyValue and NonEmptyValue properties | 
|EmptyStringToObjectConverter | Converts a string into an object. The converted value is selected between the values of EmptyValue and NotEmptyValue properties |
|FormatStringConverter | Converts an [IFormattable][3] value into a string. The ConverterParameter provides the string format |
|ResourceNameToResourceStringConverter | Converter to look up the source string in the App Resources strings and returns its value, if found |
|StringFormatConverter | Converts a source object to the formatted string version using [string.Format][2]. The ConverterParameter provides the string format |
|StringVisibilityConverter | Converts a string value into a Visibility enumeration (if the value is null or empty returns a collapsed value) |

## BoolToObjectConverter Examples
`BoolToObjectConverter` can be used to generalize the behavior of `BoolToVisibilityConverter` by allowing to pass the two values it can return.
You can use it to switch Visibility by declaring it :

```xml

<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToVisibilityConverter" TrueValue="Visible" FalseValue="Collapsed"/>
</Page.Resources>

```

and using it like that :

```xml

<Image Visibility="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}}" />

```

It can also be used to switch between two values of brush.

Note : you can use a resource for the brush or pass the color string and have it converted to a brush automatically.

```xml

<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToBrushConverter" TrueValue="Green" FalseValue="{StaticResource NopeBrush}" />
</Page.Resources>

```

and using it like that :

```xml

<Border Background="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToBrushConverter}}" />

```

An other example is to switch between two images by specifying their source :

```xml

<Page.Resources>
    <converters:BoolToObjectConverter x:Key="BoolToImageConverter" TrueValue="ms-appx:///Assets/Yes.png" FalseValue="ms-appx:///Assets/No.png" />
</Page.Resources>

```

and using it like that :

```xml

<Image Source="{x:Bind Path=MyBoolValue, Converter={StaticResource BoolToImageConverter}}" />

```


## BoolToVisibilityConverter Examples
`BoolToVisibilityConverter` can be used to easily change a boolean value to a Visibility based one. 
If targetting 14393 or later, this is done automatically through [x:Bind][1].  First, declare the converter in your resources:

```xml

<Page.Resources>
    <converters:BoolToVisibilityConverter x:Key="BoolToVisibilityConverter"/>
</Page.Resources>

```

and use it like this :

```xml

<Image Visibility="{Binding Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}}" />

```

you can also invert the boolean as a ConverterParameter:

```xml

<Image Visibility="{Binding Path=MyBoolValue, Converter={StaticResource BoolToVisibilityConverter}, ConverterParameter=True}" />

```

or if you want to not pass a parameter, you can use `BoolToObjectConverter` to create an `InverseBoolToVisibilityConveter`:

```xml

<Page.Resources>
    <converters:BoolToObjectConverter x:Key="InverseBoolToVisibilityConverter" TrueValue="Collapsed" FalseValue="Visible"/>
</Page.Resources>

```


## EmptyObjectToObjectConverter Examples
`EmptyObjectToObjectConverter`, `EmptyCollectionToObjectConverter`, and `EmptyStringToObjectConverter` work similarly to the `BoolToObjectConverter` except using `EmptyValue` and `NotEmptyValue` instead of `TrueValue`/`FalseValue`.

They inspect the type of 'empty'/null object value and return the specific value `EmptyValue` or `NotEmptyValue` based on the result.
That result can also be inverted withe a ConverterParameter.

For instance you can generalize the `CollectionVisibilityConverter` using the `EmptyCollectionToObjectConverter`:

```xml

<Page.Resources>
    <converters:EmptyCollectionToObjectConverter x:Key="CollectionVisibilityConverter" EmptyValue="Collapsed" NotEmptyValue="Visible"/>
</Page.Resources>

```

this can be used as follows to hide a list with no items and instead show text through inversion with the ConverterParameter:

```xml

<ListView Visibility="{Binding Path=MyCollectionValue, Converter={StaticResource CollectionVisibilityConverter}}" />

<TextBlock Text="No Items." Visibility="{Binding Path=MyCollectionValue, Converter={StaticResource CollectionVisibilityConverter}, ConverterParameter=True}">

```


## StringFormatConverter Examples
`StringFormatConverter` allows you to format a string property upon binding wraping [string.Format][2].  
It only allows for a single input value (the binding string), but can be formatted with the regular string.Format
methods.  First, add it to your page resources:

```xml

<Page.Resources>
    <converters:StringFormatConverter x:Key="StringFormatConverter"/>
</Page.Resources>

```

then use it like so:

```xml

<TestBlock Text="{Binding IsLoading, Converter={StaticResource StringFormatConverter}, ConverterParameter='Is Loading: {0}'}" />

<TextBlock Text="{Binding RangeMin, ElementName=RangeSelector, Converter={StaticResource StringFormatConverter}, ConverterParameter='{}{0:0.##}'}"
            
```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Converters |

## API

* [Converters source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/Converters)

[1]:https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/x-bind-markup-extension
[2]:https://docs.microsoft.com/en-us/dotnet/api/system.string.format?view=netframework-4.7
[3]:https://docs.microsoft.com/en-us/dotnet/api/system.iformattable?view=netframework-4.7