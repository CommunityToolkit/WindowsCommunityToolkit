using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// Result class used by Camera Helper.
    /// </summary>
    public class CameraHelperResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether camera helper initialization was successful or not
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets message indicating information when camera helper initialization is not successful
        /// </summary>
        public string Message { get => message; set => message = value; }

        private string message = string.Empty;
    }
}
