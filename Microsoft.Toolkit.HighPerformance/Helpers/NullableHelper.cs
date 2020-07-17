using System;
using System.Runtime.CompilerServices;


namespace Microsoft.Toolkit.HighPerformance.Helpers
{
    /// <summary>
    /// Helpers to perform operations on nullable value types
    /// </summary>
    public static class NullableHelper
    {
        /// <summary>
        /// We use this type to publically expose the fields of a Nullable{T} type
        /// </summary>
        private struct NullableHelperMap<T>
            where T : struct
        {
            public bool HasValue;
            public T Value;
        }

        /// <summary>
        /// Returns a ref <typeparamref name="T"/> to the value of <paramref name="nullable"/> if <see cref="Nullable{T}.HasValue"/> is true,
        /// else, default(T)
        /// </summary>
        /// <typeparam name="T">The type of the underlying value</typeparam>
        /// <param name="nullable">The <see cref="Nullable{T}"/></param>
        /// <returns>A ref <typeparamref name="T"/> to the underlying value or default(T)</returns>
        public static ref T GetValueOrDefaultRef<T>(this ref T? nullable)
            where T : struct
        {
            return ref Unsafe.As<T?, NullableHelperMap<T>>(ref nullable).Value;
        }
    }
}