// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;

namespace Microsoft.Windows.Toolkit.Notifications
{
    internal static class TileSizeToAdaptiveTemplateConverter
    {
        public static TileTemplateNameV3 Convert(TileSize size)
        {
            switch (size)
            {
                case TileSize.Small:
                    return TileTemplateNameV3.TileSmall;

                case TileSize.Medium:
                    return TileTemplateNameV3.TileMedium;

                case TileSize.Wide:
                    return TileTemplateNameV3.TileWide;

                case TileSize.Large:
                    return TileTemplateNameV3.TileLarge;

                default:
                    throw new NotImplementedException();
            }
        }
    }

    internal enum TileSize
    {
        Small,
        Medium,
        Wide,
        Large
    }
}