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

        private int dwellCount = 0;

        public GazeInteractionPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            GazeInput.IsDeviceAvailableChanged += GazeInput_IsDeviceAvailableChanged;

            WarnUserToPlugInDevice();

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
                    gazeButtonControl.DwellProgressFeedback += OnProgressFeedback;
                    gazeButtonControl.Invoked += OnGazeInvoked;
                    gazeButtonControl.StateChanged += GazeButtonControl_StateChanged;
                }
            }
        }

        private void GazeInput_IsDeviceAvailableChanged(object sender, object e)
        {
            WarnUserToPlugInDevice();
        }

        private void WarnUserToPlugInDevice()
        {
            if (GazeInput.IsDeviceAvailable)
            {
                WarningText.Visibility = Visibility.Collapsed;
            }
            else
            {
                WarningText.Visibility = Visibility.Visible;
            }
        }

        private void OnGazeInvoked(object sender, DwellInvokedRoutedEventArgs e)
        {
        }

        private void OnProgressFeedback(object sender, DwellProgressEventArgs e)
        {
            DwellProgressBar.Maximum = 1.0;
            DwellProgressBar.Value = e.Progress;
            if (e.State == DwellProgressState.Complete)
            {
                DwellProgressBar.Value = 0;
            }
        }

        private void GazeButtonControl_StateChanged(object sender, StateChangedEventArgs ea)
        {
            if (ea.PointerState == PointerState.Enter)
            {
                EnterRec.Visibility = Visibility.Visible;
                DwellCountText.Visibility = Visibility.Collapsed;
                DwellCountText.Text = string.Empty;
                dwellCount = 0;
                ExitRec.Visibility = Visibility.Collapsed;
            }

            if (ea.PointerState == PointerState.Fixation)
            {
                FixationRec.Visibility = Visibility.Visible;
            }

            if (ea.PointerState == PointerState.Dwell)
            {
                if (dwellCount == 0)
                {
                    DwellRec.Visibility = Visibility.Visible;
                    dwellCount = 1;
                }
                else
                {
                    RepeatRec.Visibility = Visibility.Visible;
                    DwellCountText.Text = dwellCount.ToString();
                    DwellCountText.Visibility = Visibility.Visible;
                    dwellCount += 1;
                }
            }

            if (ea.PointerState == PointerState.Exit)
            {
                ExitRec.Visibility = Visibility.Visible;

                EnterRec.Visibility = Visibility.Collapsed;
                FixationRec.Visibility = Visibility.Collapsed;
                DwellRec.Visibility = Visibility.Collapsed;
                RepeatRec.Visibility = Visibility.Collapsed;
                DwellProgressBar.Value = 0;
            }
        }
    }
}
