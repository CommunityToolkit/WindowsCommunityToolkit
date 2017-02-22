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

using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An icon element that displays Emoji
    /// </summary>
    public class EmojiIcon : Control
    {
        private TextBlock _textBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmojiIcon"/> class.
        /// </summary>
        public EmojiIcon()
        {
            DefaultStyleKey = typeof(EmojiIcon);
        }

        /// <summary>
        /// The emoji property
        /// </summary>
        public static readonly DependencyProperty EmojiProperty = DependencyProperty.Register(
            nameof(Emoji), typeof(Emoji), typeof(EmojiIcon), new PropertyMetadata(default(Emoji), OnEmojiChanged));

        /// <summary>
        /// Gets or sets the emoji.
        /// </summary>
        /// <value>
        /// The emoji.
        /// </value>
        public Emoji Emoji
        {
            get { return (Emoji)GetValue(EmojiProperty); }
            set { SetValue(EmojiProperty, value); }
        }

        /// <summary>
        /// The emoji skin tone property
        /// </summary>
        public static readonly DependencyProperty EmojiSkinToneProperty = DependencyProperty.Register(
            nameof(EmojiSkinTone), typeof(EmojiSkinTone), typeof(EmojiIcon), new PropertyMetadata(default(EmojiSkinTone), OnEmojiChanged));

        /// <summary>
        /// Gets or sets the emoji skin tone.
        /// </summary>
        /// <value>
        /// The emoji skin tone.
        /// </value>
        public EmojiSkinTone EmojiSkinTone
        {
            get { return (EmojiSkinTone)GetValue(EmojiSkinToneProperty); }
            set { SetValue(EmojiSkinToneProperty, value); }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate.
        /// In simplest terms, this means the method is called just before a UI element displays in your app.
        /// Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _textBlock = GetTemplateChild("EmojiText") as TextBlock;

            UpdateEmoji();

            base.OnApplyTemplate();
        }

        private static void OnEmojiChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as EmojiIcon)?.UpdateEmoji();
        }

        private void UpdateEmoji()
        {
            if (_textBlock != null)
            {
                var emojiText = Emoji.GetEmoji(EmojiSkinTone);
                _textBlock.Text = emojiText;
            }
        }
    }
}
