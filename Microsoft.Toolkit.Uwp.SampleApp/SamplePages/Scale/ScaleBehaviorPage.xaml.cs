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
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the scale behavior.
    /// </summary>
    public sealed partial class ScaleBehaviorPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleBehaviorPage"/> class.
        /// </summary>
        public ScaleBehaviorPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            Shell.Current.RegisterNewCommand("Restore", async (sender, args) =>
            {
                await ToolkitLogo.Rotate(value: 0f).Scale(scaleX: 1.0f, scaleY: 1.0f).Offset(offsetX: 0).Blur(value: 0).StartAsync();
            });

            Shell.Current.RegisterNewCommand("Anim", async (sender, args) =>
            {
                await ToolkitLogo.Rotate(value: 90f).Scale(scaleX: 5.0f, scaleY: 1.0f).Offset(offsetX: 150).StartAsync();
            });

            Shell.Current.RegisterNewCommand("Anim 2", async (sender, args) =>
            {
                await ToolkitLogo.Rotate(value: 90f).Then().Scale(scaleX: 5.0f, scaleY: 1.0f).Then().Offset(offsetX: 150).StartAsync();
            });

            Shell.Current.RegisterNewCommand("Anim 3", async (sender, args) =>
            {
                await ToolkitLogo.Rotate(value: 90f).Then().Scale(scaleX: 5.0f, scaleY: 1.0f).Then().Offset(offsetX: 150).Then().Blur(value: 2).StartAsync();
            });

            Shell.Current.RegisterNewCommand("Anim 4", async (sender, args) =>
            {
                await ToolkitLogo.Rotate(value: 90f).StartAsync();
                await ToolkitLogo.Scale(scaleX: 5.0f, scaleY: 1.0f).StartAsync();
                await ToolkitLogo.Offset(offsetX: 150).StartAsync();
                await ToolkitLogo.Blur(value: 2).StartAsync();
            });

            Shell.Current.RegisterNewCommand("Anim 5", async (sender, args) =>
            {
                var anim = ToolkitLogo.Rotate(value: 90f).Then().Scale(scaleX: 5.0f, scaleY: 1.0f).Then().Blur(value: 2).Then().Offset(offsetX: 150);

                anim.DurationForAll(5000);

                await anim.StartAsync();
            });

            Shell.Current.RegisterNewCommand("Anim 6", (sender, args) =>
            {
                var anim = ToolkitLogo.Rotate(value: 90f).Then().Scale(scaleX: 5.0f, scaleY: 1.0f).Then().Blur(value: 2).Then().Offset(offsetX: 150);

                anim.Start();

                anim.Stop();
            });

            Shell.Current.RegisterNewCommand("Switch mode", (sender, args) =>
            {
                AnimationSet.AlwaysUseComposition = !AnimationSet.AlwaysUseComposition;
            });
        }
    }
}
