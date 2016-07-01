// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// TextStacking specifies the vertical alignment of content.
    /// </summary>
    public enum TileTextStacking
    {
        /// <summary>
        /// Vertical align to the top.
        /// </summary>
        [EnumString("top")]
        Top,

        /// <summary>
        /// Vertical align to the center.
        /// </summary>
        [EnumString("center")]
        Center,

        /// <summary>
        /// Vertical align to the bottom.
        /// </summary>
        [EnumString("bottom")]
        Bottom
    }
}