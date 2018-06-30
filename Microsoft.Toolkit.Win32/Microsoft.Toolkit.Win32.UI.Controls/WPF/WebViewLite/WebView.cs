using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Windows.Interop;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using uwpControls = global::Windows.UI.Xaml.Controls;
using uwpInking = Windows.UI.Input.Inking;
using uwpXaml = global::Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    public class WebView : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.WebView UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.WebView;

        public WebView()
            : this(typeof(global::Windows.UI.Xaml.Controls.WebView).FullName)
        {
        }

        // Summary:
        //     Initializes a new instance of the WebView class.
        public WebView(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.WebView.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.WebView.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.WebView.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.WebView.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.WebView.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.WebView.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.WebView.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.WebView.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.WebView.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.WebView.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.WebView.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.WebView.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.WebView.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.WebView.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.WebView.WidthProperty);

            // WebView specific properties
            Bind(nameof(Source), SourceProperty, global::Windows.UI.Xaml.Controls.WebView.SourceProperty);
            Bind(nameof(DefaultBackgroundColor), DefaultBackgroundColorProperty, global::Windows.UI.Xaml.Controls.WebView.DefaultBackgroundColorProperty);
            Bind(nameof(CanGoBack), CanGoBackProperty, global::Windows.UI.Xaml.Controls.WebView.CanGoBackProperty);
            Bind(nameof(CanGoForward), CanGoForwardProperty, global::Windows.UI.Xaml.Controls.WebView.CanGoForwardProperty);
            Bind(nameof(DocumentTitle), DocumentTitleProperty, global::Windows.UI.Xaml.Controls.WebView.DocumentTitleProperty);
            Bind(nameof(ContainsFullScreenElement), ContainsFullScreenElementProperty, global::Windows.UI.Xaml.Controls.WebView.ContainsFullScreenElementProperty);
            Bind(nameof(XYFocusUp), XYFocusUpProperty, global::Windows.UI.Xaml.Controls.WebView.XYFocusUpProperty);
            Bind(nameof(XYFocusRight), XYFocusRightProperty, global::Windows.UI.Xaml.Controls.WebView.XYFocusRightProperty);
            Bind(nameof(XYFocusLeft), XYFocusLeftProperty, global::Windows.UI.Xaml.Controls.WebView.XYFocusLeftProperty);
            Bind(nameof(XYFocusDown), XYFocusDownProperty, global::Windows.UI.Xaml.Controls.WebView.XYFocusDownProperty);
            UwpControl.LoadCompleted += OnLoadCompleted;
            UwpControl.NavigationFailed += OnNavigationFailed;
            UwpControl.ScriptNotify += OnScriptNotify;
            UwpControl.ContentLoading += OnContentLoading;
            UwpControl.DOMContentLoaded += OnDOMContentLoaded;
            UwpControl.FrameContentLoading += OnFrameContentLoading;
            UwpControl.FrameDOMContentLoaded += OnFrameDOMContentLoaded;
            UwpControl.FrameNavigationCompleted += OnFrameNavigationCompleted;
            UwpControl.FrameNavigationStarting += OnFrameNavigationStarting;
            UwpControl.LongRunningScriptDetected += OnLongRunningScriptDetected;
            UwpControl.NavigationCompleted += OnNavigationCompleted;
            UwpControl.NavigationStarting += OnNavigationStarting;
            UwpControl.UnsafeContentWarningDisplaying += OnUnsafeContentWarningDisplaying;
            UwpControl.UnviewableContentIdentified += OnUnviewableContentIdentified;
            UwpControl.ContainsFullScreenElementChanged += OnContainsFullScreenElementChanged;
            UwpControl.NewWindowRequested += OnNewWindowRequested;
            UwpControl.PermissionRequested += OnPermissionRequested;
            UwpControl.UnsupportedUriSchemeIdentified += OnUnsupportedUriSchemeIdentified;
            UwpControl.SeparateProcessLost += OnSeparateProcessLost;
            UwpControl.WebResourceRequested += OnWebResourceRequested;

            base.OnInitialized(e);
        }

        public static DependencyProperty AllowedScriptNotifyUrisProperty { get; } = DependencyProperty.Register(nameof(AllowedScriptNotifyUris), typeof(System.Collections.Generic.IList<System.Uri>), typeof(WebView));

        public static DependencyProperty DataTransferPackageProperty { get; } = DependencyProperty.Register(nameof(DataTransferPackage), typeof(global::Windows.ApplicationModel.DataTransfer.DataPackage), typeof(WebView));

        public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(nameof(Source), typeof(System.Uri), typeof(WebView));

        public static DependencyProperty CanGoBackProperty { get; } = DependencyProperty.Register(nameof(CanGoBack), typeof(bool), typeof(WebView));

        public static DependencyProperty CanGoForwardProperty { get; } = DependencyProperty.Register(nameof(CanGoForward), typeof(bool), typeof(WebView));

        public static DependencyProperty DefaultBackgroundColorProperty { get; } = DependencyProperty.Register(nameof(DefaultBackgroundColor), typeof(global::Windows.UI.Color), typeof(WebView));

        public static DependencyProperty DocumentTitleProperty { get; } = DependencyProperty.Register(nameof(DocumentTitle), typeof(string), typeof(WebView));

        public static DependencyProperty ContainsFullScreenElementProperty { get; } = DependencyProperty.Register(nameof(ContainsFullScreenElement), typeof(bool), typeof(WebView));

        public static DependencyProperty XYFocusDownProperty { get; } = DependencyProperty.Register(nameof(XYFocusDown), typeof(global::Windows.UI.Xaml.DependencyObject), typeof(WebView));

        public static DependencyProperty XYFocusLeftProperty { get; } = DependencyProperty.Register(nameof(XYFocusLeft), typeof(global::Windows.UI.Xaml.DependencyObject), typeof(WebView));

        public static DependencyProperty XYFocusRightProperty { get; } = DependencyProperty.Register(nameof(XYFocusRight), typeof(global::Windows.UI.Xaml.DependencyObject), typeof(WebView));

        public static DependencyProperty XYFocusUpProperty { get; } = DependencyProperty.Register(nameof(XYFocusUp), typeof(global::Windows.UI.Xaml.DependencyObject), typeof(WebView));

        public void NavigateWithHttpRequestMessage(global::Windows.Web.Http.HttpRequestMessage requestMessage) => UwpControl.NavigateWithHttpRequestMessage(requestMessage);

        public bool Focus(global::Windows.UI.Xaml.FocusState value) => UwpControl.Focus(value);

        public void AddWebAllowedObject(string name, object pObject) => UwpControl.AddWebAllowedObject(name, pObject);

        public global::Windows.UI.Xaml.Controls.WebViewDeferredPermissionRequest DeferredPermissionRequestById(uint id) => UwpControl.DeferredPermissionRequestById(id);

        public void Navigate(System.Uri source) => UwpControl.Navigate(source);

        public void NavigateToString(string text) => UwpControl.NavigateToString(text);

        public void GoForward() => UwpControl.GoForward();

        public void GoBack() => UwpControl.GoBack();

        public void Refresh() => UwpControl.Refresh();

        public void Stop() => UwpControl.Stop();

        public global::Windows.Foundation.IAsyncAction CapturePreviewToStreamAsync(global::Windows.Storage.Streams.IRandomAccessStream stream) => UwpControl.CapturePreviewToStreamAsync(stream);

        public global::Windows.Foundation.IAsyncOperation<string> InvokeScriptAsync(string scriptName, System.Collections.Generic.IEnumerable<string> arguments) => UwpControl.InvokeScriptAsync(scriptName, arguments);

        public global::Windows.Foundation.IAsyncOperation<global::Windows.ApplicationModel.DataTransfer.DataPackage> CaptureSelectedContentToDataPackageAsync() => UwpControl.CaptureSelectedContentToDataPackageAsync();

        public void NavigateToLocalStreamUri(System.Uri source, global::Windows.Web.IUriToStreamResolver streamResolver) => UwpControl.NavigateToLocalStreamUri(source, streamResolver);

        public System.Uri BuildLocalStreamUri(string contentIdentifier, string relativePath) => UwpControl.BuildLocalStreamUri(contentIdentifier, relativePath);

        public System.Uri Source
        {
            get => (System.Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public System.Collections.Generic.IList<System.Uri> AllowedScriptNotifyUris
        {
            get => (System.Collections.Generic.IList<System.Uri>)GetValue(AllowedScriptNotifyUrisProperty);
            set => SetValue(AllowedScriptNotifyUrisProperty, value);
        }

        public global::Windows.ApplicationModel.DataTransfer.DataPackage DataTransferPackage
        {
            get => (global::Windows.ApplicationModel.DataTransfer.DataPackage)GetValue(DataTransferPackageProperty);
        }

        public global::Windows.UI.Color DefaultBackgroundColor
        {
            get => (global::Windows.UI.Color)GetValue(DefaultBackgroundColorProperty);
            set => SetValue(DefaultBackgroundColorProperty, value);
        }

        public bool CanGoBack
        {
            get => (bool)GetValue(CanGoBackProperty);
        }

        public bool CanGoForward
        {
            get => (bool)GetValue(CanGoForwardProperty);
        }

        public string DocumentTitle
        {
            get => (string)GetValue(DocumentTitleProperty);
        }

        public bool ContainsFullScreenElement
        {
            get => (bool)GetValue(ContainsFullScreenElementProperty);
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Controls.WebViewDeferredPermissionRequest> DeferredPermissionRequests
        {
            get => UwpControl.DeferredPermissionRequests;
        }

        public global::Windows.UI.Xaml.Controls.WebViewExecutionMode ExecutionMode
        {
            get => UwpControl.ExecutionMode;
        }

        public global::Windows.UI.Xaml.Controls.WebViewSettings Settings
        {
            get => UwpControl.Settings;
        }

        public global::Windows.UI.Xaml.DependencyObject XYFocusUp
        {
            get => (global::Windows.UI.Xaml.DependencyObject)GetValue(XYFocusUpProperty);
            set => SetValue(XYFocusUpProperty, value);
        }

        public global::Windows.UI.Xaml.DependencyObject XYFocusRight
        {
            get => (global::Windows.UI.Xaml.DependencyObject)GetValue(XYFocusRightProperty);
            set => SetValue(XYFocusRightProperty, value);
        }

        public global::Windows.UI.Xaml.DependencyObject XYFocusLeft
        {
            get => (global::Windows.UI.Xaml.DependencyObject)GetValue(XYFocusLeftProperty);
            set => SetValue(XYFocusLeftProperty, value);
        }

        public global::Windows.UI.Xaml.DependencyObject XYFocusDown
        {
            get => (global::Windows.UI.Xaml.DependencyObject)GetValue(XYFocusDownProperty);
            set => SetValue(XYFocusDownProperty, value);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.NavigationEventArgs> LoadCompleted = (sender, e) => { };

        private void OnLoadCompleted(object sender, global::Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            this.LoadCompleted?.Invoke(this, e);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationFailedEventArgs> NavigationFailed = (sender, e) => { };

        private void OnNavigationFailed(object sender, global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventArgs e)
        {
            this.NavigationFailed?.Invoke(this, e);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.NotifyEventArgs> ScriptNotify = (sender, e) => { };

        private void OnScriptNotify(object sender, global::Windows.UI.Xaml.Controls.NotifyEventArgs e)
        {
            this.ScriptNotify?.Invoke(this, e);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewContentLoadingEventArgs> ContentLoading = (sender, args) => { };

        private void OnContentLoading(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs args)
        {
            this.ContentLoading?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewDOMContentLoadedEventArgs> DOMContentLoaded = (sender, args) => { };

        private void OnDOMContentLoaded(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            this.DOMContentLoaded?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewContentLoadingEventArgs> FrameContentLoading = (sender, args) => { };

        private void OnFrameContentLoading(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs args)
        {
            this.FrameContentLoading?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewDOMContentLoadedEventArgs> FrameDOMContentLoaded = (sender, args) => { };

        private void OnFrameDOMContentLoaded(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            this.FrameDOMContentLoaded?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

        private void OnFrameNavigationCompleted(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            this.FrameNavigationCompleted?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationStartingEventArgs> FrameNavigationStarting = (sender, args) => { };

        private void OnFrameNavigationStarting(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs args)
        {
            this.FrameNavigationStarting?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewLongRunningScriptDetectedEventArgs> LongRunningScriptDetected = (sender, args) => { };

        private void OnLongRunningScriptDetected(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs args)
        {
            this.LongRunningScriptDetected?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

        private void OnNavigationCompleted(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            this.NavigationCompleted?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNavigationStartingEventArgs> NavigationStarting = (sender, args) => { };

        private void OnNavigationStarting(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs args)
        {
            this.NavigationStarting?.Invoke(this, args);
        }

        public event EventHandler<object> UnsafeContentWarningDisplaying = (sender, args) => { };

        private void OnUnsafeContentWarningDisplaying(global::Windows.UI.Xaml.Controls.WebView sender, object args)
        {
            this.UnsafeContentWarningDisplaying?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified = (sender, args) => { };

        private void OnUnviewableContentIdentified(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs args)
        {
            this.UnviewableContentIdentified?.Invoke(this, args);
        }

        public event EventHandler<object> ContainsFullScreenElementChanged = (sender, args) => { };

        private void OnContainsFullScreenElementChanged(global::Windows.UI.Xaml.Controls.WebView sender, object args)
        {
            this.ContainsFullScreenElementChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewNewWindowRequestedEventArgs> NewWindowRequested = (sender, args) => { };

        private void OnNewWindowRequested(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs args)
        {
            this.NewWindowRequested?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewPermissionRequestedEventArgs> PermissionRequested = (sender, args) => { };

        private void OnPermissionRequested(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs args)
        {
            this.PermissionRequested?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified = (sender, args) => { };

        private void OnUnsupportedUriSchemeIdentified(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            this.UnsupportedUriSchemeIdentified?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewSeparateProcessLostEventArgs> SeparateProcessLost = (sender, args) => { };

        private void OnSeparateProcessLost(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs args)
        {
            this.SeparateProcessLost?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite.WebViewWebResourceRequestedEventArgs> WebResourceRequested = (sender, args) => { };

        private void OnWebResourceRequested(global::Windows.UI.Xaml.Controls.WebView sender, global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs args)
        {
            this.WebResourceRequested?.Invoke(this, args);
        }
    }
}