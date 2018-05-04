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

using Windows.UI.ViewManagement;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Utilities
{
    /// <summary>
    /// Helper class for accessing UISettings properties.
    /// </summary>
    internal static class UISettingsHelper
    {
        private static UISettings _uiSettings = null;

        internal static bool AreSettingsEnablingAnimations
        {
            get
            {
                if (_uiSettings == null)
                {
                    _uiSettings = new UISettings();
                }

                return _uiSettings.AnimationsEnabled;
            }
        }

        internal static bool AreSettingsAutoHidingScrollBars
        {
            get
            {
                // TODO: Use UISettings public API once available
                return true;
            }
        }
    }
}