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
        private uint _internalCount;

        /// <summary>
        /// Store a delegate which populate the next items
        ///  can be passed as a lambda
        /// </summary>
        private Func<CancellationToken, uint, Task<ObservableCollection<T>>> _func;

        /// <summary>
        /// Store the max items, Use by HasMoreItems method
        /// </summary>
        private uint _maxItems;

        /// <summary>
        /// No limit for the data, but becareful with the memory consumption
        /// </summary>
        private bool _isInfinite;

        /// <summary>
        /// Use to stop the virtualization and the incremental loading
        /// </summary>
        private CancellationToken _cancellationToken;

        /// <summary>
        ///  Initializes a new instance of the <see cref="IncrementalCollection{T}"/> class.
        /// </summary>
        /// <param name="func">The function to call which populate the IncrementalCollection with data of Type T</param>
        public IncrementalCollection(Func<CancellationToken, uint, Task<ObservableCollection<T>>> func)
            : this(func, 0)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="IncrementalCollection{T}"/> class.
        /// </summary>
        /// <param name="func">The function to call which populate the IncrementalCollection with data of Type T</param>
        /// <param name="maxItems">Maximum of items to populate in the IncrementalColletion</param>
        public IncrementalCollection(
            Func<CancellationToken, uint, Task<ObservableCollection<T>>> func,
            uint maxItems)
        {
            _func = func;
            if (maxItems == 0)
            {
                _isInfinite = true;
            }
            else
            {
                _maxItems = maxItems;
                _isInfinite = false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection has more available items
        /// <para>true if additional unloaded items remain in the view; otherwise, false.</para>
        /// </summary>
        public bool HasMoreItems
        {
            get
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (_isInfinite)
                {
                    return true;
                }

                return Count < _maxItems;
            }
        }

        /// <summary>
        /// Initializes incremental loading from the view.
        /// </summary>
        /// <param name="count">The number of items to load</param>
        /// <returns>The wrapped results of the load operation.</returns>
        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        /// <summary>
        /// Initializes incremental loading from the view.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="count">The number of items to load</param>
        /// <returns>The wrapped results of the load operation.</returns>
        public async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken cancellationToken, uint count)
        {
            ObservableCollection<T> intermediate = null;

            _cancellationToken = cancellationToken;

            var baseIndex = Count;
            uint numberOfItemsToGenerate = 0;

            if (!_isInfinite)
            {
                if (baseIndex + count < _maxItems)
                {
                    numberOfItemsToGenerate = count;
                }
                else
                {
                    numberOfItemsToGenerate = _maxItems - (uint)baseIndex;
                }
            }
            else
            {
                numberOfItemsToGenerate = count;
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                intermediate = await _func(cancellationToken, numberOfItemsToGenerate);
            }

            if (intermediate.Count == 0)
            {
                _maxItems = (uint)Count;
                _isInfinite = false;
            }
            else
            {
                intermediate.AddTo<T>(this);
            }

            _internalCount += count;
            return new LoadMoreItemsResult { Count = (uint)intermediate.Count };
        }
    }
}
