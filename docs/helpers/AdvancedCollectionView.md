---
title: AdvancedCollectionView
author: nmetulev
description: The AdvancedCollectionView is a collection view implementation that support filtering, sorting and incremental loading. It's meant to be used in a viewmodel. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, AdvancedCollectionView
dev_langs:
  - csharp
  - vb
---

# AdvancedCollectionView

The [AdvancedCollectionView](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.advancedcollectionview) is a collection view implementation that support filtering, sorting and incremental loading. It's meant to be used in a viewmodel. 

## Usage

In your viewmodel instead of having a public [IEnumerable](https://docs.microsoft.com/dotnet/core/api/system.collections.generic.ienumerable-1) of some sort to be bound to an eg. [Listview](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Controls.ListView), create a public AdvancedCollectionView and pass your list in the constructor to it. If you've done that you can use the many useful features it provides:

* sorting your list using the `SortDirection` helper: specify any number of property names to sort on with the direction desired
* filtering your list using a [Predicate](https://docs.microsoft.com/dotnet/core/api/system.predicate-1): this will automatically filter your list only to the items that pass the check by the predicate provided
* deferring notifications using the `NotificationDeferrer` helper: with a convenient _using_ pattern you can increase performance while doing large-scale modifications in your list by waiting with updates until you've completed your work
* incremental loading: if your source collection supports the feature then AdvancedCollectionView will do as well (it simply forwards the calls)
* live shaping: when constructing the `AdvancedCollectionView` you may specify that the collection use live shaping. This means that the collection will re-filter or re-sort if there are changes to the sort properties or filter properties that are specified using `ObserveFilterProperty`

## Example

```csharp
using Microsoft.Toolkit.Uwp.UI;

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

// Set up the AdvancedCollectionView with live shaping enabled to filter and sort the original list
var acv = new AdvancedCollectionView(oc, true);

// Let's filter out the integers
int nul;
acv.Filter = x => !int.TryParse(((Person)x).Name, out nul);

// And sort ascending by the property "Name"
acv.SortDescriptions.Add(new SortDescription("Name", SortDirection.Ascending));

// Let's add a Person to the observable collection
var person = new Person { Name = "Aardvark" };
oc.Add(person);

// Our added person is now at the top of the list, but if we rename this person, we can trigger a re-sort
person.Name = "Zaphod"; // Now a re-sort is triggered and person will be last in the list

// AdvancedCollectionView can be bound to anything that uses collections. 
YourListView.ItemsSource = acv;
```
```vb
Imports Microsoft.Toolkit.Uwp.UI

' Grab a sample type
Public Class Person
    Public Property Name As String
End Class

' Set up the original list with a few sample items
Dim oc = New ObservableCollection(Of Person) From {
    New Person With {.Name = "Staff"},
    New Person With {.Name = "42"},
    New Person With {.Name = "Swan"},
    New Person With {.Name = "Orchid"},
    New Person With {.Name = "15"},
    New Person With {.Name = "Flame"},
    New Person With {.Name = "16"},
    New Person With {.Name = "Arrow"},
    New Person With {.Name = "Tempest"},
    New Person With {.Name = "23"},
    New Person With {.Name = "Pearl"},
    New Person With {.Name = "Hydra"},
    New Person With {.Name = "Lamp Post"},
    New Person With {.Name = "4"},
    New Person With {.Name = "Looking Glass"},
    New Person With {.Name = "8"}
}

' Set up the AdvancedCollectionView with live shaping enabled to filter and sort the original list
Dim acv = New AdvancedCollectionView(oc, True)

' Let's filter out the integers
Dim nul As Integer
acv.Filter = Function(x) Not Integer.TryParse((CType(x, Person)).Name, nul)

' And sort ascending by the property "Name"
acv.SortDescriptions.Add(New SortDescription("Name", SortDirection.Ascending))

' Let's add a Person to the observable collection
Dim person = New Person With {.Name = "Aardvark"}
oc.Add(person)

' Our added person is now at the top of the list, but if we rename this person, we can trigger a re-sort
person.Name = "Zaphod" ' Now a re-sort is triggered and person will be last in the list

' AdvancedCollectionView can be bound to anything that uses collections.
YourListView.ItemsSource = acv
```

## Properties

| Property | Type | Description |
| -- | -- | -- |
| CanFilter | bool | Gets a value indicating whether this CollectionView can filter its items |
| CanSort | bool | Gets a value indicating whether this CollectionView can sort its items |
| CollectionGroups | IObservableVector<object> | Gets the groups in collection |
| Count | int | Get the count of items |
| CurrentItem | object | Gets or sets the current item |
| CurrentPosition | int | Gets the position of current item |
| Filter | Predicate<object> | Gets or sets the predicate used to filter the visible items |
| HasMoreItems | bool | Gets a value indicating whether the source has more items |
| IsCurrentAfterLast | bool | Gets a value indicating whether the current item is after the last visible item |
| IsCurrentBeforeFirst | bool | Gets a value indicating whether the current item is before the first visible item |
| IsReadOnly | bool | Get a value indicating whether this CollectionView is read only |
| SortDescriptions | IList<[SortDescription](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.sortdescription)> | Gets SortDescriptions to sort the visible items |
| Source | IEnumerable | Gets or sets the source |
| SourceCollection | IEnumerable | Gets the source collection |
| this[int] | int | Gets or sets the element at the specified index |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Add(Object) | void | Add item |
| Clear() | void | Clear item |
| Contains(Object) | bool | Returns `true` if the given item contained in CollectionView |
| B(float, string) | int | Description |
| DeferRefresh() | IDisposable | Stops refreshing until it is disposed |
| IndexOf(Object) | int | Return index of an item |
| Insert(Int32, Object) | void | Insert an item in a particular place |
| LoadMoreItemsAsync(UInt32) | IAsyncOperation<[LoadMoreItemsResult](https://docs.microsoft.com/uwp/api/Windows.UI.Xaml.Data.LoadMoreItemsResult)> | Load more items from the source |
| MoveCurrentTo(Object) | bool | Move current index to item. Returns success of operation |
| MoveCurrentToFirst() | bool | Move current item to first item. Returns success of operation |
| MoveCurrentToLast() | bool | Move current item to last item. Returns success of operation |
| MoveCurrentToNext() | bool | Move current item to next item |
| MoveCurrentToPosition(Int32) | bool | Moves selected item to position |
| MoveCurrentToPrevious() | bool | Move current item to previous item |
| Refresh() | void | Manually refresh the view |
| Remove(Object) | bool | Remove item |
| RemoveAt(Int32) | bool | Remove item with index |

## Events

| Events | Description |
| -- | -- |
| CurrentChanged | Current item changed event handler |
| CurrentChanging | Current item changing event handler |
| PropertyChanged | Occurs when a property value changes |
| VectorChanged | Occurs when the vector changes |

## Remarks

_What source can I use?_

It's not necessary to use an eg. [ObservableCollection](https://docs.microsoft.com/dotnet/core/api/system.collections.objectmodel.observablecollection-1) to use the AdvancedCollectionView. It works as expected even when providing a simple [List](https://docs.microsoft.com/dotnet/core/api/system.collections.generic.list-1) in the constructor.

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
```vb
Using acv.DeferRefresh()
    For i = 0 To 500 - 1
        acv.Add(New Person With {.Name = "defer"})
    Next
End Using ' acv.Refresh() gets called here
```

## Sample Code

[AdvancedCollectionView sample page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/AdvancedCollectionView). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API

* [AdvancedCollectionView source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.UI/AdvancedCollectionView)
