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