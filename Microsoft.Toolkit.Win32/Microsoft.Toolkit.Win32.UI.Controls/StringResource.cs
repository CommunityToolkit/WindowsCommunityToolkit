// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Resources;
using System.Threading;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    internal sealed class StringResource
    {
        private static StringResource _loader;
        private readonly ResourceManager _resources;

        static StringResource()
        {
        }

        internal StringResource()
        {
            _resources = DesignerUI.ResourceManager;
        }

        public static ResourceManager Resources => GetLoader()?._resources;

        private static CultureInfo Culture => null;

        public static object GetObject(string name) => Resources?.GetObject(name, Culture);

        public static string GetString(string name) => Resources?.GetString(name, Culture);

        private static StringResource GetLoader()
        {
            if (_loader == null)
            {
                var r = new StringResource();
                Interlocked.CompareExchange(ref _loader, r, null);
            }

            return _loader;
        }
    }
}