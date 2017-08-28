---
title: Incremental Loading Collection Helpers
author: nmetulev
ms.date: 08/20/2017
description: The IncrementalLoadingCollection helpers greatly simplify the definition and usage of collections whose items can be loaded incrementally only when needed by the view
keywords: windows 10, uwp, uwp community toolkit, uwp toolkit, IncrementalLoadingCollection
---

# Incremental Loading Collection Helpers

The **IncrementalLoadingCollection** helpers greatly simplify the definition and usage of collections whose items can be loaded incrementally only when needed by the view, i.e., when user scrolls a [ListView](https://msdn.microsoft.com/library/windows/apps/windows.ui.xaml.controls.listview.aspx) or a [GridView](https://msdn.microsoft.com/library/windows/apps/windows.ui.xaml.controls.gridview.aspx).

| Helper | Purpose |
| --- | --- |
|IIncrementalSource | An interface that represents a data source whose items can be loaded incrementally. |
|IncrementalLoadingCollection | An extension of [ObservableCollection](https://msdn.microsoft.com/library/ms668604.aspx) such that its items are loaded only when needed. |

## Example
`IIncrementalSource` allows to define the data source:

```csharp
    // Be sure to include the using at the top of the file:
    //using Microsoft.Toolkit.Uwp;

    public class Person
    {
        public string Name { get; set; }
    }

    public class PeopleSource : IIncrementalSource<Person>
    {
        private readonly List<Person> people;

        public PeopleSource()
        {
            // Creates an example collection.
            people = new List<Person>();

            for (int i = 1; i <= 200; i++)
            {
                var p = new Person { Name = "Person " + i };
                people.Add(p);
            }
        }

        public async Task<IEnumerable<Person>> GetPagedItemsAsync(int pageIndex, int pageSize)
        {
            // Gets items from the collection according to pageIndex and pageSize parameters.
            var result = (from p in people
                            select p).Skip(pageIndex * pageSize).Take(pageSize);

            // Simulates a longer request...
            await Task.Delay(1000);

            return result;
        }
    }
```

The *GetPagedItemsAsync* method is invoked everytime the view need to show more items.

`IncrementalLoadingCollection` can then be bound to a [ListView](https://msdn.microsoft.com/library/windows/apps/windows.ui.xaml.controls.listview.aspx) or a [GridView-like](https://msdn.microsoft.com/library/windows/apps/windows.ui.xaml.controls.gridview.aspx) control:

```csharp
    var collection = new IncrementalLoadingCollection<PeopleSource, Person>();
    PeopleListView.ItemsSource = collection;
```

The **IncrementalLoadingCollection** constructor accepts the following arguments:

| Name | Description | Type |
| --- | --- | --- |
| source | An implementation of the **IIncrementalSource** interface that contains the logic to actually load data incrementally. If the source isn't provided to the constructor, it is created automatically. | IIncrementalSource |  
| itemsPerPage | The number of items to retrieve for each call. Default is 20. | [Integer](https://msdn.microsoft.com/library/windows/apps/System.Int32) |  
| onStartLoading | (optional) An Action that is called when a retrieval operation begins. | [Action](https://msdn.microsoft.com/library/system.action.aspx) |  
| onEndLoading | (optional) An Action that is called when a retrieval operation ends. | [Action](https://msdn.microsoft.com/library/system.action.aspx) |  
| onError | (optional) An Action that is called if an error occours during data retrieval. | [Action](https://msdn.microsoft.com/library/system.action.aspx) |  

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.14393.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [IncrementalLoadingCollection source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp/IncrementalLoadingCollection)


