// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the Carousel control.
    /// </summary>
    public sealed partial class CarouselPage : Page, IXamlRenderListener
    {
        private Carousel carouselControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselPage"/> class.
        /// </summary>
        public CarouselPage()
        {
            InitializeComponent();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            carouselControl = control.FindDescendantByName("CarouselControl") as Carousel;
            carouselControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
        }
    }
}
