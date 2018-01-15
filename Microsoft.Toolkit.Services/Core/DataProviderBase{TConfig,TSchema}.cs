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
        where TSchema : SchemaBase
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
        protected abstract IParser<TSchema> GetDefaultParser(TConfig config);
    }
}
