using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A helper class that allows Connected Animations to be enabled through XAML
    /// </summary>
    /// <seealso cref="ConnectedAnimationService"/>
    public static class Connected
    {
        private static List<ConnectedAnimationProperties> _connectedAnimationsProps = new List<ConnectedAnimationProperties>();
        private static Dictionary<string, ConnectedAnimationProperties> _previousPageConnectedAnimationProps = new Dictionary<string, ConnectedAnimationProperties>();
        private static Dictionary<UIElement, List<UIElement>> _coordinatedAnimationElements = new Dictionary<UIElement, List<UIElement>>();

        private static Frame _navigationFrame;

        /// <summary>
        /// Gets or sets the <see cref="Frame"/> hosting the pages where the Connected Animation will run
        /// Set this value if the frame hosting your pages is different than the root frame of the Window
        /// </summary>
        public static Frame NavigationFrame
        {
            get
            {
                return _navigationFrame;
            }

            set
            {
                if (_navigationFrame != null)
                {
                    _navigationFrame.Navigating -= NavigationFrame_Navigating;
                    _navigationFrame.Navigated -= NavigationFrame_Navigated;
                }

                _navigationFrame = value;
                _navigationFrame.Navigating += NavigationFrame_Navigating;
                _navigationFrame.Navigated += NavigationFrame_Navigated;
            }
        }

        private static void NavigationFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            // make sure all bindings and properties have been set
            RoutedEventHandler handler = null;
            handler = (s, args) =>
            {
                var page = s as Page;
                page.Loaded -= handler;

                object parameter;
                if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.Back)
                {
                    var sourcePage = (sender as Frame).ForwardStack.LastOrDefault();
                    parameter = sourcePage?.Parameter ?? null;
                }
                else
                {
                    parameter = e.Parameter;
                }

                var cas = ConnectedAnimationService.GetForCurrentView();

                foreach (var props in _connectedAnimationsProps)
                {
                    var connectedAnimation = cas.GetAnimation(props.Key);
                    if (connectedAnimation != null)
                    {
                        if (props.IsListAnimation && parameter != null)
                        {
                            props.ListViewBase.ScrollIntoView(parameter);

                            // give time to the UI thread to scroll the list
                            var t = props.ListViewBase.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                try
                                {
                                    var success = await props.ListViewBase.TryStartConnectedAnimationAsync(connectedAnimation, parameter, props.ElementName);
                                }
                                catch (Exception)
                                {
                                    connectedAnimation.Cancel();
                                }
                            });
                        }
                        else if (!props.IsListAnimation)
                        {
                            if (_coordinatedAnimationElements.TryGetValue(props.Element, out var coordinatedElements))
                            {
                                connectedAnimation.TryStart(props.Element, coordinatedElements);
                            }
                            else
                            {
                                connectedAnimation.TryStart(props.Element);
                            }
                        }
                    }

                    if (_previousPageConnectedAnimationProps.ContainsKey(props.Key))
                    {
                        _previousPageConnectedAnimationProps.Remove(props.Key);
                    }
                }

                // if there are animations that were prepared on previous page but no elements on this page have the same key - cancel
                foreach (var previousProps in _previousPageConnectedAnimationProps)
                {
                    var connectedAnimation = cas.GetAnimation(previousProps.Key);
                    if (connectedAnimation != null)
                    {
                        connectedAnimation.Cancel();
                    }
                }

                _previousPageConnectedAnimationProps.Clear();
            };

            var navigatedPage = NavigationFrame.Content as Page;
            navigatedPage.Loaded += handler;
        }

        private static void NavigationFrame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            var parameter = e.Parameter != null && !(e.Parameter is string str && string.IsNullOrEmpty(str)) ? e.Parameter : null;

            var cas = ConnectedAnimationService.GetForCurrentView();
            foreach (var props in _connectedAnimationsProps)
            {
                if (props.IsListAnimation && parameter != null)
                {
                    props.ListViewBase.PrepareConnectedAnimation(props.Key, e.Parameter, props.ElementName);
                }
                else if (!props.IsListAnimation)
                {
                    cas.PrepareToAnimate(props.Key, props.Element);
                }
                else
                {
                    continue;
                }

                _previousPageConnectedAnimationProps[props.Key] = props;
            }

            _connectedAnimationsProps.Clear();
            _coordinatedAnimationElements.Clear();
        }

        /// <summary>
        /// Get the connected animation key associated with the <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/></param>
        /// <returns>the connected animation key</returns>
        public static string GetKey(DependencyObject obj)
        {
            return (string)obj.GetValue(KeyProperty);
        }

        /// <summary>
        /// Sets the connected animation key associated with the <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/></param>
        /// <param name="value">The key to set</param>
        public static void SetKey(DependencyObject obj, string value)
        {
            obj.SetValue(KeyProperty, value);
        }

        public static UIElement GetAnchorElement(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(AnchorElementProperty);
        }

        public static void SetAnchorElement(DependencyObject obj, UIElement value)
        {
            obj.SetValue(AnchorElementProperty, value);
        }

        public static string GetListItemKey(DependencyObject obj)
        {
            return (string)obj.GetValue(ListItemKeyProperty);
        }

        public static void SetListItemKey(DependencyObject obj, string value)
        {
            obj.SetValue(ListItemKeyProperty, value);
        }

        public static string GetListItemElementName(DependencyObject obj)
        {
            return (string)obj.GetValue(ListItemElementNameProperty);
        }

        public static void SetListItemElementName(DependencyObject obj, string value)
        {
            obj.SetValue(ListItemElementNameProperty, value);
        }

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.RegisterAttached("Key",
                                                typeof(string),
                                                typeof(Connected),
                                                new PropertyMetadata(null, OnKeyChanged));

        public static readonly DependencyProperty AnchorElementProperty =
            DependencyProperty.RegisterAttached("AnchorElement",
                                                typeof(UIElement),
                                                typeof(Connected),
                                                new PropertyMetadata(null, OnAnchorElementChanged));

        public static readonly DependencyProperty ListItemKeyProperty =
            DependencyProperty.RegisterAttached("ListItemKey",
                                                typeof(string),
                                                typeof(Connected),
                                                new PropertyMetadata(null, OnListItemKeyChanged));


        public static readonly DependencyProperty ListItemElementNameProperty =
            DependencyProperty.RegisterAttached("ListItemElementName",
                                                typeof(string),
                                                typeof(Connected),
                                                new PropertyMetadata(null, OnListItemElementNameChanged));

        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupFrame();

            if (e.OldValue is string oldKey)
            {
                var item = _connectedAnimationsProps.Where(i => i.Key == oldKey).FirstOrDefault();
                if (item != null)
                {
                    _connectedAnimationsProps.Remove(item);
                }
            }

            if (d is FrameworkElement element && e.NewValue is string newKey)
            {
                var animation = new ConnectedAnimationProperties()
                {
                    Key = newKey,
                    Element = element,
                };

                _connectedAnimationsProps.Add(animation);
            }
        }

        private static void OnAnchorElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement element))
            {
                return;
            }

            if (e.OldValue is UIElement oldElement)
            {
                if (_coordinatedAnimationElements.TryGetValue(oldElement, out var oldElementList))
                {
                    oldElementList.Remove(element);
                }
            }

            if (!(e.NewValue is UIElement anchorElement))
            {
                return;
            }

            if (!_coordinatedAnimationElements.TryGetValue(anchorElement, out var list))
            {
                list = new List<UIElement>();
                _coordinatedAnimationElements[anchorElement] = list;
            }

            list.Add(element);
        }

        private static void OnListItemKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupFrame();

            if (e.OldValue is string oldKey)
            {
                var item = _connectedAnimationsProps.Where(i => i.Key == oldKey).FirstOrDefault();
                if (item != null)
                {
                    _connectedAnimationsProps.Remove(item);
                }
            }

            AddListViewBaseItemAnimationDetails(d);
        }

        private static void OnListItemElementNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupFrame();

            if (e.OldValue is string oldElementName)
            {
                var existingProps = _connectedAnimationsProps.Where(i => i.ElementName != null && i.ElementName == oldElementName).FirstOrDefault();
                if (existingProps != null && e.NewValue is string newElementName)
                {
                    existingProps.ElementName = newElementName;
                    return;
                }
                else if (existingProps != null)
                {
                    _connectedAnimationsProps.Remove(existingProps);
                    return;
                }
            }

            AddListViewBaseItemAnimationDetails(d);
        }

        private static void SetupFrame()
        {
            if (NavigationFrame == null && Window.Current.Content is Frame frame)
            {
                NavigationFrame = frame;
            }
        }

        private static void AddListViewBaseItemAnimationDetails(DependencyObject d)
        {
            if (!(d is Windows.UI.Xaml.Controls.ListViewBase listViewBase))
            {
                return;
            }
            var elementName = GetListItemElementName(d);
            var key = GetListItemKey(d);

            if (string.IsNullOrWhiteSpace(elementName) ||
                string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var props = new ConnectedAnimationProperties()
            {
                Key = key,
                IsListAnimation = true,
                ElementName = elementName,
                ListViewBase = listViewBase
            };

            _connectedAnimationsProps.Add(props);
        }

        internal class ConnectedAnimationProperties
        {
            public string Key { get; set; }
            public UIElement Element { get; set; }
            public List<UIElement> CoordinatedElements { get; set; }
            public string ElementName { get; set; }
            public Windows.UI.Xaml.Controls.ListViewBase ListViewBase { get; set; }
            public bool IsListAnimation { get; set; } = false;
        }
    }
}
