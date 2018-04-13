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

using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.ObjectModel;
using Windows.Devices.Input.Preview;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GazeTracingPage : IXamlRenderListener
    {
        private GazeInputSourcePreview gazeInputSourcePreview;
        private Frame rootFrame;

        public ObservableCollection<Point> GazeHistory { get; set; } = new ObservableCollection<Point>();

        public int TracePointDiameter { get; set; }

        public int MaxGazeHistorySize { get; set; }

        public bool ShowIntermediatePoints { get; set; }

        public GazeTracingPage()
        {
            this.InitializeComponent();
            DataContext = this;

            ShowIntermediatePoints = false;
            MaxGazeHistorySize = 100;

            rootFrame = Window.Current.Content as Frame;
            gazeInputSourcePreview = GazeInputSourcePreview.GetForCurrentView();
            gazeInputSourcePreview.GazeMoved += GazeInputSourcePreview_GazeMoved;

            var random = new Random();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, e) => GazeHistory.Add(new Point(random.Next(0, 300), random.Next(0, 300)));
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void UpdateGazeHistory(GazePointPreview pt)
        {
            if (!pt.EyeGazePosition.HasValue)
            {
                return;
            }

            var transform = rootFrame.TransformToVisual(this);
            var point = transform.TransformPoint(pt.EyeGazePosition.Value);
            GazeHistory.Add(point);
            if (GazeHistory.Count > MaxGazeHistorySize)
            {
                GazeHistory.RemoveAt(0);
            }
        }

        private void GazeInputSourcePreview_GazeMoved(GazeInputSourcePreview sender, GazeMovedPreviewEventArgs args)
        {
            if (!ShowIntermediatePoints)
            {
                UpdateGazeHistory(args.CurrentPoint);
                return;
            }

            var points = args.GetIntermediatePoints();
            foreach (var pt in points)
            {
                UpdateGazeHistory(pt);
            }
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var itemsControl = control.FindChildByName("Points") as ItemsControl;
            if (itemsControl != null)
            {
                itemsControl.ItemsSource = GazeHistory;
            }
        }
    }
}
