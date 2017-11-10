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

using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
<<<<<<< HEAD
    /// <summary>
    /// Seperates a collection of <see cref="IToolbarItem"/>
    /// </summary>
    public class ToolbarSeparator : AppBarSeparator, IToolbarItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolbarSeparator"/> class.
        /// </summary>
=======
    public class ToolbarSeparator : AppBarSeparator, IToolbarItem
    {
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public ToolbarSeparator()
        {
            this.DefaultStyleKey = typeof(ToolbarSeparator);
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public int Position { get; set; } = -1;
    }
}