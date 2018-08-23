using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    internal class NetFrameworkAuthenticationBroker : IAuthenticationBroker
    {
        private PopupForm popupForm;

        public Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri)
        {
            var taskCompletionSource = new TaskCompletionSource<AuthenticationResult>();
            popupForm = new PopupForm(callbackUri);
            popupForm.FormClosed += (sender, e) =>
            {
                var result = new AuthenticationResult();
                if (popupForm.actualUrl != null)
                {
                    var query = System.Web.HttpUtility.ParseQueryString(popupForm.actualUrl.Query);
                    var auth_token = query.Get("oauth_token");
                    var auth_verifier = query.Get("oauth_verifier");

                    result.ResponseData = query.ToString();
                    result.ResponseStatus = AuthenticationResultStatus.Success;
                }
                else
                {
                    result.ResponseStatus = AuthenticationResultStatus.ErrorHttp;
                }

                taskCompletionSource.SetResult(result);
            };

            return taskCompletionSource.Task;
        }
    }
}
