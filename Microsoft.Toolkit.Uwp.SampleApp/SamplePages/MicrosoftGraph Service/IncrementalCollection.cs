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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Class which allows to load a subsets of data
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public class IncrementalCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        /// <summary>
        /// Store the number of items
        /// </summary>
        private uint internalCount;

        /// <summary>
        /// Store a delegate which populate the next items
        ///  can be passed as a lambda
        /// </summary>
        private Func<CancellationToken, uint, Task<ObservableCollection<T>>> func;

        /// <summary>
        /// Store the max items, Use by HasMoreItems method
        /// </summary>
        private uint maxItems;

        /// <summary>
        /// No limit for the data, but becareful with the memory consumption
        /// </summary>
        private bool isInfinite;

        /// <summary>
        /// Use to stop the virtualization and the incremental loading
        /// </summary>
        private CancellationToken cancellationToken;

        public IncrementalCollection(Func<CancellationToken, uint, Task<ObservableCollection<T>>> func)
            : this(func, 0)
        {
        }

        public IncrementalCollection(
            Func<CancellationToken, uint, Task<ObservableCollection<T>>> func,
            uint maxItems)
        {
            this.func = func;
            if (maxItems == 0)
            {
                isInfinite = true;
            }
            else
            {
                this.maxItems = maxItems;
                isInfinite = false;
            }
        }

        public bool HasMoreItems
        {
            get
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (isInfinite)
                {
                    return true;
                }

                return this.Count < maxItems;
            }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        public async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken cancellationToken, uint count)
        {
            ObservableCollection<T> intermediate = null;

            this.cancellationToken = cancellationToken;

            var baseIndex = this.Count;
            uint numberOfitemsToGenerate = 0;

            if (!isInfinite)
            {
                if (baseIndex + count < maxItems)
                {
                    numberOfitemsToGenerate = count;
                }
                else
                {
                    numberOfitemsToGenerate = maxItems - (uint)baseIndex;
                }
            }
            else
            {
                numberOfitemsToGenerate = count;
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                intermediate = await func(cancellationToken, numberOfitemsToGenerate);
            }

            if (intermediate.Count == 0)
            {
                maxItems = (uint)this.Count;
                isInfinite = false;
            }
            else
            {
                intermediate.AddTo<T>(this);
            }

            internalCount += count;
            return new LoadMoreItemsResult { Count = (uint)intermediate.Count };
        }
    }
}
