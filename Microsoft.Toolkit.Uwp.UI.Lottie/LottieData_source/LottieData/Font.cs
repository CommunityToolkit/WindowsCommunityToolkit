// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Font
    {
        public Font(
            string name, 
            string family, 
            string style, 
            double ascent)
        {
            Name = name;
            Family = family;
            Style = style;
            Ascent = ascent;
        }

        public string Name { get; }
        public string Family { get; }
        public string Style{ get; }
        public double Ascent { get; }
    }
}
