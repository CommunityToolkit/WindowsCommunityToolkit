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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Controls;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Monaco;
using Monaco.Editor;
using Monaco.Helpers;
using Windows.System;
using Windows.System.Profile;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    /// <summary>
    /// A wrapper for the Sample Page.
    /// </summary>
    public sealed partial class SampleController : Page, INotifyPropertyChanged
    {
        public static SampleController Current { get; private set; }

        public Sample CurrentSample { get; private set; }

        public ObservableCollection<SampleCommand> Commands { get; } = new ObservableCollection<SampleCommand>();

        public bool DisplayWaitRing
        {
            set { waitRing.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public PaneState SidePaneState
        {
            get => _paneState;
            set
            {
                _paneState = value;
                UpdateProperty();
            }
        }

        private Page SamplePage => SampleContent.Content as Page;

        private XamlRenderService _xamlRenderer = new XamlRenderService();
        private bool _lastRenderedProperties = true;
        private ThreadPoolTimer _autocompileTimer;

        private DateTime _timeSampleEditedFirst = DateTime.MinValue;
        private DateTime _timeSampleEditedLast = DateTime.MinValue;
        private bool _xamlCodeRendererSupported = false;

        private PaneState _paneState;
        private bool _hasDocumentation = true;
        private bool _onlyDocumentation;

        private bool CanChangePaneState => _hasDocumentation && !_onlyDocumentation;

        public SampleController()
        {
            this.InitializeComponent();
            Current = this;

            ProcessSampleEditorTime();
        }

        public void OpenClosePane()
        {
            if (CanChangePaneState)
            {
                if (SidePaneState == PaneState.Closed)
                {
                    SidePaneState = PaneState.Normal;
                }
                else
                {
                    SidePaneState = PaneState.Closed;
                }
            }
        }

        public void ExpandCollapsePane()
        {
            if (CanChangePaneState)
            {
                if (SidePaneState == PaneState.Full)
                {
                    SidePaneState = PaneState.Normal;
                }
                else
                {
                    SidePaneState = PaneState.Full;
                }
            }
        }

        public void RegisterNewCommand(string name, RoutedEventHandler action)
        {
            Commands.Add(new SampleCommand(name, () =>
            {
                try
                {
                    action.Invoke(this, new RoutedEventArgs());
                }
                catch (Exception ex)
                {
                    ExceptionNotification.Show(ex.Message, 3000);
                }
            }));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Sample sample)
            {
                CurrentSample = sample;
            }

            if (CurrentSample != null)
            {
                if (!string.IsNullOrWhiteSpace(CurrentSample.Type))
                {
                    try
                    {
                        var pageInstance = Activator.CreateInstance(CurrentSample.PageType);
                        SampleContent.Content = pageInstance;
                    }
                    catch
                    {
                        ExceptionNotification.Show("Sample Page failed to load.");
                    }

                    if (SamplePage != null)
                    {
                        SamplePage.Loaded += SamplePage_Loaded;
                    }
                }
                else
                {
                    _onlyDocumentation = true;
                }

                DataContext = CurrentSample;

                await Samples.PushRecentSample(CurrentSample);

                var propertyDesc = CurrentSample.PropertyDescriptor;

                InfoAreaPivot.Items.Clear();

                if (propertyDesc != null)
                {
                    _xamlRenderer.DataContext = propertyDesc.Expando;
                }

                if (CurrentSample.HasDocumentation)
                {
                    var docs = await CurrentSample.GetDocumentationAsync();
                    if (!string.IsNullOrWhiteSpace(docs))
                    {
                        DocumentationTextblock.Text = docs;
                        InfoAreaPivot.Items.Add(DocumentationPivotItem);
                    }
                }

                if (propertyDesc != null && propertyDesc.Options.Count > 0)
                {
                    InfoAreaPivot.Items.Add(PropertiesPivotItem);
                }

                if (CurrentSample.HasXAMLCode)
                {
                    if (AnalyticsInfo.VersionInfo.GetDeviceFormFactor() != DeviceFormFactor.Desktop || CurrentSample.DisableXamlEditorRendering)
                    {
                        // Only makes sense (and works) for now to show Live Xaml on Desktop, so fallback to old system here otherwise.
                        XamlReadOnlyCodeRenderer.XamlSource = CurrentSample.UpdatedXamlCode;

                        InfoAreaPivot.Items.Add(XamlReadOnlyPivotItem);
                    }
                    else
                    {
                        XamlCodeRenderer.Text = CurrentSample.UpdatedXamlCode;

                        InfoAreaPivot.Items.Add(XamlPivotItem);

                        _xamlCodeRendererSupported = true;
                    }

                    InfoAreaPivot.SelectedIndex = 0;
                }

                if (CurrentSample.HasCSharpCode)
                {
                    CSharpCodeRenderer.CSharpSource = await CurrentSample.GetCSharpSourceAsync();
                    InfoAreaPivot.Items.Add(CSharpPivotItem);
                }

                if (CurrentSample.HasJavaScriptCode)
                {
                    JavaScriptCodeRenderer.CSharpSource = await CurrentSample.GetJavaScriptSourceAsync();
                    InfoAreaPivot.Items.Add(JavaScriptPivotItem);
                }

                if (!string.IsNullOrEmpty(CurrentSample.CodeUrl))
                {
                    //GitHub.NavigateUri = new Uri(CurrentSample.CodeUrl);
                    //GitHub.Visibility = Visibility.Visible;
                }
                else
                {
                    //GitHub.Visibility = Visibility.Collapsed;
                }

                if (InfoAreaPivot.Items.Count == 0)
                {
                    SidePaneState = PaneState.None;
                    _hasDocumentation = false;
                }
                else
                {
                    SidePaneState = _onlyDocumentation ? PaneState.Full : PaneState.Normal;
                }

                Shell.Current.SetTitles($"{CurrentSample.CategoryName} > {CurrentSample.Name}");
            }
            else
            {
                ExceptionNotification.Show("Sample does not exist");
            }

            if (!CanChangePaneState)
            {
                SampleTitleBar.Children.Remove(NarrowInfoButton);
                WindowStates.States.Clear();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (SamplePage is ISampleNavigation nav)
            {
                nav.NavigatingAway();
            }

            XamlCodeRenderer = null;

            // Not great, but need to collect up after WebView. (Does this work?)
            GC.Collect();
        }

        private void SamplePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentSample != null && CurrentSample.HasXAMLCode)
            {
                this._lastRenderedProperties = true;

                // Called to load the sample initially as we don't get an Item Pivot Selection Changed with Sample Loaded yet.
                var t = UpdateXamlRenderAsync(CurrentSample.BindedXamlCode);
            }
        }

        private async void InfoAreaPivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InfoAreaPivot.SelectedItem != null)
            {
                if (DataContext is Sample sample)
                {
                    TrackingManager.TrackEvent("PropertyGrid", (InfoAreaPivot.SelectedItem as FrameworkElement)?.Name, sample.Name);
                }
            }

            if (CurrentSample == null)
            {
                return;
            }

            if (InfoAreaPivot.SelectedItem == PropertiesPivotItem)
            {
                // If we switch to the Properties Panel, we want to use a binded version of the Xaml Code.
                if (CurrentSample.HasXAMLCode)
                {
                    _lastRenderedProperties = true;

                    var t = UpdateXamlRenderAsync(CurrentSample.BindedXamlCode);
                }

                return;
            }

            if (CurrentSample.HasXAMLCode && InfoAreaPivot.SelectedItem == XamlPivotItem && _lastRenderedProperties)
            {
                // Use this flag so we don't re-render the XAML tab if we're switching from tabs other than the properties one.
                _lastRenderedProperties = false;

                // If we switch to the Live Preview, then we want to use the Value based Text
                XamlCodeRenderer.Text = CurrentSample.UpdatedXamlCode;

                var t = UpdateXamlRenderAsync(CurrentSample.UpdatedXamlCode);
                await XamlCodeRenderer.RevealPositionAsync(new Position(1, 1));

                XamlCodeRenderer.Focus(FocusState.Programmatic);
                return;
            }

            if (CurrentSample.HasXAMLCode && InfoAreaPivot.SelectedItem == XamlReadOnlyPivotItem)
            {
                // Update Read-Only XAML tab on non-desktop devices to show changes to Properties
                XamlReadOnlyCodeRenderer.XamlSource = CurrentSample.UpdatedXamlCode;
            }

            if (CurrentSample.HasCSharpCode && InfoAreaPivot.SelectedItem == CSharpPivotItem)
            {
                CSharpCodeRenderer.CSharpSource = await CurrentSample.GetCSharpSourceAsync();

                return;
            }

            if (CurrentSample.HasJavaScriptCode && InfoAreaPivot.SelectedItem == JavaScriptPivotItem)
            {
                JavaScriptCodeRenderer.JavaScriptSource = await CurrentSample.GetJavaScriptSourceAsync();

                return;
            }
        }

        private async void DocumentationTextblock_OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", e.Link);
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private void DocumentationTextblock_ImageResolving(object sender, ImageResolvingEventArgs e)
        {
            e.Image = new BitmapImage(new Uri("ms-appx:///Assets/pixel.png"));
            e.Handled = true;
        }

        private void GitHub_OnClick(object sender, RoutedEventArgs e)
        {
            //TrackingManager.TrackEvent("Link", GitHub.NavigateUri.ToString());
        }

        public async Task RefreshXamlRenderAsync()
        {
            if (CurrentSample != null)
            {
                var code = string.Empty;
                if (InfoAreaPivot.SelectedItem == PropertiesPivotItem)
                {
                    code = CurrentSample.BindedXamlCode;
                }
                else
                {
                    code = CurrentSample.UpdatedXamlCode;
                }

                if (!string.IsNullOrWhiteSpace(code))
                {
                    await UpdateXamlRenderAsync(code);
                }
            }
        }

        private async Task UpdateXamlRenderAsync(string text)
        {
            // Hide any Previous Errors
            XamlCodeRenderer.Decorations.Clear();
            XamlCodeRenderer.Options.GlyphMargin = false;

            // Try and Render Xaml to a UIElement
            UIElement element = null;
            try
            {
                element = _xamlRenderer.Render(text);
            }
            catch (Exception ex)
            {
                ExceptionNotification.Show(ex.Message, 3000);
            }

            if (element != null)
            {
                // Add element to main panel
                if (SamplePage == null)
                {
                    return;
                }

                var root = SamplePage.FindDescendantByName("XamlRoot");

                if (root is Panel)
                {
                    // If we've defined a 'XamlRoot' element to host us as a panel, use that.
                    (root as Panel).Children.Clear();
                    (root as Panel).Children.Add(element);
                }
                else
                {
                    // Otherwise, just replace the entire page's content
                    SamplePage.Content = element;
                }

                // Tell the page we've finished with an update to the XAML contents, after the control has rendered.
                await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    (SamplePage as IXamlRenderListener)?.OnXamlRendered(element as FrameworkElement);
                });
            }
            else if (_xamlRenderer.Errors.Count > 0)
            {
                var error = _xamlRenderer.Errors.First();

                XamlCodeRenderer.Options.GlyphMargin = true;

                var range = new Range(error.StartLine, 1, error.EndLine, await XamlCodeRenderer.GetModel().GetLineMaxColumnAsync(error.EndLine));

                // Highlight Error Line
                XamlCodeRenderer.Decorations.Add(new IModelDeltaDecoration(
                    range,
                    new IModelDecorationOptions() { IsWholeLine = true, ClassName = _errorStyle, HoverMessage = new string[] { error.Message } }));

                // Show Glyph Icon
                XamlCodeRenderer.Decorations.Add(new IModelDeltaDecoration(
                    range,
                    new IModelDecorationOptions() { IsWholeLine = true, GlyphMarginClassName = _errorIconStyle, GlyphMarginHoverMessage = new string[] { error.Message } }));
            }
        }

        private Visibility GreaterThanZero(int value)
        {
            return value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateProperty([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CssLineStyle _errorStyle = new CssLineStyle()
        {
            BackgroundColor = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xD6, 0xD6))
        };

        private CssGlyphStyle _errorIconStyle = new CssGlyphStyle()
        {
            GlyphImage = new Uri("ms-appx-web:///Icons/Error.png")
        };

        private static readonly int[] NonCharacterCodes = new int[]
        {
            // Modifier Keys
            16, 17, 18, 20, 91,

            // Esc / Page Keys / Home / End / Insert
            27, 33, 34, 35, 36, 45,

            // Arrow Keys
            37, 38, 39, 40,

            // Function Keys
            112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123
        };

        public event PropertyChangedEventHandler PropertyChanged;

        private void XamlCodeRenderer_KeyDown(Monaco.CodeEditor sender, Monaco.Helpers.WebKeyEventArgs args)
        {
            // Handle Shortcuts.
            // Ctrl+Enter or F5 Update // TODO: Do we need this in the app handler too? (Thinking no)
            if ((args.KeyCode == 13 && args.CtrlKey) ||
                 args.KeyCode == 116)
            {
                var t = UpdateXamlRenderAsync(XamlCodeRenderer.Text);

                // Eat key stroke
                args.Handled = true;
            }

            // Ignore as a change to the document if we handle it as a shortcut above or it's a special char.
            if (!args.Handled && Array.IndexOf(NonCharacterCodes, args.KeyCode) == -1)
            {
                // TODO: Mark Dirty here if we want to prevent overwrites.

                // Setup Time for Auto-Compile
                this._autocompileTimer?.Cancel(); // Stop Old Timer

                // Create Compile Timer
                this._autocompileTimer = ThreadPoolTimer.CreateTimer(
                    async (e) =>
                    {
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                        {
                            var t = UpdateXamlRenderAsync(XamlCodeRenderer.Text);

                            if (_timeSampleEditedFirst == DateTime.MinValue)
                            {
                                _timeSampleEditedFirst = DateTime.Now;
                            }

                            _timeSampleEditedLast = DateTime.Now;
                        });
                    }, TimeSpan.FromSeconds(0.5));
            }
        }

        private void XamlCodeRenderer_Loading(object sender, RoutedEventArgs e)
        {
            XamlCodeRenderer.Options.Folding = true;
        }

        private void XamlCodeRenderer_InternalException(CodeEditor sender, Exception args)
        {
            TrackingManager.TrackException(args);

            // If you hit an issue here, please report repro steps along with all the info from the Exception object.
#if DEBUG
            Debugger.Break();
#endif
        }

        private void ProcessSampleEditorTime()
        {
            if (CurrentSample != null &&
                CurrentSample.HasXAMLCode &&
                _xamlCodeRendererSupported)
            {
                if (_timeSampleEditedFirst != DateTime.MinValue &&
                    _timeSampleEditedLast != DateTime.MinValue)
                {
                    int secondsEdditingSample = (int)Math.Floor((_timeSampleEditedLast - _timeSampleEditedFirst).TotalSeconds);
                    TrackingManager.TrackEvent("xamleditor", "edited", CurrentSample.Name, secondsEdditingSample);
                }
                else
                {
                    TrackingManager.TrackEvent("xamleditor", "not_edited", CurrentSample.Name);
                }
            }

            _timeSampleEditedFirst = _timeSampleEditedLast = DateTime.MinValue;
        }

        private void WindowStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            switch (e.NewState.Name)
            {
                case nameof(NarrowState):
                    if (CanChangePaneState)
                    {
                        SidePaneState = PaneState.Closed;
                    }

                    // Update Read-Only XAML tab when switching back to show changes to TwoWay Bound Properties
                    if (CurrentSample?.HasXAMLCode == true && InfoAreaPivot.SelectedItem == XamlReadOnlyPivotItem)
                    {
                        XamlReadOnlyCodeRenderer.XamlSource = CurrentSample.UpdatedXamlCode;
                    }

                    break;

                case nameof(WideState):
                    if (CanChangePaneState)
                    {
                        SidePaneState = PaneState.Normal;
                    }

                    break;
            }
        }

        private void PaneStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.OldState?.Name == nameof(Full) && WindowStates.CurrentState?.Name == nameof(NarrowState))
            {
                // Restart the State, full state changed things.
                VisualStateManager.GoToState(this, NarrowState.Name, false);
            }
        }
    }
}