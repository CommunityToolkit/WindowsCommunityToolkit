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