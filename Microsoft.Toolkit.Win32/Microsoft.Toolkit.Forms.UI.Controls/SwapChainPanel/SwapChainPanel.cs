using System;
using System.Windows;
using Microsoft.Toolkit.Forms.UI.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// Forms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.SwapChainPanel"/>
    /// </summary>
    public class SwapChainPanel : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.SwapChainPanel UwpControl => XamlElement as Windows.UI.Xaml.Controls.SwapChainPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapChainPanel"/> class, a
        /// Forms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.SwapChainPanel"/>
        /// </summary>
        public SwapChainPanel()
            : this(typeof(Windows.UI.Xaml.Controls.SwapChainPanel).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapChainPanel"/> class, a
        /// Forms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.SwapChainPanel"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public SwapChainPanel(string typeName)
            : base(typeName)
        {
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CreateCoreIndependentInputSource"/>
        /// </summary>
        /// <returns>CoreIndependentInputSource</returns>
        public CoreIndependentInputSource CreateCoreIndependentInputSource(CoreInputDeviceTypes deviceTypes) => UwpControl.CreateCoreIndependentInputSource((Windows.UI.Core.CoreInputDeviceTypes)deviceTypes);

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleX"/>
        /// </summary>
        public float CompositionScaleX
        {
            get => UwpControl.CompositionScaleX;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleY"/>
        /// </summary>
        public float CompositionScaleY
        {
            get => UwpControl.CompositionScaleY;
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleChanged"/>
        /// </summary>
        public event EventHandler<object> CompositionScaleChanged = (sender, args) => { };

        private void OnCompositionScaleChanged(Windows.UI.Xaml.Controls.SwapChainPanel sender, object args)
        {
            this.CompositionScaleChanged?.Invoke(this, args);
        }
    }
}