// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Automation.Peers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="ContentSizer"/> is a control which can be used to resize any element, usually its parent. If you are using a <see cref="Grid"/>, use <see cref="GridSplitter"/> instead.
    /// </summary>
    [ContentProperty(Name = nameof(Content))]
    public partial class ContentSizer : SplitBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSizer"/> class.
        /// </summary>
        public ContentSizer()
        {
            this.DefaultStyleKey = typeof(ContentSizer);

            // TODO: Can this be set in XAML, do we open a WinUI issue to track?
            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            KeyUp += ContentSizer_KeyUp;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Note, we re-register for the proper timing to check for default property values. If we just set Loaded once in our constructor this doesn't work... Not sure why... 🤷‍

            // Unhook registered events
            Loaded -= ContentSizer_Loaded;

            // Register Events
            Loaded += ContentSizer_Loaded;
        }

        /// <summary>
        /// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
        /// </summary>
        /// <returns>An automation peer for this <see cref="ContentSizer"/>.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ContentSizerAutomationPeer(this);
        }
    }
}
