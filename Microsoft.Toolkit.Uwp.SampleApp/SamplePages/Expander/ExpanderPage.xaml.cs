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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the Expander control.
    /// </summary>
    public sealed partial class ExpanderPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpanderPage"/> class.
        /// </summary>
        public ExpanderPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
        }
    }
}
