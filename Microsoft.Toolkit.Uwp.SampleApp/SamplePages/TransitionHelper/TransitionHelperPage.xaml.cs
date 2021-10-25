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
        private Button secondButton;
        private Button secondBackButton;
        private Button thirdButton;

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
                    AdditionalAnimations = new AnimationTarget[]
                    {
                        AnimationTarget.Scale,
                    },
                },
                new AnimationConfig
                {
                    Id = "image",
                    AdditionalAnimations = new AnimationTarget[]
                    {
                        AnimationTarget.Scale,
                    },
                },
                new AnimationConfig
                {
                    Id = "guide",
                },
                new AnimationConfig
                {
                    Id = "name",
                    AdditionalAnimations = new AnimationTarget[]
                    {
                        AnimationTarget.ScaleY,
                    },
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
            this.secondButton = control.FindChild("SecondButton") as Button;
            this.secondBackButton = control.FindChild("SecondBackButton") as Button;
            this.thirdButton = control.FindChild("ThirdButton") as Button;
            this.firstControl.Tapped += this.FirstControl_Tapped;
            this.secondBackButton.Click += this.SecondBackButton_Click;
            this.secondButton.Click += this.SecondButton_Click;
            this.thirdButton.Click += this.ThirdButton_Click;
        }

        private void FirstControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.transitionHelper.Source = this.firstControl;
            this.transitionHelper.Target = this.secondControl;
            this.transitionHelper.IgnoredOrUnpairedElementHideTranslation = new Vector3(20, 0, 0);
            _ = this.transitionHelper.AnimateAsync();
        }

        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            _ = this.transitionHelper.ReverseAsync();
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.secondControl;
            this.transitionHelper.Target = this.thirdControl;
            this.transitionHelper.IgnoredOrUnpairedElementHideTranslation = new Vector3(0, 20, 0);
            _ = this.transitionHelper.AnimateAsync();
        }

        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            this.transitionHelper.Source = this.thirdControl;
            this.transitionHelper.Target = this.firstControl;
            _ = this.transitionHelper.AnimateAsync();
        }
    }
}