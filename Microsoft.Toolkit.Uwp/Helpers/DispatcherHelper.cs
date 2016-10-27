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
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static methods helper for executing code in UI thread of the main window.
    /// </summary>
    public class DispatcherHelper
    {
        /// <summary>
        /// Execute the delegate on the UI thread of the current view.
        /// </summary>
        /// <param name="handler">Delegate to be executed on the current view / UI thread</param>
        public void ExecuteOnUIThread(DispatchedHandler handler)
        {
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView()?.CoreWindow?.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, handler);
        }
    }
}
