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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the FadeHeaderBehavior
    /// </summary>
    public sealed partial class FadeHeaderBehaviorPage : Page, IXamlRenderListener
    {
        private ListView myListView;

        public FadeHeaderBehaviorPage()
        {
            InitializeComponent();

            // If you wanted to use C# instead of XAML to attach the behavior, you can do it like this
            // Interaction.GetBehaviors(MyListView).Add(new FadeHeaderBehavior());
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            myListView = control.FindChildByName("MyListView") as ListView;

            // Load the ListView with Sample Data
            if (myListView != null)
            {
                myListView.ItemsSource = GenerateItems();
            }
        }

        /// <summary>
        /// Generates sample data for the ListView
        /// </summary>
        /// <returns>List of strings titles using the loop number that generated the item</returns>
        private static List<string> GenerateItems()
        {
            var list = new List<string>();

            for (var i = 1; i < 21; i++)
            {
                list.Add($"Item {i}");
            }

            return list;
        }
    }
}
