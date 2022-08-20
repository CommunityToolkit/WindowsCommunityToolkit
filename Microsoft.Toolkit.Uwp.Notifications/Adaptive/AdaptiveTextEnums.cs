// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Text style controls font size, weight, and opacity.
    /// </summary>
    public enum AdaptiveTextStyle
    {
        /// <summary>
        /// Style is determined by the renderer.
        /// </summary>
        Default,

        /// <summary>
        /// Default value. Paragraph font size, normal weight and opacity.
        /// </summary>
        Caption,

        /// <summary>
        /// Same as Caption but with subtle opacity.
        /// </summary>
        CaptionSubtle,

        /// <summary>
        /// H5 font size.
        /// </summary>
        Body,

        /// <summary>
        /// Same as Body but with subtle opacity.
        /// </summary>
        BodySubtle,

        /// <summary>
        /// H5 font size, bold weight. Essentially the bold version of Body.
        /// </summary>
        Base,

        /// <summary>
        /// Same as Base but with subtle opacity.
        /// </summary>
        BaseSubtle,

        /// <summary>
        /// H4 font size.
        /// </summary>
        Subtitle,

        /// <summary>
        /// Same as Subtitle but with subtle opacity.
        /// </summary>
        SubtitleSubtle,

        /// <summary>
        /// H3 font size.
        /// </summary>
        Title,

        /// <summary>
        /// Same as Title but with subtle opacity.
        /// </summary>
        TitleSubtle,

        /// <summary>
        /// Same as Title but with top/bottom padding removed.
        /// </summary>
        TitleNumeral,

        /// <summary>
        /// H2 font size.
        /// </summary>
        Subheader,

        /// <summary>
        /// Same as Subheader but with subtle opacity.
        /// </summary>
        SubheaderSubtle,

        /// <summary>
        /// Same as Subheader but with top/bottom padding removed.
        /// </summary>
        SubheaderNumeral,

        /// <summary>
        /// H1 font size.
        /// </summary>
        Header,

        /// <summary>
        /// Same as Header but with subtle opacity.
        /// </summary>
        HeaderSubtle,

        /// <summary>
        /// Same as Header but with top/bottom padding removed.
        /// </summary>
        HeaderNumeral
    }

    /// <summary>
    /// Controls the horizontal alignment of text.
    /// </summary>
    public enum AdaptiveTextAlign
    {
        /// <summary>
        /// Alignment is automatically determined by
        /// </summary>
        Default,

        /// <summary>
        /// The system automatically decides the alignment based on the language and culture.
        /// </summary>
        Auto,

        /// <summary>
        /// Horizontally align the text to the left.
        /// </summary>
        Left,

        /// <summary>
        /// Horizontally align the text in the center.
        /// </summary>
        Center,

        /// <summary>
        /// Horizontally align the text to the right.
        /// </summary>
        Right
    }

    internal enum AdaptiveTextPlacement
    {
        /// <summary>
        /// Default value
        /// </summary>
        Inline,
        Attribution
    }
}