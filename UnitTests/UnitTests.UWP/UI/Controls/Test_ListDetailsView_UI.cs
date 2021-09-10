// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_ListDetailsView_UI : VisualUITestBase
    {
        private const string SampleXaml = @"<controls:ListDetailsView
                                                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                                    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls""
                                                    NoSelectionContent=""No item selected"" >
                                                    <controls:ListDetailsView.ItemTemplate>
                                                        <DataTemplate>
                                                            <TextBlock Text=""Item"" />
                                                        </DataTemplate>
                                                    </controls:ListDetailsView.ItemTemplate>
                                                    <controls:ListDetailsView.DetailsTemplate>
                                                        <DataTemplate>
                                                            <TextBox Text=""{Binding}"" />
                                                        </DataTemplate>
                                                    </controls:ListDetailsView.DetailsTemplate>
                                                 </controls:ListDetailsView>";

        [TestCategory("ListDetailsView")]
        [TestMethod]
        public async Task Test_LoseFocusOnNoSelection()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var listDetailsView = XamlReader.Load(SampleXaml) as ListDetailsView;

                listDetailsView.ItemsSource = new ObservableCollection<string>
                {
                     "First",
                };

                listDetailsView.SelectedIndex = 0;
                
                await SetTestContentAsync(listDetailsView);

                await Task.Delay(1000);

                var firsttb = listDetailsView.FindDescendant<TextBox>();

                firsttb.Focus(FocusState.Programmatic);

                await Task.Delay(1000);

                var firstLostFocus = false;

                firsttb.LostFocus += (s, e) => firstLostFocus = true;

                listDetailsView.SelectedIndex = -1;

                await Task.Delay(1000);

                Assert.IsTrue(firstLostFocus, "TextBox in the first item should have lost focus.");
            });
        }

        [TestCategory("ListDetailsView")]
        [TestMethod]
        public async Task Test_LoseFocusOnSelectOther()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var listDetailsView = XamlReader.Load(SampleXaml) as ListDetailsView;

                listDetailsView.ItemsSource = new ObservableCollection<string>
                {
                     "First",
                     "Second",
                };

                listDetailsView.SelectedIndex = 0;

                await SetTestContentAsync(listDetailsView);

                await Task.Delay(1000);

                var firsttb = listDetailsView.FindDescendant<TextBox>();

                firsttb.Focus(FocusState.Programmatic);

                await Task.Delay(1000);

                var firstLostFocus = false;

                firsttb.LostFocus += (s, e) => firstLostFocus = true;

                listDetailsView.SelectedIndex = 1;

                await Task.Delay(1000);

                Assert.IsTrue(firstLostFocus, "TextBox in the first item should have lost focus.");
            });
        }
    }
}
