// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class ShapeLayerContent : LottieObject
    {
        protected private ShapeLayerContent(string name, string matchName) : base(name)
        {
            MatchName = matchName;
        }

        public string MatchName { get; }
        /// <summary>
        /// Gets the <see cref="ShapeContentType"/> of the <see cref="ShapeLayerContent"/> object.
        /// </summary>
        public abstract ShapeContentType ContentType { get; }

    }
}
