---
title: NullableBool Markup Extension
author: michael-hawker
description: The NullableBool Markup Extension allows developers to specify default values in XAML for nullable bool dependency properties.
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, nullable bool, dependency property, markup extension, XAML, markup 
---

# NullableBool Markup Extension
The [NullableBool Markup Extension](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.extensions.nullablebool) provides the ability to set nullable boolean dependency properties in XAML markup.  These types of properties can normally be bound to, but can't be explicitly set to a specific value.  This extension provides that capability.

## Syntax

**XAML**

```xaml

    <Page.Resources>
        <helpers:ObjectWithNullableBoolProperty x:Key="OurObject" NullableBool="{ex:NullableBool Value=True}"/>
    </Page.Resources>

```

**C#**

```c#
    [Bindable]
    public class ObjectWithNullableBoolProperty : DependencyObject
    {
        public bool? NullableBool
        {
            get { return (bool?)GetValue(NullableBoolProperty); }
            set { SetValue(NullableBoolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NullableBool.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NullableBoolProperty =
            DependencyProperty.Register(nameof(NullableBool), typeof(bool?), typeof(ObjectWithNullableBoolProperty), new PropertyMetadata(null));
    }
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| IsNull | bool | When set to `True` will provide a `null` value and ignore the `Value` property. The default value is `False`. |
| Value | bool | Provides the specified `True` or `False` value when `IsNull` is set to `False`. The default value is `False`. |

## Requirements

| Device family | Universal, 10.0.16299.0 or higher   |
| -- | -- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Extensions |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [NullableBool source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit.Uwp.UI/Extensions/Markup/NullableBool.cs)

## Related Topics

- [MarkupExtension Class](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.markup.markupextension)
