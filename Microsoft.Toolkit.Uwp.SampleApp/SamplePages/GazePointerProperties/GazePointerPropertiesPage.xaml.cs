using Microsoft.Toolkit.Uwp.Input.Gaze;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GazePointerPropertiesPage : IXamlRenderListener
    {
        private GazeElement gazeButtonControl;
        
        private Rectangle enterRec;
        private Rectangle fixationRec;
        private Rectangle dwellRec;
        private Rectangle repeatRec;
        private Rectangle exitRec;
        private TextBlock dwellCountText;

        private int dwellCount = 0;

        public GazePointerPropertiesPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            dwellCountText = control.FindChildByName("DwellCountText") as TextBlock;

            enterRec = control.FindChildByName("EnterRec") as Rectangle;
            fixationRec = control.FindChildByName("FixationRec") as Rectangle;
            dwellRec = control.FindChildByName("DwellRec") as Rectangle;
            repeatRec = control.FindChildByName("RepeatRec") as Rectangle;
            exitRec = control.FindChildByName("ExitRec") as Rectangle;

            var buttonControl = control.FindChildByName("TargetButton") as Button;

            if (buttonControl != null)
            {
                gazeButtonControl = GazeApi.GetGazeElement(buttonControl);

                if (gazeButtonControl == null)
                {
                    gazeButtonControl = new GazeElement();
                    GazeApi.SetGazeElement(buttonControl, gazeButtonControl);
                }

                if (gazeButtonControl != null)
                {
                    gazeButtonControl.Invoked += OnGazeInvoked;
                    gazeButtonControl.StateChanged += GazeButtonControl_StateChanged;
                }
            }
        }

        private void OnGazeInvoked(object sender, GazeInvokedRoutedEventArgs e)
        {


        }

        private void GazeButtonControl_StateChanged(object sender, GazePointerEventArgs ea)
        {

            if (ea.PointerState == GazePointerState.Enter)
            {
                enterRec.Visibility = Visibility.Visible;
                dwellCountText.Visibility = Visibility.Collapsed;
                dwellCountText.Text = "";
                dwellCount = 0;
                exitRec.Visibility = Visibility.Collapsed;
            }
            if (ea.PointerState == GazePointerState.Fixation)
            {
                fixationRec.Visibility = Visibility.Visible;
            }
            if (ea.PointerState == GazePointerState.Dwell)
            {
                if (dwellCount == 0)
                {
                    dwellRec.Visibility = Visibility.Visible;
                    dwellCount = 1;
                }
                else
                {
                    repeatRec.Visibility = Visibility.Visible;                    
                    dwellCountText.Text = dwellCount.ToString();
                    dwellCountText.Visibility = Visibility.Visible;
                    dwellCount += 1;
                }
            }
            if (ea.PointerState == GazePointerState.Exit)
            {
                exitRec.Visibility = Visibility.Visible;

                enterRec.Visibility = Visibility.Collapsed;
                fixationRec.Visibility = Visibility.Collapsed;
                dwellRec.Visibility = Visibility.Collapsed;
                repeatRec.Visibility = Visibility.Collapsed;
            }

        }

    }
}
