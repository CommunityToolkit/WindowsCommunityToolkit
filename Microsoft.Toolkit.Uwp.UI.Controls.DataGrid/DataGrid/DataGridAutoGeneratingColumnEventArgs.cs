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
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides data for the <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.DataGrid.AutoGeneratingColumn" /> event.
    /// </summary>
    public class DataGridAutoGeneratingColumnEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridAutoGeneratingColumnEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property bound to the generated column.
        /// </param>
        /// <param name="propertyType">
        /// The <see cref="T:System.Type" /> of the property bound to the generated column.
        /// </param>
        /// <param name="column">
        /// The generated column.
        /// </param>
        public DataGridAutoGeneratingColumnEventArgs(string propertyName, Type propertyType, DataGridColumn column)
        {
            this.Column = column;
            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
        }

        /// <summary>
        /// Gets or sets the generated column.
        /// </summary>
        public DataGridColumn Column
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the property bound to the generated column.
        /// </summary>
        public string PropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> of the property bound to the generated column.
        /// </summary>
        public Type PropertyType
        {
            get;
            private set;
        }
    }
}
