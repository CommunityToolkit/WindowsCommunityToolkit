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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class represents an <see cref="ObservableCollection{IType}"/> whose items can be loaded incrementally using the specified arguments as input (for example, they can be a search parameter).
    /// </summary>
    /// <typeparam name="TSource">
    /// The data source that must be loaded incrementally.
    /// </typeparam>
    /// <typeparam name="IType">
    /// The type of collection items.
    /// </typeparam>
    /// <typeparam name="TArgs">
    /// Type of input arguments.
    /// </typeparam>
    /// <seealso cref="IIncrementalSource{TSource,TArgs}"/>
    /// <seealso cref="ISupportIncrementalLoading"/>
    public class IncrementalLoadingCollection<TSource, IType, TArgs> : IncrementalLoadingCollection<TSource, IType>
        where TSource : IIncrementalSource<IType, TArgs>, new()
    {
        private readonly Func<Task<TArgs>> _onGetArguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalLoadingCollection{TSource, IType, TArgs}"/> class, specifiyng a method that will be invoked to provide additional parameters for incremental loading (for example, a search query) and, optionally, how many items to load for each data page.
        /// </summary>
        /// <param name="onGetArguments">
        /// An asynchronous <see cref="Task"/> that will be invoked to provide additional parameters for incremental loading (for example, a search query).
        /// </param>
        /// <param name="itemsPerPage">
        /// The number of items to retrieve for each call. Default is 20.
        /// </param>
        /// <param name="onStartLoading">
        /// An <see cref="Action"/> that is called when a retrieval operation begins.
        /// </param>
        /// <param name="onEndLoading">
        /// An <see cref="Action"/> that is called when a retrieval operation ends.
        /// </param>
        /// <param name="onError">
        /// An <see cref="Action"/> that is called if an error occours during data retrieval.
        /// </param>
        public IncrementalLoadingCollection(Func<Task<TArgs>> onGetArguments, int itemsPerPage = 20, Action onStartLoading = null, Action onEndLoading = null, Action<Exception> onError = null)
            : base(itemsPerPage, onStartLoading, onEndLoading, onError)
        {
            _onGetArguments = onGetArguments;
        }

        /// <summary>
        /// Actually performs the incremental loading using the specified arguments as input.
        /// </summary>
        /// <param name="cancellationToken">
        /// Used to propagate notification that operation should be canceled.
        /// </param>
        /// <returns>
        /// Returns a collection of <typeparamref name="IType"/>.
        /// </returns>
        protected override async Task<IEnumerable<IType>> LoadDataAsync(CancellationToken cancellationToken)
        {
            // Before call the actual pagination method, sets the source arguments.
            var args = await _onGetArguments.Invoke();
            Source.Arguments = args;

            var result = await Source.GetPagedItemsAsync(CurrentPageIndex++, ItemsPerPage, cancellationToken);
            return result;
        }
    }
}
