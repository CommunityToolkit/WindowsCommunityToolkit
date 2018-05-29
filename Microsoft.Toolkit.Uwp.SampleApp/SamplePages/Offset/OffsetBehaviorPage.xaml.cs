// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the offset behavior.
    /// </summary>
    public sealed partial class OffsetBehaviorPage : IXamlRenderListener
    {
        private Offset _offsetBehavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="OffsetBehaviorPage"/> class.
        /// </summary>
        public OffsetBehaviorPage()
        {
            InitializeComponent();

            SampleController.Current.RegisterNewCommand("Apply", (s, e) =>
            {
                _offsetBehavior?.StartAnimation();
            });
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (control.FindChildByName("EffectElement") is FrameworkElement element)
            {
                var behaviors = Interaction.GetBehaviors(element);
                _offsetBehavior = behaviors.FirstOrDefault(item => item is Offset) as Offset;
            }
        }
    }
}