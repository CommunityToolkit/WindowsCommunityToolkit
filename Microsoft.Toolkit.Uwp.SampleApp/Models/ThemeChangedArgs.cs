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

using System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.Models
{
    public class ThemeChangedArgs : EventArgs
    {
        /// <summary>
        /// Gets the Current UI Theme
        /// </summary>
        public ElementTheme Theme { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the Theme was set by the User.
        /// </summary>
        public bool CustomSet { get; internal set; }
    }
}