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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;

namespace Microsoft.Toolkit.Uwp.Services.Bing
{
    /// <summary>
    /// Class for connecting to Bing.
    /// </summary>
    public class BingService : Toolkit.Services.Bing.BingService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingService"/> class.
        /// </summary>
        /// <param name="config">BingSearchConfig instance.</param>
        protected BingService(Toolkit.Services.Bing.BingSearchConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Gets an instance of <see cref="IncrementalLoadingCollection{TSource, IType}"/> class that is able to load search data incrementally.
        /// </summary>
        /// <param name="config">BingSearchConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>An instance of <see cref="IncrementalLoadingCollection{TSource, IType}"/> class that is able to load search data incrementally.</returns>
        public static IncrementalLoadingCollection<BingService, Toolkit.Services.Bing.BingResult> GetAsIncrementalLoading(Toolkit.Services.Bing.BingSearchConfig config, int maxRecords = 20)
        {
            var service = new BingService(config);
            return new IncrementalLoadingCollection<BingService, Toolkit.Services.Bing.BingResult>(service, maxRecords);
        }
    }
}
