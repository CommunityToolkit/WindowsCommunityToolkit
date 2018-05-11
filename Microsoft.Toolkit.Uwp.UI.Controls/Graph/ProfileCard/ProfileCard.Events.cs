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
