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
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// A collection view implementation that supports filtering, grouping, sorting and incremental loading
    /// </summary>
    public partial class AdvancedCollectionView
    {
        /// <summary>
        /// Currently selected item changing event
        /// </summary>
        /// <param name="e">event args</param>
        private void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            if (_deferCounter > 0)
            {
                return;
            }

            CurrentChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Currently selected item changed event
        /// </summary>
        /// <param name="e">event args</param>
        private void OnCurrentChanged(object e)
        {
            if (_deferCounter > 0)
            {
                return;
            }

            CurrentChanged?.Invoke(this, e);

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(CurrentItem));
        }

        /// <summary>
        /// Vector changed event
        /// </summary>
        /// <param name="e">event args</param>
        private void OnVectorChanged(IVectorChangedEventArgs e)
        {
            if (_deferCounter > 0)
            {
                return;
            }

            VectorChanged?.Invoke(this, e);

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Count));
        }
    }
}
