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

using System.Reflection;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Internal class used to provide helpers for controls
    /// </summary>
    internal static partial class ControlHelpers
    {
        private static bool? _isInEnhancedDesignMode;

        /// <summary>
        /// Used to enable or disable user code inside a XAML designer that targets the Windows 10 Fall Creators Update SDK, or later.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Windows.ApplicationModel.DesignMode.DesignModeEnabled returns true when called from user code running inside any version of the XAML designer--regardless of which SDK version you target. This check is recommended for most users.
        /// </para><para>
        /// Starting with the Windows 10 Fall Creators Update, Visual Studio provides a new XAML designer that targets the Windows 10 Fall Creators Update and later.
        /// </para><para>
        /// Use Windows.ApplicationModel.DesignMode.DesignMode2Enabled to differentiate code that depends on functionality only enabled for a XAML designer that targets the Windows 10 Fall Creators Update SDK or later.
        /// </para>
        /// <para>
        /// More info here: https://docs.microsoft.com/en-us/uwp/api/Windows.ApplicationModel.DesignMode
        /// </para>
        /// </remarks>
        /// <returns>True if called from code running inside a XAML designer that targets the Windows 10 Fall Creators Update, or later; otherwise false.</returns>
        internal static bool EnhancedDesignModeEnabled
        {
            get
            {
                if (!_isInEnhancedDesignMode.HasValue)
                {
                    _isInEnhancedDesignMode = false;
                    if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.ApplicationModel.DesignMode", "DesignMode2Enabled"))
                    {
                        // We use reflection to not put a requirement on the SDK we're compiling against
                        var prop = typeof(Windows.ApplicationModel.DesignMode).GetTypeInfo().GetDeclaredProperty("DesignMode2Enabled");
                        if (prop != null && prop.PropertyType == typeof(bool))
                        {
                            _isInEnhancedDesignMode = (bool)prop.GetValue(null);
                        }
                    }
                }

                return _isInEnhancedDesignMode.Value;
            }
        }
    }
}
