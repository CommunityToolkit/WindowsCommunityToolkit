// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Base class for data providers in this library.
    /// </summary>
    /// <typeparam name="TConfig">Strong typed query configuration object.</typeparam>
    /// <typeparam name="TSchema">Strong typed object to parse the response items into.</typeparam>
    public abstract class DataProviderBase<TConfig, TSchema> : DataProviderBase<TConfig>
        where TSchema : Parsers.SchemaBase
    {
        /// <summary>
        /// Load data from provider endpoint.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <returns>List of strong typed objects.</returns>
        public Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int maxRecords = 20, int pageIndex = 0)
        {
            return LoadDataAsync(config, maxRecords, pageIndex, GetDefaultParser(config));
        }

        /// <summary>
        /// Default parser abstract method.
        /// </summary>
        /// <param name="config">Query configuration object.</param>
        /// <returns>Strong typed default parser.</returns>
        protected abstract Parsers.IParser<TSchema> GetDefaultParser(TConfig config);
    }
}