// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the <see cref="TransitionHelper"/> helper.
    /// </summary>
    public sealed partial class TransitionHelperPage : Page, IXamlRenderListener
    {
        private readonly TransitionHelper transitionHelper = new ();
        private FrameworkElement firstControl;
        private FrameworkElement secondControl;
        private FrameworkElement thirdControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionHelperPage"/> class.
        /// </summary>
        public TransitionHelperPage()
        {
            this.InitializeComponent();
            this.transitionHelper.AnimationConfigs = new AnimationConfig[]
            {
                new AnimationConfig
                {
                    Id = "background",
                    ScaleMode = ScaleMode.Scale
                },
                new AnimationConfig
                {
                    Id = "image",
                    ScaleMode = ScaleMode.Scale
                },
                new AnimationConfig
                {
                    Id = "guide",
                },
                new AnimationConfig
                {
                    Id = "name",
                    ScaleMode = ScaleMode.ScaleY
                },
                new AnimationConfig
                {
                    Id = "desc",
                },
            };
        }

        /// <inheritdoc/>
        public void OnXamlRendered(FrameworkElement control)
        {
            this.firstControl = control.FindChild("FirstControl");
            this.secondControl = control.FindChild("SecondControl");
            this.thirdControl = control.FindChild("ThirdControl");
            var minToMidButton = control.FindChild("MinToMidButton") as Button;
            var minToMaxButton = control.FindChild("MinToMaxButton") as Button;
            var midGoBackButton = control.FindChild("MidGoBackButton") as Button;
            var midToMinButton = control.FindChild("MidToMinButton") as Button;
            var midToMaxButton = control.FindChild("MidToMaxButton") as Button;
            var maxGoBackButton = control.FindChild("MaxGoBackButton") as Button;
            var maxToMinButton = control.FindChild("MaxToMinButton") as Button;
            var maxToMidButton = control.FindChild("MaxToMidButton") as Button;
            minToMidButton.Click += this.MinToMidButton_Click;
            minToMaxButton.Click += this.MinToMaxButton_Click;
            midToMinButton.Click += this.MidToMinButton_Click;
            midToMaxButton.Click += this.MidToMaxButton_Click;
            maxToMinButton.Click += this.MaxToMinButton_Click;
            maxToMidButton.Click += this.MaxToMidButton_Click;
            midGoBackButton.Click += this.GoBackButton_Click;
            maxGoBackButton.Click += this.GoBackButton_Click;
        }

        private void MaxToMidButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.thirdControl;
            this.transitionHelper.Target = this.secondControl;
            _ = this.transitionHelper.AnimateAsync();
        }

        private void MaxToMinButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.thirdControl;
            this.transitionHelper.Target = this.firstControl;
            _ = this.transitionHelper.AnimateAsync();
        }

        private void MidToMinButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.secondControl;
            this.transitionHelper.Target = this.firstControl;
            _ = this.transitionHelper.AnimateAsync();
        }

        private void MidToMaxButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.secondControl;
            this.transitionHelper.Target = this.thirdControl;
            _ = this.transitionHelper.AnimateAsync();
        }

        private void MinToMaxButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.firstControl;
            this.transitionHelper.Target = this.thirdControl;
            _ = this.transitionHelper.AnimateAsync();
        }

        private void MinToMidButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.firstControl;
            this.transitionHelper.Target = this.secondControl;
            _ = this.transitionHelper.AnimateAsync();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            _ = this.transitionHelper.ReverseAsync();
        }
    }
}