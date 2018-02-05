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
using System.Collections;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Sort description
    /// </summary>
    public class SortDescription
    {
        /// <summary>
        /// Gets the name of property to sort on
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the direction of sort
        /// </summary>
        public SortDirection Direction { get; }

        /// <summary>
        /// Gets the comparer
        /// </summary>
        public IComparer Comparer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescription"/> class that describes
        /// a sort on the object itself
        /// </summary>
        /// <param name="direction">Direction of sort</param>
        /// <param name="comparer">Comparer to use. If null, will use default comparer</param>
        public SortDescription(SortDirection direction, IComparer comparer = null)
            : this(null, direction, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescription"/> class.
        /// </summary>
        /// <param name="propertyName">Name of property to sort on</param>
        /// <param name="direction">Direction of sort</param>
        /// <param name="comparer">Comparer to use. If null, will use default comparer</param>
        public SortDescription(string propertyName, SortDirection direction, IComparer comparer = null)
        {
            PropertyName = propertyName;
            Direction = direction;
            Comparer = comparer ?? ObjectComparer.Instance;
        }

        private class ObjectComparer : IComparer
        {
            public static readonly IComparer Instance = new ObjectComparer();

            private ObjectComparer()
            {
            }

            public int Compare(object x, object y)
            {
                var cx = x as IComparable;
                var cy = y as IComparable;

                // ReSharper disable once PossibleUnintendedReferenceComparison
                return cx == cy ? 0 : cx == null ? -1 : cy == null ? +1 : cx.CompareTo(cy);
            }
        }
    }
}