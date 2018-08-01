namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public class InkCanvas : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkCanvas UwpControl { get; set; }

        public InkCanvas()
            : this(typeof(Windows.UI.Xaml.Controls.InkCanvas).FullName)
        {
        }

        protected InkCanvas(string name)
            : base(name)
        {
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkCanvas;
            UwpControl.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen | Windows.UI.Core.CoreInputDeviceTypes.Touch;
        }
    }
}
