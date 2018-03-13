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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Controls;
using Microsoft.Toolkit.Uwp.SampleApp.Pages;
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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class Shell
    {
        public static Shell Current { get; private set; }

        private bool _isPaneOpen;

        private XamlRenderService _xamlRenderer = new XamlRenderService();
        private bool _lastRenderedProperties = true;
        private ThreadPoolTimer _autocompileTimer;

        private DateTime _timeSampleEditedFirst = DateTime.MinValue;
        private DateTime _timeSampleEditedLast = DateTime.MinValue;
        private bool _xamlCodeRendererSupported = false;
        private string documentationPath;

        public bool DisplayWaitRing
        {
            set { waitRing.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public ObservableCollection<SampleCommand> Commands { get; } = new ObservableCollection<SampleCommand>();

        public Shell()
        {
            InitializeComponent();

            Current = this;
            DocumentationTextblock.SetRenderer<SampleAppMarkdownRenderer>();
        }

        public void ShowInfoArea()
        {
            InfoAreaGrid.Visibility = Visibility.Visible;
            RootGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
            RootGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            RootGrid.RowDefinitions[2].Height = new GridLength(32);
            Splitter.Visibility = Visibility.Visible;
        }

        public void ShowOnlyHeader(string title)
        {
            Title.Text = title;
            HideInfoArea();
        }

        /// <summary>
        /// Navigates to a Sample via a deep link.
        /// </summary>
        /// <param name="deepLink">The deep link. Specified as protocol://[collectionName]?sample=[sampleName]</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task NavigateToSampleAsync(string deepLink)
        {
            var parser = DeepLinkParser.Create(deepLink);
            var targetSample = await Samples.GetSampleByName(parser["sample"]);
            if (targetSample != null)
            {
                NavigateToSample(targetSample);
            }
        }

        public void NavigateToSample(Sample sample)
        {
            var pageType = Type.GetType("Microsoft.Toolkit.Uwp.SampleApp.SamplePages." + sample.Type);

            if (pageType != null)
            {
                InfoAreaPivot.Items.Clear();
                NavigationFrame.Navigate(pageType, sample.Name);
            }
        }

        public void RegisterNewCommand(string name, RoutedEventHandler action)
        {
            Commands.Add(new SampleCommand(name, (parameter) =>
            {
                try
                {
                    action.Invoke(this, (parameter as RoutedEventArgs) ?? new RoutedEventArgs());
                }
                catch (Exception ex)
                {
                    ExceptionNotification.Show(ex.Message, 3000);
                }
            }));
        }

        public Task StartSearch(string startingText = "")
        {
            return HamburgerMenu.StartSearch(startingText);
        }

        public async Task RefreshXamlRenderAsync()
        {
            if (HamburgerMenu.CurrentSample != null)
            {
                var code = string.Empty;
                if (InfoAreaPivot.SelectedItem == PropertiesPivotItem)
                {
                    code = HamburgerMenu.CurrentSample.BindedXamlCode;
                }
                else
                {
                    code = HamburgerMenu.CurrentSample.UpdatedXamlCode;
                }

                if (!string.IsNullOrWhiteSpace(code))
                {
                    await UpdateXamlRenderAsync(code);
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationFrame.Navigating += NavigationFrame_Navigating;
            NavigationFrame.Navigated += NavigationFrameOnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            // Get list of samples
            var sampleCategories = (await Samples.GetCategoriesAsync()).ToList();

            HamburgerMenu.ItemsSource = sampleCategories;

            // Options
            HamburgerMenu.OptionsItemsSource = new[]
            {
                new Option { Glyph = "\xE10F", Name = "About", PageType = typeof(About) }
            };

            HideInfoArea();
            NavigationFrame.Navigate(typeof(About));

            if (!string.IsNullOrWhiteSpace(e?.Parameter?.ToString()))
            {
                var parser = DeepLinkParser.Create(e.Parameter.ToString());
                var targetSample = await Sample.FindAsync(parser.Root, parser["sample"]);
                if (targetSample != null)
                {
                    NavigateToSample(targetSample);
                }
            }
        }

        private async void NavigationFrame_Navigating(object sender, NavigatingCancelEventArgs navigationEventArgs)
        {
            ProcessSampleEditorTime();

            SampleCategory category;
            if (navigationEventArgs.Parameter == null)
            {
                DataContext = null;
                HamburgerMenu.CurrentSample = null;
                category = navigationEventArgs.Parameter as SampleCategory;

                if (category != null)
                {
                    TrackingManager.TrackPage($"{navigationEventArgs.SourcePageType.Name} - {category.Name}");
                }
                else
                {
                    TrackingManager.TrackPage($"{navigationEventArgs.SourcePageType.Name}");
                }

                HideInfoArea();
            }
            else
            {
                TrackingManager.TrackPage(navigationEventArgs.SourcePageType.Name);
                Commands.Clear();
                ShowInfoArea();

                var sampleName = navigationEventArgs.Parameter.ToString();
                HamburgerMenu.CurrentSample = await Samples.GetSampleByName(sampleName);
                DataContext = HamburgerMenu.CurrentSample;

                if (HamburgerMenu.CurrentSample == null)
                {
                    HideInfoArea();
                    return;
                }

                category = await Samples.GetCategoryBySample(HamburgerMenu.CurrentSample);
                await Samples.PushRecentSample(HamburgerMenu.CurrentSample);

                var propertyDesc = HamburgerMenu.CurrentSample.PropertyDescriptor;

                InfoAreaPivot.Items.Clear();

                if (propertyDesc != null)
                {
                    _xamlRenderer.DataContext = propertyDesc.Expando;
                }

                Title.Text = HamburgerMenu.CurrentSample.Name;

                if (propertyDesc != null && propertyDesc.Options.Count > 0)
                {
                    InfoAreaPivot.Items.Add(PropertiesPivotItem);
                }

                if (HamburgerMenu.CurrentSample.HasXAMLCode)
                {
                    if (AnalyticsInfo.VersionInfo.GetDeviceFormFactor() != DeviceFormFactor.Desktop || HamburgerMenu.CurrentSample.DisableXamlEditorRendering)
                    {
                        // Only makes sense (and works) for now to show Live Xaml on Desktop, so fallback to old system here otherwise.
                        XamlReadOnlyCodeRenderer.SetCode(HamburgerMenu.CurrentSample.UpdatedXamlCode, "xaml");

                        InfoAreaPivot.Items.Add(XamlReadOnlyPivotItem);
                    }
                    else
                    {
                        XamlCodeRenderer.Text = HamburgerMenu.CurrentSample.UpdatedXamlCode;

                        InfoAreaPivot.Items.Add(XamlPivotItem);

                        _xamlCodeRendererSupported = true;
                    }

                    InfoAreaPivot.SelectedIndex = 0;
                }

                if (HamburgerMenu.CurrentSample.HasCSharpCode)
                {
                    var code = await HamburgerMenu.CurrentSample.GetCSharpSourceAsync();

                    CSharpCodeRenderer.SetCode(code, "c#");
                    InfoAreaPivot.Items.Add(CSharpPivotItem);
                }

                if (HamburgerMenu.CurrentSample.HasJavaScriptCode)
                {
                    var code = await HamburgerMenu.CurrentSample.GetJavaScriptSourceAsync();

                    JavaScriptCodeRenderer.SetCode(code, "js");
                    InfoAreaPivot.Items.Add(JavaScriptPivotItem);
                }

                if (!string.IsNullOrEmpty(HamburgerMenu.CurrentSample.CodeUrl))
                {
                    GitHub.NavigateUri = new Uri(HamburgerMenu.CurrentSample.CodeUrl);
                    GitHub.Visibility = Visibility.Visible;
                }
                else
                {
                    GitHub.Visibility = Visibility.Collapsed;
                }

                if (HamburgerMenu.CurrentSample.HasDocumentation)
                {
                    var docs = await this.HamburgerMenu.CurrentSample.GetDocumentationAsync();
                    documentationPath = docs.path;
                    if (!string.IsNullOrWhiteSpace(docs.contents))
                    {
                        DocumentationTextblock.Text = docs.contents;
                        InfoAreaPivot.Items.Add(DocumentationPivotItem);
                    }
                }

                if (InfoAreaPivot.Items.Count == 0)
                {
                    HideInfoArea();
                }

                HamburgerMenu.Title = $"{category.Name} > {HamburgerMenu.CurrentSample?.Name}";
                ApplicationViewExtensions.SetTitle(this, $"{category.Name} > {HamburgerMenu.CurrentSample?.Name}");
            }
        }

        private void HideInfoArea()
        {
            InfoAreaGrid.Visibility = Visibility.Collapsed;
            RootGrid.ColumnDefinitions[1].Width = GridLength.Auto;
            RootGrid.RowDefinitions[2].Height = GridLength.Auto;
            HamburgerMenu.CurrentSample = null;
            Commands.Clear();
            Splitter.Visibility = Visibility.Collapsed;
            HamburgerMenu.Title = string.Empty;
            ApplicationViewExtensions.SetTitle(this, string.Empty);
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandOrCloseProperties();
        }

        private void PivotTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ExpandOrCloseProperties();
        }

        private void ExpandOrCloseProperties()
        {
            var states = VisualStateManager.GetVisualStateGroups(HamburgerMenu).FirstOrDefault();
            string currentState = states.CurrentState.Name;

            switch (currentState)
            {
                case "NarrowState":
                case "MediumState":
                    // If pane is open, close it
                    if (_isPaneOpen)
                    {
                        Grid.SetRowSpan(InfoAreaGrid, 1);
                        Grid.SetRow(InfoAreaGrid, 2);
                        _isPaneOpen = false;
                        ExpandButton.Content = "";
                    }
                    else
                    {
                        // pane is closed, so let's open it
                        Grid.SetRowSpan(InfoAreaGrid, 2);
                        Grid.SetRow(InfoAreaGrid, 1);
                        _isPaneOpen = true;
                        ExpandButton.Content = "";

                        // Update Read-Only XAML tab when switching back to show changes to TwoWay Bound Properties
                        if (HamburgerMenu.CurrentSample?.HasXAMLCode == true && InfoAreaPivot.SelectedItem == XamlReadOnlyPivotItem)
                        {
                            XamlReadOnlyCodeRenderer.SetCode(HamburgerMenu.CurrentSample.UpdatedXamlCode, "xaml");
                        }
                    }

                    break;

                case "WideState":
                    // If pane is open, close it
                    if (_isPaneOpen)
                    {
                        Grid.SetColumnSpan(InfoAreaGrid, 1);
                        Grid.SetColumn(InfoAreaGrid, 1);
                        _isPaneOpen = false;
                        ExpandButton.Content = "";
                    }
                    else
                    {
                        // Pane is closed, so let's open it
                        Grid.SetColumnSpan(InfoAreaGrid, 2);
                        Grid.SetColumn(InfoAreaGrid, 0);
                        _isPaneOpen = true;
                        ExpandButton.Content = "";
                    }

                    break;
            }
        }

        /// <summary>
        /// Called when [back requested] event is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="backRequestedEventArgs">The <see cref="BackRequestedEventArgs"/> instance containing the event data.</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (backRequestedEventArgs.Handled)
            {
                return;
            }

            if (NavigationFrame.CanGoBack)
            {
                NavigationFrame.GoBack();
                backRequestedEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// When the frame has navigated this method is called.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="navigationEventArgs">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigationFrameOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;

            if (_isPaneOpen)
            {
                ExpandOrCloseProperties();
            }

            if (HamburgerMenu.CurrentSample != null && HamburgerMenu.CurrentSample.HasXAMLCode)
            {
                this._lastRenderedProperties = true;

                // Called to load the sample initially as we don't get an Item Pivot Selection Changed with Sample Loaded yet.
                var t = UpdateXamlRenderAsync(HamburgerMenu.CurrentSample.BindedXamlCode);
            }
        }

        private void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var option = e.ClickedItem as Option;
            if (option == null)
            {
                return;
            }

            if (NavigationFrame.CurrentSourcePageType != option.PageType)
            {
                NavigationFrame.Navigate(option.PageType);
            }

            HamburgerMenu.IsPaneOpen = false;

            var expanders = HamburgerMenu.FindDescendants<Expander>();
            foreach (var expander in expanders)
            {
                expander.IsExpanded = false;
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

            if (HamburgerMenu.CurrentSample == null)
            {
                return;
            }

            if (InfoAreaPivot.SelectedItem == PropertiesPivotItem)
            {
                // If we switch to the Properties Panel, we want to use a binded version of the Xaml Code.
                if (HamburgerMenu.CurrentSample.HasXAMLCode)
                {
                    _lastRenderedProperties = true;

                    var t = UpdateXamlRenderAsync(HamburgerMenu.CurrentSample.BindedXamlCode);
                }

                return;
            }

            if (HamburgerMenu.CurrentSample.HasXAMLCode && InfoAreaPivot.SelectedItem == XamlPivotItem && _lastRenderedProperties)
            {
                // Use this flag so we don't re-render the XAML tab if we're switching from tabs other than the properties one.
                _lastRenderedProperties = false;

                // If we switch to the Live Preview, then we want to use the Value based Text
                XamlCodeRenderer.Text = HamburgerMenu.CurrentSample.UpdatedXamlCode;

                var t = UpdateXamlRenderAsync(HamburgerMenu.CurrentSample.UpdatedXamlCode);
                await XamlCodeRenderer.RevealPositionAsync(new Position(1, 1));

                XamlCodeRenderer.Focus(FocusState.Programmatic);
                return;
            }

            if (HamburgerMenu.CurrentSample.HasXAMLCode && InfoAreaPivot.SelectedItem == XamlReadOnlyPivotItem)
            {
                // Update Read-Only XAML tab on non-desktop devices to show changes to Properties
                XamlReadOnlyCodeRenderer.SetCode(HamburgerMenu.CurrentSample.UpdatedXamlCode, "xaml");
            }

            if (HamburgerMenu.CurrentSample.HasCSharpCode && InfoAreaPivot.SelectedItem == CSharpPivotItem)
            {
                var code = await HamburgerMenu.CurrentSample.GetCSharpSourceAsync();
                CSharpCodeRenderer.SetCode(code, "c#");

                return;
            }

            if (HamburgerMenu.CurrentSample.HasJavaScriptCode && InfoAreaPivot.SelectedItem == JavaScriptPivotItem)
            {
                var code = await HamburgerMenu.CurrentSample.GetJavaScriptSourceAsync();
                JavaScriptCodeRenderer.SetCode(code, "js");

                return;
            }
        }

        private async void DocumentationTextblock_OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", e.Link);
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri result))
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
        }

        private async void DocumentationTextblock_ImageResolving(object sender, ImageResolvingEventArgs e)
        {
            var deferral = e.GetDeferral();
            BitmapImage image = null;

            // Determine if the link is not absolute, meaning it is relative.
            if (!Uri.TryCreate(e.Url, UriKind.Absolute, out Uri url))
            {
                url = new Uri(documentationPath + e.Url);
            }

            if (url.Scheme == "ms-appx")
            {
                image = new BitmapImage(url);
            }
            else
            {
                var imageStream = await this.HamburgerMenu.CurrentSample.GetImageStream(url);

                if (imageStream != null)
                {
                    image = new BitmapImage();
                    await image.SetSourceAsync(imageStream);
                }
            }

            // Handle only if no exceptions occur.
            if (image != null)
            {
                e.Image = image;
                e.Handled = true;
            }

            deferral.Complete();
        }

        private void GitHub_OnClick(object sender, RoutedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", GitHub.NavigateUri.ToString());
        }

        private void HamburgerMenu_SamplePickerItemClick(object sender, ItemClickEventArgs e)
        {
            NavigateToSample(e.ClickedItem as Sample);
        }

        private Visibility GreaterThanZero(int value)
        {
            return value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private CssLineStyle _errorStyle = new CssLineStyle()
        {
            BackgroundColor = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xD6, 0xD6))
        };

        private CssGlyphStyle _errorIconStyle = new CssGlyphStyle()
        {
            GlyphImage = new Uri("ms-appx-web:///Icons/Error.png")
        };

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
                var content = NavigationFrame.Content as Page;
                var root = content.FindDescendantByName("XamlRoot");

                if (root is Panel)
                {
                    // If we've defined a 'XamlRoot' element to host us as a panel, use that.
                    (root as Panel).Children.Clear();
                    (root as Panel).Children.Add(element);
                }
                else
                {
                    // Otherwise, just replace the entire page's content
                    content.Content = element;
                }

                // Tell the page we've finished with an update to the XAML contents, after the control has rendered.
                await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    (content as IXamlRenderListener)?.OnXamlRendered(element as FrameworkElement);
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
            if (HamburgerMenu.CurrentSample != null &&
                HamburgerMenu.CurrentSample.HasXAMLCode &&
                _xamlCodeRendererSupported)
            {
                if (_timeSampleEditedFirst != DateTime.MinValue &&
                    _timeSampleEditedLast != DateTime.MinValue)
                {
                    int secondsEdditingSample = (int)Math.Floor((_timeSampleEditedLast - _timeSampleEditedFirst).TotalSeconds);
                    TrackingManager.TrackEvent("xamleditor", "edited", HamburgerMenu.CurrentSample.Name, secondsEdditingSample);
                }
                else
                {
                    TrackingManager.TrackEvent("xamleditor", "not_edited", HamburgerMenu.CurrentSample.Name);
                }
            }

            _timeSampleEditedFirst = _timeSampleEditedLast = DateTime.MinValue;
        }
    }
}