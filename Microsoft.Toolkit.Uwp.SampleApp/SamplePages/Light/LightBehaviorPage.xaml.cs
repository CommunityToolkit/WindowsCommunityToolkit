// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            else
            {
                NoCreatorUpdateWarningText.Visibility = Visibility.Visible;
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