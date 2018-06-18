using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Windows.Interop;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using Windows.Web.Http;
using uwpControls = global::Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewLite
{
    public class WebView : WindowsXamlHost
    {
        protected uwpControls.WebView UwpControl => this.XamlRoot as uwpControls.WebView;

        // Summary:
        //     Initializes a new instance of the WebView class.
        public WebView()
            : base()
        {
            TypeName = "Windows.UI.Xaml.Controls.WebView";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, uwpControls.WebView.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpControls.WebView.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpControls.WebView.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpControls.WebView.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpControls.WebView.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpControls.WebView.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpControls.WebView.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpControls.WebView.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpControls.WebView.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpControls.WebView.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpControls.WebView.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpControls.WebView.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpControls.WebView.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpControls.WebView.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpControls.WebView.WidthProperty);

            // WebView specific properties
            // Bind(nameof(AllowedScriptNotifyUris), AllowedScriptNotifyUrisProperty, uwpControls.WebView.AllowedScriptNotifyUrisProperty);
            // Bind(nameof(DataTransferPackage), DataTransferPackageProperty, uwpControls.WebView.DataTransferPackageProperty);
            Bind(nameof(Source), SourceProperty, uwpControls.WebView.SourceProperty);
            Bind(nameof(CanGoBack), CanGoBackProperty, uwpControls.WebView.CanGoBackProperty);
            Bind(nameof(CanGoForward), CanGoForwardProperty, uwpControls.WebView.CanGoForwardProperty);
            Bind(nameof(DefaultBackgroundColor), DefaultBackgroundColorProperty, uwpControls.WebView.DefaultBackgroundColorProperty);
            Bind(nameof(DocumentTitle), DocumentTitleProperty, uwpControls.WebView.DocumentTitleProperty);
            Bind(nameof(ContainsFullScreenElement), ContainsFullScreenElementProperty, uwpControls.WebView.ContainsFullScreenElementProperty);
            Bind(nameof(XYFocusDown), XYFocusDownProperty, uwpControls.WebView.XYFocusDownProperty);
            Bind(nameof(XYFocusLeft), XYFocusLeftProperty, uwpControls.WebView.XYFocusLeftProperty);
            Bind(nameof(XYFocusRight), XYFocusRightProperty, uwpControls.WebView.XYFocusRightProperty);
            Bind(nameof(XYFocusUp), XYFocusUpProperty, uwpControls.WebView.XYFocusUpProperty);
        }

        public static DependencyProperty AllowedScriptNotifyUrisProperty { get; } = DependencyProperty.Register(nameof(AllowedScriptNotifyUris), typeof(IList<Uri>), typeof(WebView));

        public static DependencyProperty DataTransferPackageProperty { get; } = DependencyProperty.Register(nameof(DataTransferPackage), typeof(DataPackage), typeof(WebView));

        public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(nameof(Source), typeof(Uri), typeof(WebView));

        public static DependencyProperty CanGoBackProperty { get; } = DependencyProperty.Register(nameof(CanGoBack), typeof(bool), typeof(WebView));

        public static DependencyProperty CanGoForwardProperty { get; } = DependencyProperty.Register(nameof(CanGoForward), typeof(bool), typeof(WebView));

        public static DependencyProperty DefaultBackgroundColorProperty { get; } = DependencyProperty.Register(nameof(DefaultBackgroundColor), typeof(Color), typeof(WebView));

        public static DependencyProperty DocumentTitleProperty { get; } = DependencyProperty.Register(nameof(DocumentTitle), typeof(string), typeof(WebView));

        public static DependencyProperty ContainsFullScreenElementProperty { get; } = DependencyProperty.Register(nameof(ContainsFullScreenElement), typeof(bool), typeof(WebView));

        public static DependencyProperty XYFocusDownProperty { get; } = DependencyProperty.Register(nameof(XYFocusDown), typeof(DependencyObject), typeof(WebView));

        public static DependencyProperty XYFocusLeftProperty { get; } = DependencyProperty.Register(nameof(XYFocusLeft), typeof(DependencyObject), typeof(WebView));

        public static DependencyProperty XYFocusRightProperty { get; } = DependencyProperty.Register(nameof(XYFocusRight), typeof(DependencyObject), typeof(WebView));

        public static DependencyProperty XYFocusUpProperty { get; } = DependencyProperty.Register(nameof(XYFocusUp), typeof(DependencyObject), typeof(WebView));

        public static WebViewExecutionMode DefaultExecutionMode { get => uwpControls.WebView.DefaultExecutionMode; }

        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty); set => SetValue(SourceProperty, value);
        }

        public IList<Uri> AllowedScriptNotifyUris
        {
            get => (IList<Uri>)GetValue(AllowedScriptNotifyUrisProperty); set => SetValue(AllowedScriptNotifyUrisProperty, value);
        }

        public DataPackage DataTransferPackage
        {
            get => (DataPackage)GetValue(DataTransferPackageProperty);
        }

        public Color DefaultBackgroundColor
        {
            get => (Color)GetValue(DefaultBackgroundColorProperty); set => SetValue(DefaultBackgroundColorProperty, value);
        }

        public bool CanGoBack
        {
            get => (bool)GetValue(CanGoBackProperty); set => SetValue(CanGoBackProperty, value);
        }

        public bool CanGoForward
        {
            get => (bool)GetValue(CanGoForwardProperty); set => SetValue(CanGoForwardProperty, value);
        }

        public string DocumentTitle
        {
            get => (string)GetValue(DocumentTitleProperty); set => SetValue(DocumentTitleProperty, value);
        }

        public bool ContainsFullScreenElement
        {
            get => (bool)GetValue(ContainsFullScreenElementProperty); set => SetValue(ContainsFullScreenElementProperty, value);
        }

        public IList<WebViewDeferredPermissionRequest> DeferredPermissionRequests => UwpControl.DeferredPermissionRequests;

        public WebViewExecutionMode ExecutionMode => UwpControl.ExecutionMode;

        public WebViewSettings Settings => UwpControl.Settings;

        public DependencyObject XYFocusUp
        {
            get => (DependencyObject)GetValue(XYFocusUpProperty); set => SetValue(XYFocusUpProperty, value);
        }

        public DependencyObject XYFocusRight
        {
            get => (DependencyObject)GetValue(XYFocusRightProperty); set => SetValue(XYFocusRightProperty, value);
        }

        public DependencyObject XYFocusLeft
        {
            get => (DependencyObject)GetValue(XYFocusLeftProperty); set => SetValue(XYFocusLeftProperty, value);
        }

        public DependencyObject XYFocusDown
        {
            get => (DependencyObject)GetValue(XYFocusDownProperty); set => SetValue(XYFocusDownProperty, value);
        }

        [RemoteAsync]
        public static IAsyncAction ClearTemporaryWebDataAsync() => uwpControls.WebView.ClearTemporaryWebDataAsync();

        public void Navigate(Uri source) => UwpControl.Navigate(source);

        public void NavigateToString(string text) => UwpControl.NavigateToString(text);

        public void GoForward() => UwpControl.GoForward();

        public void GoBack() => UwpControl.GoBack();

        public void Refresh() => UwpControl.Refresh();

        public void Stop() => UwpControl.Stop();

        [RemoteAsync]
        public IAsyncAction CapturePreviewToStreamAsync(IRandomAccessStream stream) => UwpControl.CapturePreviewToStreamAsync(stream);

        [RemoteAsync]
        public IAsyncOperation<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments) => UwpControl.InvokeScriptAsync(scriptName, arguments);

        [RemoteAsync]
        public IAsyncOperation<DataPackage> CaptureSelectedContentToDataPackageAsync() => UwpControl.CaptureSelectedContentToDataPackageAsync();

        public void NavigateToLocalStreamUri(Uri source, IUriToStreamResolver streamResolver) => UwpControl.NavigateToLocalStreamUri(source, streamResolver);

        public Uri BuildLocalStreamUri(string contentIdentifier, string relativePath) => UwpControl.BuildLocalStreamUri(contentIdentifier, relativePath);

        public void NavigateWithHttpRequestMessage(HttpRequestMessage requestMessage) => UwpControl.NavigateWithHttpRequestMessage(requestMessage);

        public bool Focus(global::Windows.UI.Xaml.FocusState value) => UwpControl.Focus(value);

        public void AddWebAllowedObject(string name, object pObject) => UwpControl.AddWebAllowedObject(name, pObject);

        public WebViewDeferredPermissionRequest DeferredPermissionRequestById(uint id) => UwpControl.DeferredPermissionRequestById(id);

        public event LoadCompletedEventHandler LoadCompleted
        {
            add { UwpControl.LoadCompleted += value; }
            remove { UwpControl.LoadCompleted -= value; }
        }

        public event WebViewNavigationFailedEventHandler NavigationFailed
        {
            add { UwpControl.NavigationFailed += value; }
            remove { UwpControl.NavigationFailed -= value; }
        }

        public event NotifyEventHandler ScriptNotify
        {
            add { UwpControl.ScriptNotify += value; }
            remove { UwpControl.ScriptNotify -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewContentLoadingEventArgs> ContentLoading
        {
            add { UwpControl.ContentLoading += value; }
            remove { UwpControl.ContentLoading -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewDOMContentLoadedEventArgs> DOMContentLoaded
        {
            add { UwpControl.DOMContentLoaded += value; } remove { UwpControl.DOMContentLoaded -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewContentLoadingEventArgs> FrameContentLoading
        {
            add { UwpControl.FrameContentLoading += value; } remove { UwpControl.FrameContentLoading -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewDOMContentLoadedEventArgs> FrameDOMContentLoaded
        {
            add { UwpControl.FrameDOMContentLoaded += value; } remove { UwpControl.FrameDOMContentLoaded -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewNavigationCompletedEventArgs> FrameNavigationCompleted
        {
            add { UwpControl.FrameNavigationCompleted += value; } remove { UwpControl.FrameNavigationCompleted -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewNavigationStartingEventArgs> FrameNavigationStarting
        {
            add { UwpControl.FrameNavigationStarting += value; } remove { UwpControl.FrameNavigationStarting -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewLongRunningScriptDetectedEventArgs> LongRunningScriptDetected
        {
            add { UwpControl.LongRunningScriptDetected += value; } remove { UwpControl.LongRunningScriptDetected -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewNavigationCompletedEventArgs> NavigationCompleted
        {
            add { UwpControl.NavigationCompleted += value; } remove { UwpControl.NavigationCompleted -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewNavigationStartingEventArgs> NavigationStarting
        {
            add { UwpControl.NavigationStarting += value; } remove { UwpControl.NavigationStarting -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, object> UnsafeContentWarningDisplaying
        {
            add { UwpControl.UnsafeContentWarningDisplaying += value; } remove { UwpControl.UnsafeContentWarningDisplaying -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified
        {
            add { UwpControl.UnviewableContentIdentified += value; } remove { UwpControl.UnviewableContentIdentified -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, object> ContainsFullScreenElementChanged
        {
            add { UwpControl.ContainsFullScreenElementChanged += value; } remove { UwpControl.ContainsFullScreenElementChanged -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewNewWindowRequestedEventArgs> NewWindowRequested
        {
            add { UwpControl.NewWindowRequested += value; } remove { UwpControl.NewWindowRequested -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewPermissionRequestedEventArgs> PermissionRequested
        {
            add { UwpControl.PermissionRequested += value; } remove { UwpControl.PermissionRequested -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified
        {
            add { UwpControl.UnsupportedUriSchemeIdentified += value; } remove { UwpControl.UnsupportedUriSchemeIdentified -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewSeparateProcessLostEventArgs> SeparateProcessLost
        {
            add { UwpControl.SeparateProcessLost += value; } remove { UwpControl.SeparateProcessLost -= value; }
        }

        public event TypedEventHandler<uwpControls.WebView, WebViewWebResourceRequestedEventArgs> WebResourceRequested
        {
            add { UwpControl.WebResourceRequested += value; } remove { UwpControl.WebResourceRequested -= value; }
        }
    }
}