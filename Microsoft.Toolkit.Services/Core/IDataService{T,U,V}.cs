// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Generic interface that all deployed service providers implement.
    /// </summary>
    /// <typeparam name="T">Reference to underlying data service provider.</typeparam>
    /// <typeparam name="U">Strongly-typed schema for data returned in list query.</typeparam>
    /// <typeparam name="V">Configuration type specifying query parameters.</typeparam>
    public interface IDataService<T, U, V>
    {
        /// <summary>
        /// Gets the underlying data service provider.
        /// </summary>
        T Provider { get; }

        /// <summary>
        /// Makes a request for a list of data from the given service provider.
        /// </summary>
        /// <param name="config">Describes the query on the list data request.</param>
        /// <param name="maxRecords">Specifies an upper limit to the number of records returned.</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <returns>Returns a strongly typed list of results from the service.</returns>
        Task<List<U>> RequestAsync(V config, int maxRecords, int pageIndex = 0);
    }
}
