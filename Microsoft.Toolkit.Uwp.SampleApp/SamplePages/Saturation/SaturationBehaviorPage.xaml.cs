// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    using UI.Animations;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// A demonstration page of how you can use the Saturation effect using behaviors.
    /// </summary>
    public sealed partial class SaturationBehaviorPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaturationBehaviorPage"/> class.
        /// </summary>
        public SaturationBehaviorPage()
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

            if (!AnimationExtensions.SaturationEffect.IsSupported)
            {
                WarningText.Visibility = Visibility.Visible;
            }
        }
    }
}