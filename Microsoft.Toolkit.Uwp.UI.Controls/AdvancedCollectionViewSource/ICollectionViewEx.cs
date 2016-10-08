using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.AdvancedCollectionViewSource
{
    /// <summary>
    /// Extended ICollectionView with filtering and sorting
    /// </summary>
    public interface ICollectionViewEx : ICollectionView
    {
        /// <summary>
        /// Gets a value indicating whether this CollectionView can filter its items
        /// </summary>
        bool CanFilter { get; }

        /// <summary>
        /// Gets or sets the predicate used to filter the visisble items
        /// </summary>
        Predicate<object> Filter { get; set; }

        /// <summary>
        /// Gets a value indicating whether this CollectionView can sort its items
        /// </summary>
        bool CanSort { get; }

        /// <summary>
        /// Gets SortDescriptions to sort the visible items
        /// </summary>
        IList<SortDescription> SortDescriptions { get; }

        /// <summary>
        /// Gets a value indicating whether this CollectionView can group its items
        /// </summary>
        bool CanGroup { get; }

        /// <summary>
        /// Gets GroupDescriptions to group the visible items
        /// </summary>
        IList<object> GroupDescriptions { get; }

        /// <summary>
        /// Gets the source collection
        /// </summary>
        IEnumerable SourceCollection { get; }

        /// <summary>
        /// Stops refreshing until it is disposed
        /// </summary>
        /// <returns>An disposable object</returns>
        IDisposable DeferRefresh();

        /// <summary>
        /// Manually refreshes the view
        /// </summary>
        void Refresh();
    }
}