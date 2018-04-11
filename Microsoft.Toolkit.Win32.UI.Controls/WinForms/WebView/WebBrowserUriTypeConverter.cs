using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public class WebBrowserUriTypeConverter : UriTypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var uri = base.ConvertFrom(context, culture, value) as Uri;
            if (uri != null && !string.IsNullOrEmpty(uri.OriginalString) && !uri.IsAbsoluteUri)
            {
                try
                {
                    uri = new Uri("http://" + uri.OriginalString.Trim());
                }
                catch (UriFormatException)
                {
                }
            }

            return uri;
        }
    }
}