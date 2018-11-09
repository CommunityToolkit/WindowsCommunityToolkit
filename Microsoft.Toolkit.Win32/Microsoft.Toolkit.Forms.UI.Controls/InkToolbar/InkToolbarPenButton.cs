// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton"/>
    /// </summary>[Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarPenButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarPenButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarPenButton;

        private System.Collections.Generic.Dictionary<string, object> DesignerProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarPenButton"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton"/>
        /// </summary>
        public InkToolbarPenButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarPenButton).FullName)
        {
        }

        protected InkToolbarPenButton(string name)
            : base(name)
        {
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        /// <inheritdoc />
        protected override void SetContent(UIElement newValue)
        {
            // intentionally empty
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsExtensionGlyphShown { get => (bool)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets the underlying Uwp control's Toolkind property <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.ToolKind"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InkToolbarTool ToolKind { get => (InkToolbarTool)UwpControl?.ToolKind; }

        /// <summary>
        /// Gets or sets the underlying Uwp control's SelectedStrokeWidth property <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidth"/>
        /// </summary>
        [DefaultValue((double)0)]
        public double SelectedStrokeWidth { get => (double)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets the underlying Uwp control's SelectedBrushIndex property <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndex"/>
        /// </summary>
        [DefaultValue((int)0)]
        public int SelectedBrushIndex { get => (int)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets the underlying Uwp control's Palette <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.Palette"/>
        /// </summary>
        [DefaultValue(null)]
        public IList<Brush> Palette
        {
            get => (this.GetUwpControlValue() as IList<Windows.UI.Xaml.Media.Brush>)?.Cast<Brush>().ToList();
            set => this.SetUwpControlValue(value?.Select(x => x.UwpInstance).ToList());
        }

        /// <summary>
        /// Gets or sets the underlying Uwp control's MinStrokeWidth <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidth"/>
        /// </summary>
        [DefaultValue((double)0)]
        public double MinStrokeWidth { get => (double)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets the underlying Uwp control's MaxStrokeWidth <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidth"/>
        /// </summary>
        [DefaultValue((double)0)]
        public double MaxStrokeWidth { get => (double)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets the underlying Uwp control's SesectedBrush <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrush"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush SelectedBrush { get => UwpControl?.SelectedBrush; }
    }
}