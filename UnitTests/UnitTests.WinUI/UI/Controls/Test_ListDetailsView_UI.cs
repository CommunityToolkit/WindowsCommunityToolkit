// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_ListDetailsView_UI : VisualUITestBase
    {
        private const string SampleXaml = @"<controls:ListDetailsView
                                                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                                    xmlns:controls=""using:CommunityToolkit.WinUI.UI.Controls""
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

                var firsttb = listDetailsView.FindDescendant<TextBox>();

                await App.DispatcherQueue.EnqueueAsync(() => firsttb.Focus(FocusState.Programmatic));

                Assert.AreEqual(firsttb, FocusManager.GetFocusedElement(), "TextBox didn't get focus");

                var tcs = new TaskCompletionSource<bool>();

                firsttb.LostFocus += (s, e) => tcs.SetResult(true);

                listDetailsView.SelectedIndex = -1;

                await Task.WhenAny(tcs.Task, Task.Delay(2000));

                Assert.IsTrue(tcs.Task.IsCompleted);
                Assert.IsTrue(tcs.Task.Result, "TextBox in the first item should have lost focus.");
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

                var firsttb = listDetailsView.FindDescendant<TextBox>();

                await App.DispatcherQueue.EnqueueAsync(() => firsttb.Focus(FocusState.Programmatic));

                Assert.AreEqual(firsttb, FocusManager.GetFocusedElement(), "TextBox didn't get focus");

                var tcs = new TaskCompletionSource<bool>();

                firsttb.LostFocus += (s, e) => tcs.SetResult(true);

                listDetailsView.SelectedIndex = 1;

                await Task.WhenAny(tcs.Task, Task.Delay(2000));

                Assert.IsTrue(tcs.Task.IsCompleted);
                Assert.IsTrue(tcs.Task.Result, "TextBox in the first item should have lost focus.");
            });
        }
    }
}
