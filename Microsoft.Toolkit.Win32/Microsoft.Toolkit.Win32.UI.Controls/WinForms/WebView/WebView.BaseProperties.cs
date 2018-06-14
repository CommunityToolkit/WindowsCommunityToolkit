// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <inheritdoc cref="Control" />
    public partial class WebView : Control
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
        /// </summary>
        /// <value><see langword="true" /> if the control can accept data that the user drags onto it; otherwise, <see langword="false" />.</value>
        /// <exception cref="NotSupportedException">The value of <see cref="AllowDrop"/> is modified.</exception>
        /// <remarks><see cref="AllowDrop"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AllowDrop
        {
            get => base.AllowDrop;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>A <see cref="Color"/> that represents the background color of the control.</value>
        /// <exception cref="NotSupportedException">The value of <see cref="BackColor"/> is modified.</exception>
        /// <remarks><see cref="BackColor"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get => base.BackColor;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the background image displayed in the control.
        /// </summary>
        /// <value>An <see cref="Image"/> that represents the image to display in the background of the control.</value>
        /// <exception cref="NotSupportedException">The value of <see cref="BackgroundImage"/> is modified.</exception>
        /// <remarks><see cref="BackgroundImage"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get => base.BackgroundImage;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the background image layout as defined in the <see cref="ImageLayout" /> enumeration.
        /// </summary>
        /// <value>One of the values of <see cref="ImageLayout"/>.</value>
        /// <exception cref="NotSupportedException">The value of <see cref="BackgroundImageLayout" /> is modified.</exception>
        /// <remarks><see cref="BackgroundImageLayout"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout
        {
            get => base.BackgroundImageLayout;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
        /// </summary>
        /// <value>A <see cref="Cursor"/> that represents the cursor to display when the mouse pointer is over the control.</value>
        /// <exception cref="NotSupportedException">The value of <see cref="Cursor" /> is modified.</exception>
        /// <remarks><see cref="Cursor"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Cursor Cursor
        {
            get => base.Cursor;
            set => throw new NotSupportedException();
        }

        // Shadowed because the property is not virtual and we needed to override the behavior

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <value><see langword="true" /> if the control can respond to user interaction; otherwise, <see langword="false" />.</value>
        /// <exception cref="NotSupportedException">The value of <see cref="Enabled" /> is modified.</exception>
        /// <remarks><see cref="Enabled"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Enabled
        {
            get => base.Enabled;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The <see cref="Font"/> to apply to the text displayed by the control. </value>
        /// <exception cref="NotSupportedException">The value of <see cref="Font" /> is modified.</exception>
        /// <remarks><see cref="Font"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font
        {
            get => base.Font;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <value>The foreground <see cref="Color"/> of the control. </value>
        /// <exception cref="NotSupportedException">The value of <see cref="ForeColor" /> is modified.</exception>
        /// <remarks><see cref="ForeColor"/> cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => throw new NotSupportedException();
        }

        // Shadowed so we can put the attributes here to hide it in the designer

        /// <summary>
        /// Gets or sets the Input Method Editor (IME) mode of the control.
        /// </summary>
        /// <value>One of the <see cref="ImeMode" /> values. The default is <see cref="ImeMode.Inherit"/>.</value>
        /// /// <remarks>The <see cref="ImeMode"/> property is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImeMode ImeMode
        {
            get => base.ImeMode;
            set => base.ImeMode = value;
        }

        // New so we can put the attributes here to hide it in the designer

        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        /// <value>A <see cref="Padding" /> representing the control's internal spacing characteristics.</value>
        /// /// <remarks>The <see cref="Padding"/> property is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Padding Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        /// <value>One of the <see cref="RightToLeft" /> values.</value>
        /// <exception cref="NotSupportedException">The value of the <see cref="RightToLeft" /> property is modified.</exception>
        /// <remarks>The <see cref="RightToLeft"/> property cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Localizable(false)]
        public override RightToLeft RightToLeft
        {
            get => RightToLeft.No;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <value>The text displayed in the control.</value>
        /// <exception cref="NotSupportedException">The value of the <see cref="Text" /> property is modified.</exception>
        /// <remarks>The <see cref="Text" /> property cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(false)]
        public override string Text
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the wait cursor for the current control and all child controls.
        /// </summary>
        /// <value><see langword="true" /> to use the wait cursor for the current control and all child controls; otherwise, <see langword="false" />.</value>
        /// <exception cref="NotSupportedException">The value of the <see cref="UseWaitCursor" /> property is modified.</exception>
        /// <remarks>The <see cref="UseWaitCursor"/> property cannot be modified and is not visible in the designer.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool UseWaitCursor
        {
            get => base.UseWaitCursor;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        /// <value>The default <see cref="Size"/> of the control.</value>
        protected override Size DefaultSize => new Size(250, 250);
    }
}