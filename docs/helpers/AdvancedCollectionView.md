---
title: AdvancedCollectionView
author: nmetulev
ms.date: 08/20/2017
description: The AdvancedCollectionView is a collection view implementation that support filtering, sorting and incremental loading. It's meant to be used in a viewmodel. 
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, AdvancedCollectionView
---

# AdvancedCollectionView

The AdvancedCollectionView is a collection view implementation that support filtering, sorting and incremental loading. It's meant to be used in a viewmodel. 

## Usage

In your viewmodel instead of having a public [IEnumerable](https://docs.microsoft.com/en-us/dotnet/core/api/system.collections.generic.ienumerable-1) of some sort to be bound to an eg. [Listview](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.ListView), create a public AdvancedCollectionView and pass your list in the constructor to it. If you've done that you can use the many useful features it provides:

* sorting your list using the `SortDirection` helper: specify any number of property names to sort on with the direction desired
* filtering your list using a [Predicate](https://docs.microsoft.com/en-us/dotnet/core/api/system.predicate-1): this will automatically filter your list only to the items that pass the check by the predicate provided
* deferring notifications using the `NotificationDeferrer` helper: with a convenient _using_ pattern you can increase performance while doing large-scale modifications in your list by waiting with updates until you've completed your work
* incremental loading: if your source collection supports the feature then AdvancedCollectionView will do as well (it simply forwards the calls)

## Example

```csharp

    // Be sure to include the using at the top of the file:
    //using Microsoft.Toolkit.Uwp.UI;

    // Grab a sample type
    public class Person
    {
        public string Name { get; set; }
    }

    // Set up the original list with a few sample items

    var oc = new ObservableCollection<Person>
    {
        new Person { Name = "Staff" },
        new Person { Name = "42" },
        new Person { Name = "Swan" },
        new Person { Name = "Orchid" },
        new Person { Name = "15" },
        new Person { Name = "Flame" },
        new Person { Name = "16" },
        new Person { Name = "Arrow" },
        new Person { Name = "Tempest" },
        new Person { Name = "23" },
        new Person { Name = "Pearl" },
        new Person { Name = "Hydra" },
        new Person { Name = "Lamp Post" },
        new Person { Name = "4" },
        new Person { Name = "Looking Glass" },
        new Person { Name = "8" },
    };

    // Set up the AdvancedCollectionView to filter and sort the original list

    var acv = new AdvancedCollectionView(oc);

    // Let's filter out the integers
    int nul;
    acv.Filter = x => !int.TryParse(((Person)x).Name, out nul);

    // And sort ascending by the property "Name"
    acv.SortDescriptions.Add(new SortDescription("Name", SortDirection.Ascending));

    // AdvancedCollectionView can be bound to anything that uses collections. 
    YourListView.ItemsSource = acv;



```

## Remarks

_What source can I use?_

It's not necessary to use an eg. [ObservableCollection](https://docs.microsoft.com/en-us/dotnet/core/api/system.collections.objectmodel.observablecollection-1) to use the AdvancedCollectionView. It works as expected even when providing a simple [List](https://docs.microsoft.com/en-us/dotnet/core/api/system.collections.generic.list-1) in the constructor.

_Any performance guidelines?_

If you're removing, modifying or inserting large amounts of items while having filtering and/or sorting set up, it's recommended that you use the `NotificationDeferrer` helper provided. It skips any performance heavy logic while it's in use, and automatically calls the `Refresh` method when disposed.

```csharp

    using (acv.DeferRefresh())
    {
        for (var i = 0; i < 500; i++)
        {
            acv.Add(new Person { Name = "defer" });
        }
    } // acv.Refresh() gets called here

```

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |

## API

* [AdvancedCollectionView source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.UI/AdvancedCollectionView)

