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
                int returnedFromUIThread = await DispatcherHelper.ExecuteOnUIThreadAsync<int>(() =>
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
