// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    internal class NetFrameworkAuthenticationBroker : IAuthenticationBroker
    {
        private PopupWPF popupForm;

        public Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri)
        {
            var taskCompletionSource = new TaskCompletionSource<AuthenticationResult>();
            popupForm = new PopupWPF(callbackUri);
            popupForm.FormClosed += (sender, e) =>
            {
                var result = new AuthenticationResult();
                if (popupForm.ActualUrl != null)
                {
                    var query = System.Web.HttpUtility.ParseQueryString(popupForm.ActualUrl.Query);

                    result.ResponseData = query.ToString();
                    result.ResponseStatus = AuthenticationResultStatus.Success;
                }
                else
                {
                    result.ResponseStatus = AuthenticationResultStatus.ErrorHttp;
                }

                taskCompletionSource.SetResult(result);
                return false;
            };

            popupForm.Show();
            popupForm.NavigateTo(requestUri.AbsoluteUri);

            return taskCompletionSource.Task;
        }
    }
}
