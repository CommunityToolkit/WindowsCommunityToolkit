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
        private readonly TransitionHelper _transitionHelper = new();
        private FrameworkElement _firstControl;
        private FrameworkElement _secondControl;
        private FrameworkElement _thirdControl;
        private FrameworkElement _secondNameTextBlock;
        private FrameworkElement _secondDescTextBlock;
        private FrameworkElement _thirdNameTextBlock;
        private FrameworkElement _thirdDescTextBlock;
        private Button _secondButton;
        private Button _secondBackButton;
        private Button _thirdButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionHelperPage"/> class.
        /// </summary>
        public TransitionHelperPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            this._firstControl = control.FindChild("FirstControl");
            this._secondControl = control.FindChild("SecondControl");
            this._thirdControl = control.FindChild("ThirdControl");
            this._secondNameTextBlock = control.FindChild("SecondNameTextBlock");
            this._secondDescTextBlock = control.FindChild("SecondDescTextBlock");
            this._thirdNameTextBlock = control.FindChild("ThirdNameTextBlock");
            this._thirdDescTextBlock = control.FindChild("ThirdDescTextBlock");
            this._secondButton = control.FindChild("SecondButton") as Button;
            this._secondBackButton = control.FindChild("SecondBackButton") as Button;
            this._thirdButton = control.FindChild("ThirdButton") as Button;
            this._firstControl.Tapped += FirstControl_Tapped;
            this._secondBackButton.Click += SecondBackButton_Click;
            this._secondButton.Click += SecondButton_Click;
            this._thirdButton.Click += ThirdButton_Click;
        }

        private void FirstControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _transitionHelper.AnimationConfigs = new AnimationConfig[]
            {
                new AnimationConfig
                {
                    Id = "background"
                },
                new AnimationConfig
                {
                    Id = "image",
                    AdditionalAnimations = new AnimationTarget[] { AnimationTarget.Scale }
                },
                new AnimationConfig
                {
                    Id = "guide"
                },
            };
            _transitionHelper.Source = _firstControl;
            _transitionHelper.Target = _secondControl;
            _transitionHelper.IgnoredElementHideTranslation = new Vector3(20, 0, 0);
            TransitionHelper.SetIsIgnored(_secondNameTextBlock, true);
            TransitionHelper.SetIsIgnored(_secondDescTextBlock, true);
            _ = _transitionHelper.AnimateAsync();
        }

        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _transitionHelper.ReverseAsync();
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            _transitionHelper.AnimationConfigs = new AnimationConfig[]
            {
                new AnimationConfig
                {
                    Id = "background"
                },
                new AnimationConfig
                {
                    Id = "image",
                    AdditionalAnimations = new AnimationTarget[] { AnimationTarget.Scale }
                },
                new AnimationConfig
                {
                    Id = "guide"
                },
                new AnimationConfig
                {
                    Id = "name",
                },
                new AnimationConfig
                {
                    Id = "desc",
                },
            };
            _transitionHelper.Source = _secondControl;
            _transitionHelper.Target = _thirdControl;
            _transitionHelper.IgnoredElementHideTranslation = new Vector3(0, 20, 0);
            TransitionHelper.SetIsIgnored(_secondNameTextBlock, false);
            TransitionHelper.SetIsIgnored(_secondDescTextBlock, false);
            _ = _transitionHelper.AnimateAsync();
        }

        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            _transitionHelper.AnimationConfigs = new AnimationConfig[]
            {
                new AnimationConfig
                {
                    Id = "background"
                },
                new AnimationConfig
                {
                    Id = "image",
                    AdditionalAnimations = new AnimationTarget[] { AnimationTarget.Scale }
                },
                new AnimationConfig
                {
                    Id = "guide"
                }
            };
            _transitionHelper.Source = _thirdControl;
            _transitionHelper.Target = _firstControl;
            TransitionHelper.SetIsIgnored(_thirdNameTextBlock, true);
            TransitionHelper.SetIsIgnored(_thirdDescTextBlock, true);
            _ = _transitionHelper.AnimateAsync();
        }
    }
}