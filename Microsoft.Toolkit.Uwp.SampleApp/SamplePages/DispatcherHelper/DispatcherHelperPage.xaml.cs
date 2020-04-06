// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class DispatcherHelperPage
    {
        public DispatcherHelperPage()
        {
            this.InitializeComponent();
        }

        private async void ExecuteFromDifferentThreadButton_Click(object sender, RoutedEventArgs e)
        {
            int crossThreadReturnedValue = await Task.Run<int>(async () =>
            {
#pragma warning disable CS0612 // Type or member is obsolete
                int returnedFromUIThread = await DispatcherHelper.ExecuteOnUIThreadAsync<int>(() =>
#pragma warning restore CS0612 // Type or member is obsolete
                {
                    NormalTextBlock.Text = "Updated from a random thread!";
                    return 1;
                });

                return returnedFromUIThread + 1;
            });

            await Task.Delay(200);

            NormalTextBlock.Text += $" And the value {crossThreadReturnedValue} was also returned successfully!";
        }
    }
}
