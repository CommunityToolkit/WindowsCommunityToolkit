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

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DispatcherHelperPage : Page
    {
        public DispatcherHelperPage()
        {
            this.InitializeComponent();
        }

        private async void ExecuteFromDifferentThreadButton_Click(object sender, RoutedEventArgs e)
        {
            int crossThreadReturnedValue = await Task.Run<int>(async () =>
            {
                int returnedFromOtherThread = await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    NormalTextBlock.Text = "Updated from a random thread!";

                    int value = await Task.Run(() => { return 1 + 1; });

                    return value;
                });

                return returnedFromOtherThread + 1;
            });

            await Task.Delay(200);

            NormalTextBlock.Text += $" And the value {crossThreadReturnedValue} was also returned successfully!";
        }
    }
}
