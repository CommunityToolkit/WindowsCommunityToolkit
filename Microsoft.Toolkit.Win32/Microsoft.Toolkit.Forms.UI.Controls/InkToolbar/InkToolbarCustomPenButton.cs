// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Interop;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>
    /// </summary>
    [Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarCustomPenButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarCustomPenButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarCustomPenButton;

        private System.Collections.Generic.Dictionary<string, object> DesignerProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPenButton"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>
        /// </summary>
        public InkToolbarCustomPenButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarCustomPenButton).FullName)
        {
        }

        protected InkToolbarCustomPenButton(string name)
            : base(name)
        {
            // Return immediately if control is instantiated by the Visual Studio Designer
            // https://stackoverflow.com/questions/1166226/detecting-design-mode-from-a-controls-constructor
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
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
        /// Gets or sets the underlying Uwp control's CustomPen property <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPen"/>
        /// </summary>
        [DefaultValue(null)]
        public InkToolbarCustomPen CustomPen { get => (InkToolbarCustomPen)this.GetUwpControlValue(); set => this.SetUwpControlValue(value.UwpInstance); }

        /// <summary>
        /// Gets or sets the underlying Uwp control's ConfigurationContent property <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContent"/>
        /// </summary>
        [DefaultValue(null)]
        public object ConfigurationContent { get => this.GetUwpControlValue(); set => this.SetUwpControlValue(value as Windows.UI.Xaml.UIElement); }

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