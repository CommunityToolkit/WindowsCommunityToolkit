// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the FocusBehavior
    /// </summary>
    public sealed partial class FocusBehaviorPage : Page, IXamlRenderListener
    {
        public FocusBehaviorPage() => InitializeComponent();

        public void OnXamlRendered(FrameworkElement control)
        {

        }
    }
}
