namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public class MediaPlayerElement : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.MediaPlayerElement UwpControl { get; set; }

        public MediaPlayerElement()
            : this(typeof(Windows.UI.Xaml.Controls.MediaPlayerElement).FullName)
        {
        }

        protected MediaPlayerElement(string name)
            : base(name)
        {
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.MediaPlayerElement;
        }
    }
}
