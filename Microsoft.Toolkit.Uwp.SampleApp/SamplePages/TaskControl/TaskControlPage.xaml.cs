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

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using UI.Helpers;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public sealed partial class TaskControlPage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private TaskWithNotification<string> normalTask;

        public TaskWithNotification<string> NormalTask
        {
            get
            {
                return normalTask;
            }

            set
            {
                normalTask = value;
                RaiseProperty(nameof(NormalTask));
            }
        }

        private TaskWithNotification<BitmapImage> imageTask;

        public TaskWithNotification<BitmapImage> ImageTask
        {
            get
            {
                return imageTask;
            }

            set
            {
                imageTask = value;
                RaiseProperty(nameof(ImageTask));
            }
        }

        private TaskWithNotification<SamplePerson> complexTask;

        public TaskWithNotification<SamplePerson> ComplexTask
        {
            get
            {
                return complexTask;
            }

            set
            {
                complexTask = value;
                RaiseProperty(nameof(ComplexTask));
            }
        }

        public TaskControlPage()
        {
            InitializeComponent();
        }

        private CancellationTokenSource normalTaskCTS = new CancellationTokenSource();
        private CancellationTokenSource imageTaskCTS = new CancellationTokenSource();
        private CancellationTokenSource complexTaskCTS = new CancellationTokenSource();

        private async Task<string> NormalJob(CancellationToken token)
        {
            for (int i = 0; i < 6; i++)
            {
                await Task.Delay(1000);
                token.ThrowIfCancellationRequested();
            }

            return "JOB DONE";
        }

        private async Task<string> NormalJobException(CancellationToken token)
        {
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                token.ThrowIfCancellationRequested();
            }

            throw new Exception();
        }

        private void Button_NormalStart(object sender, RoutedEventArgs e)
        {
            normalTaskCTS = new CancellationTokenSource();
            NormalTask = new TaskWithNotification<string>(NormalJob(normalTaskCTS.Token));
        }

        private void Button_NormalStartException(object sender, RoutedEventArgs e)
        {
            normalTaskCTS = new CancellationTokenSource();
            NormalTask = new TaskWithNotification<string>(NormalJobException(normalTaskCTS.Token));
        }

        private async Task<BitmapImage> ImageJob(CancellationToken token)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream memoryStream = new MemoryStream();
            await Task.Run(
                async () =>
                {
                    Stream tempStream = await new HttpClient().GetStreamAsync(@"https://apod.nasa.gov/apod/image/1605/quivertrees_breuer_1080.jpg");
                    await tempStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                }, token);
            await image.SetSourceAsync(memoryStream.AsRandomAccessStream());
            return image;
        }

        private async Task<BitmapImage> ImageJobException(CancellationToken token)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream memoryStream = new MemoryStream();
            await Task.Run(
                async () =>
                {
                    Stream tempStream = await new HttpClient().GetStreamAsync(@"https://apod.nasa.gov/apod/image/1605/quivertrees_breuer_1080.jpg");
                    await tempStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    throw new Exception();
                }, token);
            return image;
        }

        private void Button_ImageStart(object sender, RoutedEventArgs e)
        {
            imageTaskCTS = new CancellationTokenSource();
            ImageTask = new TaskWithNotification<BitmapImage>(ImageJob(imageTaskCTS.Token));
        }

        private void Button_ImageStartException(object sender, RoutedEventArgs e)
        {
            imageTaskCTS = new CancellationTokenSource();
            ImageTask = new TaskWithNotification<BitmapImage>(ImageJobException(imageTaskCTS.Token));
        }

        private void Button_ComplexStart(object sender, RoutedEventArgs e)
        {
            complexTaskCTS = new CancellationTokenSource();
            Func<Task<SamplePerson>> myFunc = async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(500);
                    complexTaskCTS.Token.ThrowIfCancellationRequested();
                }

                return new SamplePerson { Name = "John", Profession = "Programmer" };
            };
            ComplexTask = new TaskWithNotification<SamplePerson>(myFunc.Invoke());
        }

        private void Button_ComplexStartException(object sender, RoutedEventArgs e)
        {
            complexTaskCTS = new CancellationTokenSource();
            Func<Task<SamplePerson>> myFunc = async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(500);
                    complexTaskCTS.Token.ThrowIfCancellationRequested();
                }

                throw new Exception();
            };
            ComplexTask = new TaskWithNotification<SamplePerson>(myFunc.Invoke());
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            normalTaskCTS.Cancel();
            imageTaskCTS.Cancel();
            complexTaskCTS.Cancel();
        }
    }
}
