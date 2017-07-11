using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Base class providing HttpHelpr hook.
    /// </summary>
    public abstract class HttpDataProviderBase
    {
        private HttpHelper _httpHelper = null;

        /// <summary>
        /// Gets instance of <see cref="HttpHelper"/>
        /// </summary>
        protected HttpHelper HttpHelperInstance => _httpHelper ?? (_httpHelper = new HttpHelper(1));
    }
}
