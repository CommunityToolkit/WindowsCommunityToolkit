// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Controls;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        public SampleController()
        {
            _themeListener = new ThemeListener();

            Current = this;
            InitializeComponent();

            _themeListener.ThemeChanged += (s) =>
            {
                ThemeChanged?.Invoke(this, new ThemeChangedArgs { Theme = GetCurrentTheme() });
            };

            // Prevent Pop in on wider screens.
            if (((FrameworkElement)Window.Current.Content).ActualWidth > 700)
            {
                SidePaneState = PaneState.Normal;
            }

            ThemePicker.SelectedIndex = (int)GetCurrentTheme();
            ThemePicker.SelectionChanged += ThemePicker_SelectionChanged;

            DocumentationTextblock.SetRenderer<SampleAppMarkdownRenderer>();

            ProcessSampleEditorTime();
            XamlCodeEditor.UpdateRequested += XamlCodeEditor_UpdateRequested;
        }

        /// <summary>
        /// Gets the Current UI Theme.
        /// </summary>
        /// <returns>The Current UI Theme</returns>
        public ElementTheme GetCurrentTheme()
        {
            return DemoAreaGrid.RequestedTheme;
        }

        /// <summary>
        /// Gets the current System Theme.
        /// </summary>
        /// <returns>System Theme</returns>
        public ApplicationTheme SystemTheme()
        {
            return _themeListener.CurrentTheme;
        }

        /// <summary>
        /// Sets the Current UI Theme.
        /// </summary>
        /// <param name="theme">Theme to set</param>
        public void SetCurrentTheme(ElementTheme theme)
        {
            DemoAreaGrid.RequestedTheme = theme;
            var args = new ThemeChangedArgs
            {
                CustomSet = true,
                Theme = GetCurrentTheme()
            };

            ThemeChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Gets the Current UI Theme.
        /// </summary>
        /// <returns>The Current UI Theme</returns>
        public ElementTheme GetActualTheme()
        {
            var theme = _themeListener.CurrentTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            if (DemoAreaGrid.RequestedTheme != ElementTheme.Default)
            {
                theme = RequestedTheme;
            }

            return theme;
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

        public void ShowExceptionNotification(Exception ex)
        {
            if (ex != null)
            {
                ExceptionNotification.Show(ex.Message);
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
                    ShowExceptionNotification(ex);
                }
            }));
        }

        public void RefreshXamlRender()
        {
            if (CurrentSample != null)
            {
                string code;
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
                    UpdateXamlRender(code);
                }
            }
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

                        // Some samples use the OnNavigatedTo and OnNavigatedFrom
                        // Can't use Frame here because some samples depend on the current Frame
                        MethodInfo method = CurrentSample.PageType.GetMethod(
                            "OnNavigatedTo",
                            BindingFlags.Instance | BindingFlags.NonPublic);

                        if (method != null)
                        {
                            method.Invoke(pageInstance, new object[] { e });
                        }
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

                InfoAreaPivot.Items.Clear();

                if (CurrentSample.HasXAMLCode)
                {
                    // Load Sample Properties before we load sample (if we haven't before)
                    await CurrentSample.PreparePropertyDescriptorAsync();

                    // We only have properties on examples with live XAML
                    var propertyDesc = CurrentSample.PropertyDescriptor;

                    if (propertyDesc != null)
                    {
                        _xamlRenderer.DataContext = propertyDesc.Expando;
                    }

                    if (propertyDesc?.Options.Count > 0)
                    {
                        InfoAreaPivot.Items.Add(PropertiesPivotItem);
                    }

                    if (AnalyticsInfo.VersionInfo.GetDeviceFormFactor() != DeviceFormFactor.Desktop || CurrentSample.DisableXamlEditorRendering)
                    {
                        // Only makes sense (and works) for now to show Live Xaml on Desktop, so fallback to old system here otherwise.
                        XamlReadOnlyCodeRenderer.SetCode(CurrentSample.UpdatedXamlCode, "xaml");

                        InfoAreaPivot.Items.Add(XamlReadOnlyPivotItem);
                    }
                    else
                    {
                        XamlCodeEditor.Text = CurrentSample.UpdatedXamlCode;

                        InfoAreaPivot.Items.Add(XamlPivotItem);

                        _xamlCodeRendererSupported = true;
                    }

                    InfoAreaPivot.SelectedIndex = 0;
                }

                if (CurrentSample.HasCSharpCode)
                {
                    var code = await CurrentSample.GetCSharpSourceAsync();

                    CSharpCodeRenderer.SetCode(code, "c#");
                    InfoAreaPivot.Items.Add(CSharpPivotItem);
                }

                if (CurrentSample.HasJavaScriptCode)
                {
                    var code = await CurrentSample.GetJavaScriptSourceAsync();

                    JavaScriptCodeRenderer.SetCode(code, "js");
                    InfoAreaPivot.Items.Add(JavaScriptPivotItem);
                }

                if (CurrentSample.HasDocumentation)
                {
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
                    var (contents, path) = await CurrentSample.GetDocumentationAsync();
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
                    documentationPath = path;
                    if (!string.IsNullOrWhiteSpace(contents))
                    {
                        DocumentationTextblock.Text = contents;
                        InfoAreaPivot.Items.Add(DocumentationPivotItem);
                    }
                }

                // Hide the Github button if there isn't a CodeUrl.
                if (string.IsNullOrEmpty(CurrentSample.CodeUrl))
                {
                    GithubButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GithubButton.Visibility = Visibility.Visible;
                }

                DataContext = CurrentSample;

                if (InfoAreaPivot.Items.Count == 0)
                {
                    SidePaneState = PaneState.None;
                }
                else
                {
                    SidePaneState = _onlyDocumentation ? PaneState.Full : PaneState.Normal;
                }

                Shell.Current.SetAppTitle($"{CurrentSample.CategoryName} > {CurrentSample.Name}");
            }
            else
            {
                ExceptionNotification.Show("Sample does not exist");
            }

            if (!CanChangePaneState)
            {
                SampleTitleBar.Children.Remove(NarrowInfoButton);
            }

            if (e.NavigationMode != NavigationMode.Back)
            {
                var nop = Samples.PushRecentSample(CurrentSample);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (SamplePage != null)
            {
                MethodInfo method = CurrentSample.PageType.GetMethod(
                    "OnNavigatedFrom",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                if (method != null)
                {
                    method.Invoke(SamplePage, new object[] { e });
                }
            }

            XamlCodeEditor = null;
            _themeListener.Dispose();

            // Not great, but need to collect up after WebView. (Does this work?)
            GC.Collect();
        }

        private void SamplePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentSample != null && CurrentSample.HasXAMLCode)
            {
                _lastRenderedProperties = true;

                // Called to load the sample initially as we don't get an Item Pivot Selection Changed with Sample Loaded yet.
                UpdateXamlRender(CurrentSample.BindedXamlCode);
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

                    UpdateXamlRender(CurrentSample.BindedXamlCode);
                }

                return;
            }

            if (CurrentSample.HasXAMLCode && InfoAreaPivot.SelectedItem == XamlPivotItem && _lastRenderedProperties)
            {
                // Use this flag so we don't re-render the XAML tab if we're switching from tabs other than the properties one.
                _lastRenderedProperties = false;

                // If we switch to the Live Preview, then we want to use the Value based Text
                XamlCodeEditor.Text = CurrentSample.UpdatedXamlCode;

                UpdateXamlRender(CurrentSample.UpdatedXamlCode);
                await XamlCodeEditor.ResetPosition();

                XamlCodeEditor.Focus(FocusState.Programmatic);
                return;
            }

            if (CurrentSample.HasXAMLCode && InfoAreaPivot.SelectedItem == XamlReadOnlyPivotItem)
            {
                // Update Read-Only XAML tab on non-desktop devices to show changes to Properties
                XamlReadOnlyCodeRenderer.SetCode(CurrentSample.UpdatedXamlCode, "xaml");
            }

            if (CurrentSample.HasCSharpCode && InfoAreaPivot.SelectedItem == CSharpPivotItem)
            {
                var code = await CurrentSample.GetCSharpSourceAsync();
                CSharpCodeRenderer.SetCode(code, "c#");

                return;
            }

            if (CurrentSample.HasJavaScriptCode && InfoAreaPivot.SelectedItem == JavaScriptPivotItem)
            {
                var code = await CurrentSample.GetJavaScriptSourceAsync();
                JavaScriptCodeRenderer.SetCode(code, "js");

                return;
            }
        }

        private async void XamlCodeEditor_UpdateRequested(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                UpdateXamlRender(XamlCodeEditor.Text);
            });
        }

        private async void DocumentationTextblock_OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", e.Link);
            var link = e.Link;
            if (e.Link.EndsWith(".md"))
            {
                link = string.Format("https://docs.microsoft.com/en-us/windows/communitytoolkit/{0}/{1}", CurrentSample.CategoryName.ToLower(), link.Replace(".md", string.Empty));
            }

            if (Uri.TryCreate(link, UriKind.Absolute, out Uri result))
            {
                await Launcher.LaunchUriAsync(result);
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
                var imageStream = await CurrentSample.GetImageStream(url);

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

        private async void GitHub_OnClick(object sender, RoutedEventArgs e)
        {
            var url = CurrentSample.CodeUrl;
            TrackingManager.TrackEvent("Link", url);
            try
            {
                await Launcher.LaunchUriAsync(new Uri(url));
            }
            catch
            {
            }
        }

        private void UpdateXamlRender(string text)
        {
            if (XamlCodeEditor == null)
            {
                return;
            }

            // Hide any Previous Errors
            XamlCodeEditor.ClearErrors();

            // Try and Render Xaml to a UIElement
            UIElement element = null;
            try
            {
                element = _xamlRenderer.Render(text);
            }
            catch (Exception ex)
            {
                ShowExceptionNotification(ex);
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
                if (element is FrameworkElement fe)
                {
                    fe.Loaded += XamlFrameworkElement_Loaded;
                }
            }
            else if (_xamlRenderer.Errors.Count > 0)
            {
                var error = _xamlRenderer.Errors.First();

                XamlCodeEditor.ReportError(error);
            }
        }

        private async void XamlFrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                fe.Loaded -= XamlFrameworkElement_Loaded;

                await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    (SamplePage as IXamlRenderListener)?.OnXamlRendered(fe);
                });
            }
        }

        private Visibility GreaterThanZero(int value)
        {
            return value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private Visibility Not(bool value)
        {
            return value ? Visibility.Collapsed : Visibility.Visible;
        }

        private void UpdateProperty([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessSampleEditorTime()
        {
            if (CurrentSample != null &&
                CurrentSample.HasXAMLCode &&
                _xamlCodeRendererSupported)
            {
                if (XamlCodeEditor.TimeSampleEditedFirst != DateTime.MinValue &&
                    XamlCodeEditor.TimeSampleEditedLast != DateTime.MinValue)
                {
                    int secondsEdditingSample = (int)Math.Floor((XamlCodeEditor.TimeSampleEditedLast - XamlCodeEditor.TimeSampleEditedFirst).TotalSeconds);
                    TrackingManager.TrackEvent("xamleditor", "edited", CurrentSample.Name, secondsEdditingSample);
                }
                else
                {
                    TrackingManager.TrackEvent("xamleditor", "not_edited", CurrentSample.Name);
                }
            }

            XamlCodeEditor.ResetTimer();
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
                        XamlReadOnlyCodeRenderer.SetCode(CurrentSample.UpdatedXamlCode, "xaml");
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

        private void ThemePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SetCurrentTheme((ElementTheme)ThemePicker.SelectedIndex);
            }
            catch (Exception ex)
            {
                ShowExceptionNotification(ex);
            }
        }

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

        public bool UseBackground
        {
            get
            {
                return _useBackground;
            }

            set
            {
                _useBackground = value;
                UpdateProperty(nameof(UseBackground));
            }
        }

        private Page SamplePage => SampleContent.Content as Page;

        private bool CanChangePaneState => !_onlyDocumentation;

        private XamlRenderService _xamlRenderer = new XamlRenderService();
        private bool _lastRenderedProperties = true;
        private bool _xamlCodeRendererSupported = false;

        private bool _useBackground = false;

        private PaneState _paneState;
        private bool _onlyDocumentation;
        private string documentationPath;

        private ThemeListener _themeListener;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ThemeChangedArgs> ThemeChanged;
    }
}
