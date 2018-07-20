// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement"/>
    /// </summary>
    public class MapElement
    {
        internal global::Windows.UI.Xaml.Controls.Maps.MapElement UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapElement"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement"/>
        /// </summary>
        public MapElement(global::Windows.UI.Xaml.Controls.Maps.MapElement instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.ZIndex"/>
        /// </summary>
        public int ZIndex
        {
            get => UwpInstance.ZIndex;
            set => UwpInstance.ZIndex = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.Visible"/>
        /// </summary>
        public bool Visible
        {
            get => UwpInstance.Visible;
            set => UwpInstance.Visible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.MapTabIndex"/>
        /// </summary>
        public int MapTabIndex
        {
            get => UwpInstance.MapTabIndex;
            set => UwpInstance.MapTabIndex = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.Tag"/>
        /// </summary>
        public object Tag
        {
            get => UwpInstance.Tag;
            set => UwpInstance.Tag = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.MapStyleSheetEntryState"/>
        /// </summary>
        public string MapStyleSheetEntryState
        {
            get => UwpInstance.MapStyleSheetEntryState;
            set => UwpInstance.MapStyleSheetEntryState = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.MapStyleSheetEntry"/>
        /// </summary>
        public string MapStyleSheetEntry
        {
            get => UwpInstance.MapStyleSheetEntry;
            set => UwpInstance.MapStyleSheetEntry = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement.IsEnabled"/>
        /// </summary>
        public bool IsEnabled
        {
            get => UwpInstance.IsEnabled;
            set => UwpInstance.IsEnabled = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapElement"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapElement(
            global::Windows.UI.Xaml.Controls.Maps.MapElement args)
        {
            return FromMapElement(args);
        }

        /// <summary>
        /// Creates a <see cref="MapElement"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapElement"/> instance containing the event data.</param>
        /// <returns><see cref="MapElement"/></returns>
        public static MapElement FromMapElement(global::Windows.UI.Xaml.Controls.Maps.MapElement args)
        {
            return new MapElement(args);
        }
    }
}