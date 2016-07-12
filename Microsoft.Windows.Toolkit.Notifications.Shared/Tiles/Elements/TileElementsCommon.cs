// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



namespace Microsoft.Windows.Toolkit.Notifications
{
    internal enum TilePresentation
    {
        [EnumString("people")]
        People,

        [EnumString("photos")]
        Photos,

        [EnumString("contact")]
        Contact
    }

    internal enum TileImagePlacement
    {
        [EnumString("inline")]
        Inline,

        [EnumString("background")]
        Background,

        [EnumString("peek")]
        Peek
    }
}