// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// MicrosoftGraphUIExtensions Extension
    /// </summary>
    public static class MicrosoftGraphUIExtensions
    {
        /// <summary>
        /// Add a source collection of items to a destination collection
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="itemsSource">Source Collection</param>
        /// <param name="itemsDest">Destination Collection</param>
        public static void AddTo<T>(this ObservableCollection<T> itemsSource, ObservableCollection<T> itemsDest)
        {
            foreach (var item in itemsSource)
            {
                itemsDest.Add(item);
            }
        }
    }
}
