// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class ShapeGroup : ShapeLayerContent
    {
        public ShapeGroup(
            string name,
            string matchName,
            IEnumerable<ShapeLayerContent> items) 
            : base(name, matchName)
        {
            Items = items;
        }

        public IEnumerable<ShapeLayerContent> Items { get; }

        public override ShapeContentType ContentType => ShapeContentType.Group;

        public override LottieObjectType ObjectType => LottieObjectType.ShapeGroup;
    }
}
