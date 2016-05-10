using Microsoft.Windows.Toolkit.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    [ConnectedServiceOAuth("Facebook")]
    public class FacebookOAuthTokens
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }
    }
}
