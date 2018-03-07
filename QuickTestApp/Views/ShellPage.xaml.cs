using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.Uwp.UI.Controls;

using QuickTestApp.Helpers;
using QuickTestApp.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QuickTestApp.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;

        private bool _isPaneOpen;

        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private object _selected;

        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.CompactInline;

        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set { Set(ref _displayMode, value); }
        }

        private object _lastSelectedItem;

        private ObservableCollection<ShellNavigationItem> _primaryItems = new ObservableCollection<ShellNavigationItem>();

        public ObservableCollection<ShellNavigationItem> PrimaryItems
        {
            get { return _primaryItems; }
            set { Set(ref _primaryItems, value); }
        }

        private ObservableCollection<ShellNavigationItem> _secondaryItems = new ObservableCollection<ShellNavigationItem>();

        public ObservableCollection<ShellNavigationItem> SecondaryItems
        {
            get { return _secondaryItems; }
            set { Set(ref _secondaryItems, value); }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = this;
            Initialize();
        }

        private void Initialize()
        {
            NavigationService.Frame = shellFrame;
            NavigationService.Navigated += Frame_Navigated;
            PopulateNavItems();

            InitializeState(Window.Current.Bounds.Width);
        }

        private void InitializeState(double windowWith)
        {
            if (windowWith < WideStateMinWindowWidth)
            {
                GoToState(NarrowStateName);
            }
            else if (windowWith < PanoramicStateMinWindowWidth)
            {
                GoToState(WideStateName);
            }
            else
            {
                GoToState(PanoramicStateName);
            }
        }

        private void PopulateNavItems()
        {
            _primaryItems.Clear();
            _secondaryItems.Clear();

            // TODO WTS: Change the symbols for each item as appropriate for your app
            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
            _primaryItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<BlankPage>("Shell_Blank".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<WebViewPage>("Shell_WebView".GetLocalized(), Symbol.Document));
            _secondaryItems.Add(ShellNavigationItem.FromType<SettingsPage>("Settings", Symbol.Setting));
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            var navigationItem = PrimaryItems?.FirstOrDefault(i => i.PageType == e?.SourcePageType);
            if (navigationItem == null)
            {
                navigationItem = SecondaryItems?.FirstOrDefault(i => i.PageType == e?.SourcePageType);
            }

            if (navigationItem != null)
            {
                ChangeSelected(_lastSelectedItem, navigationItem);
                _lastSelectedItem = navigationItem;
            }
        }

        private void ChangeSelected(object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as ShellNavigationItem).IsSelected = false;
            }

            if (newValue != null)
            {
                (newValue as ShellNavigationItem).IsSelected = true;
                Selected = newValue;
            }
        }

        private void Navigate(object item)
        {
            var navigationItem = item as ShellNavigationItem;
            if (navigationItem != null)
            {
                NavigationService.Navigate(navigationItem.PageType);
            }
        }

        private void ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
            {
                IsPaneOpen = false;
            }

            Navigate(e.InvokedItem);
        }

        private void OpenPane_Click(object sender, RoutedEventArgs e)
        {
            IsPaneOpen = !_isPaneOpen;
        }

        private void WindowStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e) => GoToState(e.NewState.Name);

        private void GoToState(string stateName)
        {
            switch (stateName)
            {
                case PanoramicStateName:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    break;
                case WideStateName:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    IsPaneOpen = false;
                    break;
                case NarrowStateName:
                    DisplayMode = SplitViewDisplayMode.Overlay;
                    IsPaneOpen = false;
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
