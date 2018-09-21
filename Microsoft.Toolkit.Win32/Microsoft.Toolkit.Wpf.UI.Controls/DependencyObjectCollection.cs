// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using Windows.Foundation.Collections;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    [ContentWrapper(typeof(DependencyObject))]
    public class DependencyObjectCollection : DependencyObject, IObservableVector<DependencyObject>
    {
        public event VectorChangedEventHandler<DependencyObject> VectorChanged;

        private readonly IObservableVector<Windows.UI.Xaml.DependencyObject> _underlyingCollection;

        internal DependencyObjectCollection(IObservableVector<Windows.UI.Xaml.DependencyObject> underlying)
        {
            _underlyingCollection = underlying;
            VectorChanged -= null;
        }

        private void OnVectorChanged()
        {
            VectorChanged?.Invoke(null, null);
        }

        public int IndexOf(DependencyObject item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, DependencyObject item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public DependencyObject this[int index]
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public void Add(DependencyObject item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(DependencyObject item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(DependencyObject[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(DependencyObject item)
        {
            throw new NotImplementedException();
        }

        public int Count => throw new NotSupportedException();

        public bool IsReadOnly => throw new NotSupportedException();

        public IEnumerator<DependencyObject> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
