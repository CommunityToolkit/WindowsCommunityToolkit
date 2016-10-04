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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="Blade"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeControl
    {
        /// <summary>
        /// Fires whenever a <see cref="Blade"/> is opened
        /// </summary>
        public static event EventHandler<Blade> BladeOpened;

        /// <summary>
        /// Fires whenever a <see cref="Blade"/> is closed
        /// </summary>
        public static event EventHandler<Blade> BladeClosed;
    }
}
