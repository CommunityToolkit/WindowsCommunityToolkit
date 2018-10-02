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
    /// A page that shows how to use the light behavior.
    /// </summary>
    public sealed partial class LightBehaviorPage : IXamlRenderListener
    {
        private Light _lightBehavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightBehaviorPage"/> class.
        /// </summary>
        public LightBehaviorPage()
        {
            this.InitializeComponent();

            if (!AnimationExtensions.IsLightingSupported)
            {
                WarningText.Visibility = Visibility.Visible;
            }

            SampleController.Current.RegisterNewCommand("Apply", (s, e) =>
            {
                _lightBehavior?.StartAnimation();
            });
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (control.FindChildByName("EffectElement") is FrameworkElement element)
            {
                var behaviors = Interaction.GetBehaviors(element);
                _lightBehavior = behaviors.FirstOrDefault(item => item is Light) as Light;
            }
        }
    }
}