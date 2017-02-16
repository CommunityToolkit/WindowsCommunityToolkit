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

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Helpers.Emoji;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the Expander control.
    /// </summary>
    public sealed partial class EmojiPage
    {
        private static Emoji[] Emojis => new[] { Emoji.AdmissionTickets, Emoji.ThumbsUpSign, Emoji.FamilyManBoyGirl };

        private EmojiSkinTone[] SkinTonesList => new[] { EmojiSkinTone.Default, EmojiSkinTone.Type12, EmojiSkinTone.Type3, EmojiSkinTone.Type4, EmojiSkinTone.Type5, EmojiSkinTone.Type6 };

        /// <summary>
        /// Initializes a new instance of the <see cref="EmojiPage"/> class.
        /// </summary>
        public EmojiPage()
        {
            InitializeComponent();

            EmojiList.ItemsSource = Emojis;
            EmojiList.SelectedItem = Emojis.First();

            SkinTones.ItemsSource = SkinTonesList;
            SkinTones.SelectedItem = SkinTonesList.First();
        }

        private void EmojiList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var emoji = (Emoji) EmojiList.SelectedItem;
            SkinTones.IsEnabled = emoji.SupportsMultipleColors();
        }
    }
}
