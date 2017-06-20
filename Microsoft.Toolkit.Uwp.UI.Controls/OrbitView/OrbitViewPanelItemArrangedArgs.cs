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
    /// Event args used by the <see cref="OrbitViewPanel"/> ItemArranged event
    /// </summary>
    public class OrbitViewPanelItemArrangedArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="ElementProperties"/> or arranged item
        /// </summary>
        public OrbitViewElementProperties ElementProperties { get; set; }

        /// <summary>
        /// Gets or sets the index of the item that was arranged
        /// </summary>
        public int ItemIndex { get; set; }
    }
}