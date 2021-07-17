// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageSurfaceBrushPage : Page
    {
        public ImageSurfaceBrushPage()
        {
            this.InitializeComponent();

            StretchOptions.ItemsSource = new List<string>
            {
                "None",
                "Fill",
                "Uniform",
                "UniformToFill"
            };

            HAOptions.ItemsSource = new List<string>
            {
                "Left",
                "Center",
                "Right"
            };

            VAOptions.ItemsSource = new List<string>
            {
                "Top",
                "Center",
                "Bottom"
            };

            StretchOptions.SelectionChanged += OnStretchOptionsChanged;
            StretchOptions.SelectedIndex = 2;

            HAOptions.SelectionChanged += OnHorizontalAlignmentChanged;
            HAOptions.SelectedIndex = 1;

            VAOptions.SelectionChanged += OnVerticalAlignmentChanged;
            VAOptions.SelectedIndex = 1;
        }

        private void OnStretchOptionsChanged(object sender, SelectionChangedEventArgs e)
        {
            var stretch = StretchOptions.SelectedValue as string;
            if (!string.IsNullOrWhiteSpace(stretch))
            {
                if (Enum.TryParse(typeof(Stretch), stretch, out object stretchEnum))
                {
                    ImageOptions.Stretch = (Stretch)stretchEnum;
                }
            }
        }

        private void OnHorizontalAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            var align = HAOptions.SelectedValue as string;
            if (!string.IsNullOrWhiteSpace(align))
            {
                if (Enum.TryParse(typeof(AlignmentX), align, out object alignEnum))
                {
                    ImageOptions.HorizontalAlignment = (AlignmentX)alignEnum;
                }
            }
        }

        private void OnVerticalAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            var align = VAOptions.SelectedValue as string;
            if (!string.IsNullOrWhiteSpace(align))
            {
                if (Enum.TryParse(typeof(AlignmentY), align, out object alignEnum))
                {
                    ImageOptions.VerticalAlignment = (AlignmentY)alignEnum;
                }
            }
        }
    }
}
