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

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// A collection view implementation that supports filtering, grouping, sorting and incremental loading
    /// </summary>
    public partial class AdvancedCollectionView
    {
        /// <summary>
        /// Stops refreshing until it is disposed
        /// </summary>
        /// <returns>An disposable object</returns>
        public IDisposable DeferRefresh()
        {
            return new NotificationDeferrer(this);
        }

        /// <summary>
        /// Notification deferrer helper class
        /// </summary>
        public class NotificationDeferrer : IDisposable
        {
            private readonly AdvancedCollectionView _acvs;
            private readonly object _currentItem;

            /// <summary>
            /// Initializes a new instance of the <see cref="NotificationDeferrer"/> class.
            /// </summary>
            /// <param name="acvs">Source ACVS</param>
            public NotificationDeferrer(AdvancedCollectionView acvs)
            {
                _acvs = acvs;
                _currentItem = _acvs.CurrentItem;
                _acvs._deferCounter++;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
                _acvs.MoveCurrentTo(_currentItem);
                _acvs._deferCounter--;
                _acvs.Refresh();
            }
        }
    }
}
