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

using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Triggers {

    /// <summary>
    /// Detects whether the device is in tablet or in desktop mode.
    /// </summary>
    /// <remarks>
    /// The approach is described in the TechNet wiki at
    /// http://social.technet.microsoft.com/wiki/contents/articles/31069.windows-10-apps-leverage-continuum-feature-to-change-ui-for-mousekeyboard-users-using-custom-statetrigger.aspx
    /// </remarks>
    public sealed class UserInteractionModeTrigger : StateTriggerBase {

        /// <summary>
        /// Identifies the <see cref="Mode"/> property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty
            = DependencyProperty.Register("Mode",
                typeof(UserInteractionMode),
                typeof(UserInteractionModeTrigger),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public UserInteractionModeTrigger() {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled) {
                var win = Window.Current;

                WindowActivatedEventHandler h = null;
                h = (s, e) => {
                    win.Activated -= h;
                    this.UpdateMode();
                };
                win.Activated += h;

                win.SizeChanged += this.OnWindowSizeChanged;
            }
        }

        /// <summary>
        /// Gets or sets the current user interaction mode to be triggered.
        /// </summary>
        public UserInteractionMode Mode {
            get {
                return (UserInteractionMode) this.GetValue(ModeProperty);
            }
            set {
                this.SetValue(ModeProperty, value);
            }
        }

        /// <summary>
        /// According to the TechNet article, the window resize event is fired
        /// if the interaction mode is changed, so we use this event to
        /// re-evaluate the trigger's state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowSizeChanged(object sender,
                WindowSizeChangedEventArgs e) {
            this.UpdateMode();
        }

        /// <summary>
        /// Evaluates the current interaction mode.
        /// </summary>
        private void UpdateMode() {
            var settings = UIViewSettings.GetForCurrentView();
            var curMode = settings.UserInteractionMode;
            this.SetActive(curMode == this.Mode);
        }
    }
}
