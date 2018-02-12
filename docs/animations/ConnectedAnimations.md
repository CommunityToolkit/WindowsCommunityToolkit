---
title: Connected Animations XAML Attached Properties
ms.date: 11/22/2017
description: The Connected Animation XAML Attached Properties enable connected animations to be defined in your XAML code
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, connected animations, animation, coordinated animations
---

# Connected Animations XAML Attached Properties

[Connected animations](https://docs.microsoft.com/en-us/windows/uwp/style/connected-animation) let you create a dynamic and compelling navigation experience by animating the transition of an element between two different views.

The Connected Animations XAML Attached Properties enable connected animations to be defined directly in your XAML code by simply adding a Key to the element that should animate. There are also attached properties to enable coordinated animations and animations in lists and grids.

## Syntax

**XAML**

```xaml
<Page ...
    xmlns:animations="using:Microsoft.Toolkit.Uwp.UI.Animations"/>
 
<Border x:Name="Element" animations:Connected.Key="item"></Border>

<TextBlock animations:Connected.AnchorElement="{x:Bind Element}" Text="Hello World"/>

<GridView animations:Connected.ListItemElementName="ItemThumbnail"
          animations:Connected.ListItemKey="listItem">
    <GridView.ItemTemplate>
        <DataTemplate>
            <Image x:Name="ItemThumbnail" Height="200" Width="200"></Image>
        </DataTemplate>
    </GridView.ItemTemplate>
</GridView>
 ```

## Sample Output

![Connected animations](../resources/images/Animations/connected.gif)

## Properties

### Connected.Key
Registers element with the [ConnectedAnimationsService](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.media.animation.connectedanimation.aspx). For the animation to work, the same key must be registered on two different pages when navigating

### Connected.AnchorElement
To enable [coordinated animations](https://docs.microsoft.com/en-us/windows/uwp/style/connected-animation#coordinated-animation), use the AnchorElement attached property on the element that should appear alongside the connected animation element by specifying the connected animation element

### Connected.ListItemKey
Registers a ListView/GridView for connected animations. When navigating from/to a page that is using this property, the connected animation will use the item passed as a **parameter** in the page navigation to select the item in the list that should animate. The Connected.ListItemElementName attached property must also be set for the animation to be registered

### Connected.ListItemElementName
Specifies what named element in the DataTemplate of an item should animate. The Connected.ListItemKey attached property must also be set for the animation to be registered.

## Requirements

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher   |
| ---------------------------------------------------------------- | ----------------------------------- |
| Namespace                                                        | Microsoft.Toolkit.Uwp.UI.Animations |
| NuGet package | [Microsoft.Toolkit.Uwp.UI.Animations](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI.Animations/) |

## API

* [Connected animations source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI.Animations/ConnectedAnimations)

