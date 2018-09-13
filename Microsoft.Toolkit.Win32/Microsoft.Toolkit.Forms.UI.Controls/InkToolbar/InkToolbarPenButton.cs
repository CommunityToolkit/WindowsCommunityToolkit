// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton"/>
    /// </summary>[Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarPenButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarPenButton UwpControl { get; set; }

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
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkToolbarPenButton;
        }

        protected override void SetContent()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown { get => UwpControl.IsExtensionGlyphShown; set => UwpControl.IsExtensionGlyphShown = value; }

        /// <summary>
        /// Gets the underlying Uwp control's Toolkind property <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.ToolKind"/>
        /// </summary>
        public InkToolbarTool ToolKind { get => (InkToolbarTool)UwpControl.ToolKind; }

        /// <summary>
        /// Gets or sets the underlying Uwp control's SelectedStrokeWidth property <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidth"/>
        /// </summary>
        public double SelectedStrokeWidth { get => UwpControl.SelectedStrokeWidth; set => UwpControl.SelectedStrokeWidth = value; }

        /// <summary>
        /// Gets or sets the underlying Uwp control's SelectedBrushIndex property <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndex"/>
        /// </summary>
        public int SelectedBrushIndex { get => UwpControl.SelectedBrushIndex; set => UwpControl.SelectedBrushIndex = value; }

        /// <summary>
        /// Gets or sets the underlying Uwp control's Palette <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.Palette"/>
        /// </summary>
        public IList<Brush> Palette { get => UwpControl.Palette?.Cast<Brush>().ToList(); set => UwpControl.Palette = value?.Select(x => x.UwpInstance).ToList(); }

        /// <summary>
        /// Gets or sets the underlying Uwp control's MinStrokeWidth <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidth"/>
        /// </summary>
        public double MinStrokeWidth { get => UwpControl.MinStrokeWidth; set => UwpControl.MinStrokeWidth = value; }

        /// <summary>
        /// Gets or sets the underlying Uwp control's MaxStrokeWidth <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidth"/>
        /// </summary>
        public double MaxStrokeWidth { get => UwpControl.MaxStrokeWidth; set => UwpControl.MaxStrokeWidth = value; }

        /// <summary>
        /// Gets the underlying Uwp control's SesectedBrush <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrush"/>
        /// </summary>
        public Brush SelectedBrush { get => UwpControl.SelectedBrush; }
    }
}