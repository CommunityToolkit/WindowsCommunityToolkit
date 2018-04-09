using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input.Preview;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GazeTracingPage : Page
    {
        private GazeInputSourcePreview gazeInputSourcePreview;

        public ObservableCollection<Point> GazeHistory { get; set; }

        public int TracePointDiameter { get; set; }
        public int MaxGazeHistorySize { get; set; }

        public bool ShowIntermediatePoints { get; set; }

        public GazeTracingPage()
        {
            this.InitializeComponent();

            ShowIntermediatePoints = false;
            MaxGazeHistorySize = 100;
            GazeHistory = new ObservableCollection<Point>();

            gazeInputSourcePreview = GazeInputSourcePreview.GetForCurrentView();
            gazeInputSourcePreview.GazeMoved += GazeInputSourcePreview_GazeMoved;

        }

        private void UpdateGazeHistory(GazePointPreview pt)
        {
            if (!pt.EyeGazePosition.HasValue)
            {
                return;
            }

            GazeHistory.Add(pt.EyeGazePosition.Value);
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
    }
}
