using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public delegate void ImageFailedExEventHandler(object sender, ImageFailedExEventArgs e);

    public class ImageFailedExEventArgs : EventArgs
    {
        public ImageFailedExEventArgs(Exception errorException)
        {
            ErrorException = errorException;
            if (ErrorException != null)
            {
                ErrorMessage = ErrorException.Message;
            }
            else
            {
                ErrorMessage = null;
            }
        }

        public Exception ErrorException { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}
