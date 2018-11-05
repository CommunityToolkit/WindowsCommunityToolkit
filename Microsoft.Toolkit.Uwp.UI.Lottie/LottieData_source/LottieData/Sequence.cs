// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// A sequence of items.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Sequence<T> : IEquatable<Sequence<T>>
    {
        static readonly string ItemTypeName = typeof(T).Name;
        readonly T[] _items;
        int _hashcode;

        public Sequence(IEnumerable<T> items)
        {
            _items = items.ToArray();
        }

        /// <summary>
        /// The items in the sequence.
        /// </summary>
        public IEnumerable<T> Items => _items;

        public bool Equals(Sequence<T> other) =>
            other != null &&
            Enumerable.SequenceEqual(_items, other.Items);

        public override bool Equals(object obj)
        {
            var other = obj as Sequence<T>;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            if (_hashcode == 0)
            {
                // Calculate the hashcode and cache it.
                // Hash doesn't have to be perfect, just needs to
                // be consistent, so to save some time just look at
                // the first few items.
                for (var i = 0; i < 3 && i < _items.Length; i++)
                {
                    _hashcode ^= _items[i].GetHashCode();
                }
            }
            return _hashcode;
        }

        public override string ToString() => $"{ItemTypeName}s: {string.Join(", ", Items)}";
    }
}
