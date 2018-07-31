// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Data.BindingExpression"/>
    /// </summary>
    public class BindingExpression
    {
        internal Windows.UI.Xaml.Data.BindingExpression UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpression"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Data.BindingExpression"/>
        /// </summary>
        public BindingExpression(Windows.UI.Xaml.Data.BindingExpression instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Data.BindingExpression.DataItem"/>
        /// </summary>
        public object DataItem
        {
            get => UwpInstance.DataItem;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Data.BindingExpression.ParentBinding"/>
        /// </summary>
        public Windows.UI.Xaml.Data.Binding ParentBinding
        {
            get => UwpInstance.ParentBinding;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Data.BindingExpression"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.BindingExpression"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Data.BindingExpression"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BindingExpression(
            Windows.UI.Xaml.Data.BindingExpression args)
        {
            return FromBindingExpression(args);
        }

        /// <summary>
        /// Creates a <see cref="BindingExpression"/> from <see cref="Windows.UI.Xaml.Data.BindingExpression"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Data.BindingExpression"/> instance containing the event data.</param>
        /// <returns><see cref="BindingExpression"/></returns>
        public static BindingExpression FromBindingExpression(Windows.UI.Xaml.Data.BindingExpression args)
        {
            return new BindingExpression(args);
        }
    }
}