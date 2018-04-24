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

using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GazeInteractionPage : IXamlRenderListener
    {
        private GazeElement gazeButtonControl;

        private Rectangle thresholdRec;
        private Rectangle fixationRec;
        private Rectangle dwellRec;
        private Rectangle repeatRec;
        private Rectangle exitRec;
        private TextBlock dwellCountText;

        private int dwellCount = 0;

        public GazeInteractionPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            dwellCountText = control.FindChildByName("DwellCountText") as TextBlock;

            thresholdRec = control.FindChildByName("EnterRec") as Rectangle;
            fixationRec = control.FindChildByName("FixationRec") as Rectangle;
            dwellRec = control.FindChildByName("DwellRec") as Rectangle;
            repeatRec = control.FindChildByName("RepeatRec") as Rectangle;
            exitRec = control.FindChildByName("ExitRec") as Rectangle;

            var buttonControl = control.FindChildByName("TargetButton") as Button;

            if (buttonControl != null)
            {
                gazeButtonControl = GazeInput.GetGazeElement(buttonControl);

                if (gazeButtonControl == null)
                {
                    gazeButtonControl = new GazeElement();
                    GazeInput.SetGazeElement(buttonControl, gazeButtonControl);
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

        private void GazeButtonControl_StateChanged(object sender, StateChangedEventArgs ea)
        {
            if (ea.PointerState == PointerState.Enter)
            {
                thresholdRec.Visibility = Visibility.Visible;
                dwellCountText.Visibility = Visibility.Collapsed;
                dwellCountText.Text = string.Empty;
                dwellCount = 0;
                exitRec.Visibility = Visibility.Collapsed;
            }

            if (ea.PointerState == PointerState.Fixation)
            {
                fixationRec.Visibility = Visibility.Visible;
            }

            if (ea.PointerState == PointerState.Dwell)
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

            if (ea.PointerState == PointerState.Exit)
            {
                exitRec.Visibility = Visibility.Visible;

                thresholdRec.Visibility = Visibility.Collapsed;
                fixationRec.Visibility = Visibility.Collapsed;
                dwellRec.Visibility = Visibility.Collapsed;
                repeatRec.Visibility = Visibility.Collapsed;
            }
        }
    }
}
