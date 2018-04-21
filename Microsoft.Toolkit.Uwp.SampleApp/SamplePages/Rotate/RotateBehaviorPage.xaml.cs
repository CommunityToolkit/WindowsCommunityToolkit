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

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the Rotation behavior.
    /// </summary>
    public sealed partial class RotateBehaviorPage : IXamlRenderListener
    {
        private Rotate _rotateBehavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateBehaviorPage"/> class.
        /// </summary>
        public RotateBehaviorPage()
        {
            InitializeComponent();

            SampleController.Current.RegisterNewCommand("Apply", (s, e) =>
            {
                _rotateBehavior?.StartAnimation();
            });
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (control.FindChildByName("EffectElement") is FrameworkElement element)
            {
                var behaviors = Interaction.GetBehaviors(element);
                _rotateBehavior = behaviors.FirstOrDefault(item => item is Rotate) as Rotate;
            }
        }
    }
}