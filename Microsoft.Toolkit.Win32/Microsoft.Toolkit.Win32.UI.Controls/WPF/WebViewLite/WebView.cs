using System;
using System.Windows;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    public class WebView : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.WebView UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.WebView;

        public WebView()
            : this("Windows.UI.Xaml.Controls.WebView")
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

            base.OnInitialized(e);
        }

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

        public event global::Windows.UI.Xaml.Navigation.LoadCompletedEventHandler LoadCompleted
        {
            add { UwpControl.LoadCompleted += value; }
            remove { UwpControl.LoadCompleted -= value; }
        }

        public event global::Windows.UI.Xaml.Controls.WebViewNavigationFailedEventHandler NavigationFailed
        {
            add { UwpControl.NavigationFailed += value; }
            remove { UwpControl.NavigationFailed -= value; }
        }

        public event global::Windows.UI.Xaml.Controls.NotifyEventHandler ScriptNotify
        {
            add { UwpControl.ScriptNotify += value; }
            remove { UwpControl.ScriptNotify -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs> ContentLoading
        {
            add { UwpControl.ContentLoading += value; }
            remove { UwpControl.ContentLoading -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs> DOMContentLoaded
        {
            add { UwpControl.DOMContentLoaded += value; }
            remove { UwpControl.DOMContentLoaded -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewContentLoadingEventArgs> FrameContentLoading
        {
            add { UwpControl.FrameContentLoading += value; }
            remove { UwpControl.FrameContentLoading -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs> FrameDOMContentLoaded
        {
            add { UwpControl.FrameDOMContentLoaded += value; }
            remove { UwpControl.FrameDOMContentLoaded -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs> FrameNavigationCompleted
        {
            add { UwpControl.FrameNavigationCompleted += value; }
            remove { UwpControl.FrameNavigationCompleted -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs> FrameNavigationStarting
        {
            add { UwpControl.FrameNavigationStarting += value; }
            remove { UwpControl.FrameNavigationStarting -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewLongRunningScriptDetectedEventArgs> LongRunningScriptDetected
        {
            add { UwpControl.LongRunningScriptDetected += value; }
            remove { UwpControl.LongRunningScriptDetected -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs> NavigationCompleted
        {
            add { UwpControl.NavigationCompleted += value; }
            remove { UwpControl.NavigationCompleted -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs> NavigationStarting
        {
            add { UwpControl.NavigationStarting += value; }
            remove { UwpControl.NavigationStarting -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, object> UnsafeContentWarningDisplaying
        {
            add { UwpControl.UnsafeContentWarningDisplaying += value; }
            remove { UwpControl.UnsafeContentWarningDisplaying -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified
        {
            add { UwpControl.UnviewableContentIdentified += value; }
            remove { UwpControl.UnviewableContentIdentified -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, object> ContainsFullScreenElementChanged
        {
            add { UwpControl.ContainsFullScreenElementChanged += value; }
            remove { UwpControl.ContainsFullScreenElementChanged -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewNewWindowRequestedEventArgs> NewWindowRequested
        {
            add { UwpControl.NewWindowRequested += value; }
            remove { UwpControl.NewWindowRequested -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewPermissionRequestedEventArgs> PermissionRequested
        {
            add { UwpControl.PermissionRequested += value; }
            remove { UwpControl.PermissionRequested -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified
        {
            add { UwpControl.UnsupportedUriSchemeIdentified += value; }
            remove { UwpControl.UnsupportedUriSchemeIdentified -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewSeparateProcessLostEventArgs> SeparateProcessLost
        {
            add { UwpControl.SeparateProcessLost += value; }
            remove { UwpControl.SeparateProcessLost -= value; }
        }

        public event global::Windows.Foundation.TypedEventHandler<global::Windows.UI.Xaml.Controls.WebView, global::Windows.UI.Xaml.Controls.WebViewWebResourceRequestedEventArgs> WebResourceRequested
        {
            add { UwpControl.WebResourceRequested += value; }
            remove { UwpControl.WebResourceRequested -= value; }
        }
    }
}