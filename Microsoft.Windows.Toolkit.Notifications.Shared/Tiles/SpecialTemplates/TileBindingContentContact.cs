// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// Phone-only. Supported on Small, Medium, and Wide.
    /// </summary>
    public sealed class TileBindingContentContact : ITileBindingContent
    {
        /// <summary>
        /// Phone-only. Supported on Small, Medium, and Wide.
        /// </summary>
        public TileBindingContentContact() { }

        /// <summary>
        /// The image to display.
        /// </summary>
        public TileBasicImage Image { get; set; }

        /// <summary>
        /// A line of text that is displayed. Not displayed on Small Tile.
        /// </summary>
        public TileBasicText Text { get; set; }

        internal TileTemplateNameV3 GetTemplateName(TileSize size)
        {
            return TileSizeToAdaptiveTemplateConverter.Convert(size);
        }

        internal void PopulateElement(Element_TileBinding binding, TileSize size)
        {
            binding.Presentation = TilePresentation.Contact;

            // Small size doesn't display the text, so no reason to include it in the payload
            if (Text != null && size != TileSize.Small)
                binding.Children.Add(Text.ConvertToElement());

            if (Image != null)
                binding.Children.Add(Image.ConvertToElement());
        }
    }
}