// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the offset behavior.
    /// </summary>
    public sealed partial class OnDevicePage : IXamlRenderListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnDevicePage"/> class.
        /// </summary>
        public OnDevicePage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
        }
    }
}