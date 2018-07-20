// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using Windows.Foundation.Collections;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    [ContentWrapper(typeof(DependencyObject))]
    public class DependencyObjectCollection : DependencyObject, IObservableVector<DependencyObject>, IList<DependencyObject>, IEnumerable<DependencyObject>
    {
        public event VectorChangedEventHandler<DependencyObject> VectorChanged;

        private readonly global::Windows.Foundation.Collections.IObservableVector<global::Windows.UI.Xaml.DependencyObject> underlyingCollection;

        internal DependencyObjectCollection(global::Windows.Foundation.Collections.IObservableVector<global::Windows.UI.Xaml.DependencyObject> underlying)
        {
            underlyingCollection = underlying;
            VectorChanged -= null;
        }

        private void OnVectorChanged()
        {
            VectorChanged?.Invoke(null, null);
        }

        public int IndexOf(DependencyObject item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, DependencyObject item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public DependencyObject this[int index]
        {
            get => throw new System.NotSupportedException();
            set => throw new System.NotSupportedException();
        }

        public void Add(DependencyObject item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(DependencyObject item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(DependencyObject[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(DependencyObject item)
        {
            throw new System.NotImplementedException();
        }

        public int Count => throw new System.NotSupportedException();

        public bool IsReadOnly => throw new System.NotSupportedException();

        public IEnumerator<DependencyObject> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
