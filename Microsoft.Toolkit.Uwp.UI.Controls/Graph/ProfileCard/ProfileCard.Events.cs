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

        private static void OnDefaultImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var profileCardControl = d as ProfileCard;
            if (string.IsNullOrEmpty(profileCardControl.UserId) || profileCardControl.UserId.Equals("Invalid UserId"))
            {
                profileCardControl.UserPhoto = profileCardControl.DefaultImage;
            }
        }
    }
}
