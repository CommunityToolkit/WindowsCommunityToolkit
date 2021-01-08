// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An collection of <see cref="Case"/> to help with XAML interop.
    /// </summary>
    public class CaseCollection : IList<Case>, IEnumerable<Case> // TODO: Do we need this or can we use an ObservableCollection directly??? (Or is it useful to have it manage the registration of the child events?)
    {
        internal SwitchPresenter Parent { get; set; } // TODO: Can we remove Parent need here and just use events?

        private readonly List<Case> _internalList = new List<Case>();

        /// <inheritdoc/>
        public int Count => _internalList.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public Case this[int index] { get => _internalList[index]; set => Insert(index, value); }

        /// <summary>
        /// Raised when an animation has been added/removed or modified
        /// </summary>
        public event EventHandler CaseCollectionChanged;

        private void ValueChanged(object sender, EventArgs e)
        {
            CaseCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseCollection"/> class.
        /// </summary>
        public CaseCollection()
        {
        }

        /// <inheritdoc/>
        public int IndexOf(Case item)
        {
            return _internalList.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, Case item)
        {
            item.ValueChanged += ValueChanged;
            item.Parent = Parent;
            _internalList.Insert(index, item);
            CaseCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _internalList.Count)
            {
                var xcase = _internalList[index];
                xcase.ValueChanged -= ValueChanged;
                xcase.Parent = null;
            }

            _internalList.RemoveAt(index);
            CaseCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void Add(Case item)
        {
            item.ValueChanged += ValueChanged;
            item.Parent = Parent;
            _internalList.Add(item);
            CaseCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var xcase in _internalList)
            {
                xcase.ValueChanged -= ValueChanged;
                xcase.Parent = null;
            }

            _internalList.Clear();
            CaseCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public bool Contains(Case item)
        {
            return _internalList.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(Case[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public bool Remove(Case item)
        {
            var result = _internalList.Remove(item);
            if (result)
            {
                item.ValueChanged -= ValueChanged;
                item.Parent = null;
                CaseCollectionChanged?.Invoke(this, EventArgs.Empty);
            }

            return result;
        }

        /// <inheritdoc/>
        public IEnumerator<Case> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }
    }
}
