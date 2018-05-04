// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.Data
{
    [Bindable]
    public class DataGridDataSource
    {
        private static ObservableCollection<DataGridDataItem> _items;
        private static CollectionViewSource groupedItems;
        private string _cachedSortedColumn = string.Empty;

        // Loading data
        public async Task<IEnumerable<DataGridDataItem>> GetDataAsync()
        {
            var uri = new Uri($"ms-appx:///Assets/mtns.csv");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();
            _items = new ObservableCollection<DataGridDataItem>();

            using (StreamReader sr = new StreamReader(randomStream.AsStreamForRead()))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

                    _items.Add(
                        new DataGridDataItem()
                        {
                            Rank = uint.Parse(values[0]),
                            Mountain = values[1],
                            Height_m = uint.Parse(values[2]),
                            Range = values[3],
                            Coordinates = values[4],
                            Prominence = uint.Parse(values[5]),
                            Parent_mountain = values[6],
                            First_ascent = uint.Parse(values[7]),
                            Ascents = values[8]
                        });
                }
            }

            return _items;
        }

        // Sorting implementation using LINQ
        public string CachedSortedColumn
        {
            get
            {
                return _cachedSortedColumn;
            }

            set
            {
                _cachedSortedColumn = value;
            }
        }

        public ObservableCollection<DataGridDataItem> SortData(string sortBy, bool ascending)
        {
            _cachedSortedColumn = sortBy;
            switch (sortBy)
            {
                case "Rank":
                    if (ascending)
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Rank ascending
                                                                          select item);
                    }
                    else
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Rank descending
                                                                          select item);
                    }

                case "Parent_mountain":
                    if (ascending)
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Parent_mountain ascending
                                                                          select item);
                    }
                    else
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Parent_mountain descending
                                                                          select item);
                    }

                case "Mountain":
                    if (ascending)
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Mountain ascending
                                                                          select item);
                    }
                    else
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Mountain descending
                                                                          select item);
                    }

                case "Height_m":
                    if (ascending)
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Height_m ascending
                                                                          select item);
                    }
                    else
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Height_m descending
                                                                          select item);
                    }

                case "Range":
                    if (ascending)
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Range ascending
                                                                          select item);
                    }
                    else
                    {
                        return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                          orderby item.Range descending
                                                                          select item);
                    }
            }

            return _items;
        }

        // Grouping implementation using LINQ
        public CollectionViewSource GroupData()
        {
            ObservableCollection<GroupInfoCollection<DataGridDataItem>> groups = new ObservableCollection<GroupInfoCollection<DataGridDataItem>>();
            var query = from item in _items
                        orderby item
                        group item by item.Range into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoCollection<DataGridDataItem> info = new GroupInfoCollection<DataGridDataItem>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }

                groups.Add(info);
            }

            groupedItems = new CollectionViewSource();
            groupedItems.IsSourceGrouped = true;
            groupedItems.Source = groups;

            return groupedItems;
        }

        public class GroupInfoCollection<T> : ObservableCollection<T>
        {
            public object Key { get; set; }

            public new IEnumerator<T> GetEnumerator()
            {
                return (IEnumerator<T>)base.GetEnumerator();
            }
        }

        // Filtering implementation using LINQ
        public enum FilterOptions
        {
            Rank_Low = 0,
            Rank_High = 1,
            Height_Low = 2,
            Height_High = 3
        }

        public ObservableCollection<DataGridDataItem> FilterData(FilterOptions filterBy)
        {
            switch (filterBy)
            {
                case FilterOptions.Rank_Low:
                    return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                      where item.Rank < 50
                                                                      select item);

                case FilterOptions.Rank_High:
                    return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                      where item.Rank > 50
                                                                      select item);

                case FilterOptions.Height_High:
                    return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                      where item.Height_m > 8000
                                                                      select item);

                case FilterOptions.Height_Low:
                    return new ObservableCollection<DataGridDataItem>(from item in _items
                                                                      where item.Height_m < 8000
                                                                      select item);
            }

            return _items;
        }
    }
}
