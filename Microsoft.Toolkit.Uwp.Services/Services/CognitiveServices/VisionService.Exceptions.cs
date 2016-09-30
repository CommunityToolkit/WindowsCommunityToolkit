using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    public class VisionServiceException : Exception
    {
        public RequestExceptionDetails Details { get; private set; }

        public VisionServiceException(string message, RequestExceptionDetails details)
            : base(message)
        {
            Details = details;
        }
    }
}
