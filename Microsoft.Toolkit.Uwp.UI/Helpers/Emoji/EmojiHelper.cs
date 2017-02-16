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
using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI.Helpers.Emoji
{
    /// <summary>
    /// Provides attached properties for emojis
    /// </summary>
    public static class EmojiHelper
    {
        /// <summary>
        /// The emoji property
        /// </summary>
        public static readonly DependencyProperty EmojiProperty = DependencyProperty.RegisterAttached(
            "Emoji", typeof(Emoji), typeof(EmojiHelper), new PropertyMetadata(default(Emoji), OnPropertyChanged));

        /// <summary>
        /// Sets the emoji.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetEmoji(DependencyObject element, Emoji value)
        {
            element.SetValue(EmojiProperty, value);
        }

        /// <summary>
        /// Gets the emoji.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>An emoji enum</returns>
        public static Emoji GetEmoji(DependencyObject element)
        {
            return (Emoji)element.GetValue(EmojiProperty);
        }

        /// <summary>
        /// The emoji skin tone property
        /// </summary>
        public static readonly DependencyProperty EmojiSkinToneProperty = DependencyProperty.RegisterAttached(
            "EmojiSkinTone", typeof(EmojiSkinTone), typeof(EmojiHelper), new PropertyMetadata(EmojiSkinTone.Default, OnPropertyChanged));

        /// <summary>
        /// Sets the emoji skin tone.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetEmojiSkinTone(DependencyObject element, EmojiSkinTone value)
        {
            element.SetValue(EmojiSkinToneProperty, value);
        }

        /// <summary>
        /// Gets the emoji skin tone.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The emoji skin tone value</returns>
        public static EmojiSkinTone GetEmojiSkinTone(DependencyObject element)
        {
            return (EmojiSkinTone)element.GetValue(EmojiSkinToneProperty);
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var emojiText = GetEmojiText(sender);

            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                textBlock.Text = emojiText;
                return;
            }

            var run = sender as Run;
            if (run != null)
            {
                run.Text = emojiText;
                return;
            }
        }

        private static string GetEmojiText(DependencyObject sender)
        {
            var emoji = GetEmoji(sender);
            var skinTone = GetEmojiSkinTone(sender);

            var emojiText = emoji.GetEmoji(skinTone);
            return emojiText;
        }
    }
}
