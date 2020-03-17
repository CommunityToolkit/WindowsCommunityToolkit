// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel that allows for a Master/Details pattern.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ItemsControl" />
    public partial class MasterDetailsView
    {
        private AppViewBackButtonVisibility? previousSystemBackButtonVisibility;
        private bool previousNavigationViewBackEnabled;

        // Int used because the underlying type is an enum, but we don't have access to the enum
        private int previousNavigationViewBackVisibilty;
        private Button inlineBackButton;
        private object navigationView;
        private Frame frame;

        /// <summary>
        /// Sets the back button visibility based on the current visual state and selected item
        /// </summary>
        private void SetBackButtonVisibility(MasterDetailsViewState? previousState = null)
        {
            const int backButtonVisible = 1;

            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (ViewState == MasterDetailsViewState.Details)
            {
                if ((BackButtonBehavior == BackButtonBehavior.Inline) && (inlineBackButton != null))
                {
                    inlineBackButton.Visibility = Visibility.Visible;
                }
                else if (BackButtonBehavior == BackButtonBehavior.Automatic)
                {
                    // Continue to support the system back button if it is being used
                    SystemNavigationManager navigationManager = SystemNavigationManager.GetForCurrentView();
                    if (navigationManager.AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible)
                    {
                        // Setting this indicates that the system back button is being used
                        previousSystemBackButtonVisibility = navigationManager.AppViewBackButtonVisibility;
                    }
                    else if ((inlineBackButton != null) && ((navigationView == null) || (frame == null)))
                    {
                        // We can only use the new NavigationView if we also have a Frame
                        // If there is no frame we have to use the inline button
                        inlineBackButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SetNavigationViewBackButtonState(backButtonVisible, true);
                    }
                }
                else if (BackButtonBehavior != BackButtonBehavior.Manual)
                {
                    SystemNavigationManager navigationManager = SystemNavigationManager.GetForCurrentView();
                    previousSystemBackButtonVisibility = navigationManager.AppViewBackButtonVisibility;

                    navigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
            }
            else if (previousState == MasterDetailsViewState.Details)
            {
                if ((BackButtonBehavior == BackButtonBehavior.Inline) && (inlineBackButton != null))
                {
                    inlineBackButton.Visibility = Visibility.Collapsed;
                }
                else if (BackButtonBehavior == BackButtonBehavior.Automatic)
                {
                    if (!previousSystemBackButtonVisibility.HasValue)
                    {
                        if ((inlineBackButton != null) && ((navigationView == null) || (frame == null)))
                        {
                            inlineBackButton.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            SetNavigationViewBackButtonState(previousNavigationViewBackVisibilty, previousNavigationViewBackEnabled);
                        }
                    }
                }

                if (previousSystemBackButtonVisibility.HasValue)
                {
                    // Make sure we show the back button if the stack can navigate back
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = previousSystemBackButtonVisibility.Value;
                    previousSystemBackButtonVisibility = null;
                }
            }
        }

        private void SetNavigationViewBackButtonState(int visible, bool enabled)
        {
            if (navigationView == null)
            {
                return;
            }

            System.Type navType = navigationView.GetType();
            PropertyInfo visibleProperty = navType.GetProperty("IsBackButtonVisible");
            if (visibleProperty != null)
            {
                previousNavigationViewBackVisibilty = (int)visibleProperty.GetValue(navigationView);
                visibleProperty.SetValue(navigationView, visible);
            }

            PropertyInfo enabledProperty = navType.GetProperty("IsBackEnabled");
            if (enabledProperty != null)
            {
                previousNavigationViewBackEnabled = (bool)enabledProperty.GetValue(navigationView);
                enabledProperty.SetValue(navigationView, enabled);
            }
        }

        /// <summary>
        /// Closes the details pane if we are in narrow state
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The event args</param>
        private void OnFrameNavigating(object sender, NavigatingCancelEventArgs args)
        {
            if ((args.NavigationMode == NavigationMode.Back) && (ViewState == MasterDetailsViewState.Details))
            {
                SelectedItem = null;
                args.Cancel = true;
            }
        }

        /// <summary>
        /// Closes the details pane if we are in narrow state
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The event args</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
            if (ViewState == MasterDetailsViewState.Details)
            {
                // let the OnFrameNavigating method handle it if
                if (frame == null || !frame.CanGoBack)
                {
                    SelectedItem = null;
                }

                args.Handled = true;
            }
        }

        private void OnInlineBackButtonClicked(object sender, RoutedEventArgs e)
        {
            SelectedItem = null;
        }
    }
}
