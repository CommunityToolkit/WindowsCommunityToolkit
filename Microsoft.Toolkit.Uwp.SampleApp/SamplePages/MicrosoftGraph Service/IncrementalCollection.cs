using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{

    public class IncrementalCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public bool IsEmtpty()
        {
            return this.Count == 0 ? true : false;
        }

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
            uint numberOfitemsTogenerate = 0;

            if (!isInfinite)
            {
                if (baseIndex + count < maxItems)
                {
                    numberOfitemsTogenerate = count;
                }
                else
                {
                    numberOfitemsTogenerate = maxItems - (uint)baseIndex;
                }
            }
            else
            {
                numberOfitemsTogenerate = count;
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                intermediate = await func(cancellationToken, numberOfitemsTogenerate);
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
