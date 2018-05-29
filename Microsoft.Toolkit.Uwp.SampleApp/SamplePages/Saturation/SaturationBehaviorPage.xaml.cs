// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A demonstration page of how you can use the Saturation effect using behaviors.
    /// </summary>
    public sealed partial class SaturationBehaviorPage : IXamlRenderListener
    {
        private Saturation _saturationBehavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaturationBehaviorPage"/> class.
        /// </summary>
        public SaturationBehaviorPage()
        {
            InitializeComponent();

            if (!AnimationExtensions.SaturationEffect.IsSupported)
            {
                WarningText.Visibility = Visibility.Visible;
            }

            SampleController.Current.RegisterNewCommand("Apply", (s, e) =>
            {
                _saturationBehavior?.StartAnimation();
            });
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (control.FindChildByName("EffectElement") is FrameworkElement element)
            {
                var behaviors = Interaction.GetBehaviors(element);
                _saturationBehavior = behaviors.FirstOrDefault(item => item is Saturation) as Saturation;
            }
        }
    }
}