// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the <see cref="TransitionHelper"/> helper.
    /// </summary>
    public sealed partial class TransitionHelperPage : Page, IXamlRenderListener
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionHelperPage"/> class.
        /// </summary>
        public TransitionHelperPage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public void OnXamlRendered(FrameworkElement control)
        {
        }
    }
}