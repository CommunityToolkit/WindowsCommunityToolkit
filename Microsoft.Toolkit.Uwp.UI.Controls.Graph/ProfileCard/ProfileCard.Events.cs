// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the events for the <see cref="ProfileCard"/> control.
    /// </summary>
    public partial class ProfileCard : Control
    {
        private static void OnUserIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as ProfileCard).FetchUserInfo();

        private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var profileCard = d as ProfileCard;
            ProfileCardItem profileItem = profileCard.CurrentProfileItem.Clone();
            profileItem.DisplayMode = (ViewType)e.NewValue;
            profileCard.CurrentProfileItem = profileItem;
        }

        private static void OnDefaultValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var profileCard = d as ProfileCard;
            var graphService = MicrosoftGraphService.Instance;

            if (!graphService.IsAuthenticated
                || string.IsNullOrEmpty(profileCard.UserId)
                || profileCard.UserId.Equals("Invalid UserId"))
            {
                profileCard.InitUserProfile();
            }
        }
    }
}
