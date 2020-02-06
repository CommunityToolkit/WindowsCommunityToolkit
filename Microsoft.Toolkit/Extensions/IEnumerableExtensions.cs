using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// Set of extensions for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Provides similar function to <see href="https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.append?view=netstandard-2.0">Append</see> without copying the collection.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection.</typeparam>
        /// <param name="collection">Collection to append to.</param>
        /// <param name="item">Item to append.</param>
        /// <param name="includeNull">If false, will not add item if it's null to the result. Defaults to true.</param>
        /// <returns>In-place result of collection plus item.</returns>
        public static IEnumerable<T> ConcatItem<T>(this IEnumerable<T> collection, T item, bool includeNull = true)
        {
            foreach (var i in collection)
            {
                yield return i;
            }

            if (includeNull || item != null)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Similar to <see href="https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.concat?view=netstandard-2.0">Concat</see> put prepends the passed collection to the result without copying.
        /// </summary>
        /// <typeparam name="T">Type of items in collections.</typeparam>
        /// <param name="collection">Initial collection.</param>
        /// <param name="items">Items to prepend to collection.</param>
        /// <returns>Set of items passed as argument prepended to initial collection in-place.</returns>
        public static IEnumerable<T> Prefix<T>(this IEnumerable<T> collection, IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                yield return i;
            }

            foreach (var i in collection)
            {
                yield return i;
            }
        }

        /// <summary>
        /// Provides similar function to <see href="https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.prepend?view=netstandard-2.0">Prepend</see> without copying the collection.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection.</typeparam>
        /// <param name="collection">Collection to prepend to.</param>
        /// <param name="item">Item to prepend.</param>
        /// <param name="includeNull">If false, will not add item if it's null to the result. Defaults to true.</param>
        /// <returns>In-place result of collection prepended with item.</returns>
        public static IEnumerable<T> PrefixItem<T>(this IEnumerable<T> collection, T item, bool includeNull = true)
        {
            if (includeNull || item != null)
            {
                yield return item;
            }

            foreach (var i in collection)
            {
                yield return i;
            }
        }
    }
}
