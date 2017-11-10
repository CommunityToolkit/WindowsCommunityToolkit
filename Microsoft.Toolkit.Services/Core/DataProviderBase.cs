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
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Exceptions;
=======
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Exceptions;
using System.Net.Http;
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Base class for data providers in this library.
    /// </summary>
    /// <typeparam name="TConfig">Query configuration type for given provider.</typeparam>
    public abstract class DataProviderBase<TConfig>
    {
        /// <summary>
<<<<<<< HEAD
        /// Initializes a new instance of the <see cref="DataProviderBase{TConfig}"/> class.
        /// </summary>
        public DataProviderBase()
        {
=======
        /// Constructor
        /// </summary>
        public DataProviderBase()
        {
            
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        }

        /// <summary>
        /// Load data from provider endpoint.
        /// </summary>
        /// <typeparam name="TSchema">Strong typed object to parse the response items into.</typeparam>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <param name="parser">Parser to use for results.</param>
        /// <returns>Strong typed list of results.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, int pageIndex, IParser<TSchema> parser)
            where TSchema : SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }

            if (parser == null)
            {
                throw new ParserNullException();
            }

            ValidateConfig(config);

            var result = await GetDataAsync(config, maxRecords, pageIndex, parser);
            if (result != null)
            {
                return result
                    .Take(maxRecords)
                    .ToList();
            }

            return Array.Empty<TSchema>();
        }

        private static HttpClient httpClient;

        /// <summary>
<<<<<<< HEAD
        /// Gets or sets static instance of HttpClient.
=======
        /// Static instance of HttpClient.
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        /// </summary>
        public static HttpClient HttpClient
        {
            get { return httpClient ?? (httpClient = new HttpClient()); }
            set { httpClient = value; }
        }
<<<<<<< HEAD

=======
        
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        /// <summary>
        /// Derived classes will have to implement this method to return provider data
        /// </summary>
        /// <param name="config">Configuration to use</param>
        /// <param name="maxRecords">Maximum number of records to return</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <param name="parser">Parser to use</param>
        /// <typeparam name="TSchema">Schema defining data returned</typeparam>
        /// <returns>List of data</returns>
        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, int pageIndex, IParser<TSchema> parser)
            where TSchema : SchemaBase;

        /// <summary>
        /// Method provided by derived class to validate specified configuration
        /// </summary>
        /// <param name="config">Configuration to validate</param>
        protected abstract void ValidateConfig(TConfig config);
    }
}
