// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Services.Core;
using ApplicationForm = System.Windows.Forms.Application;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    internal class NetFrameworkAuthenticationBroker : IAuthenticationBroker
    {
        public Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri)
        {
            int numberForms = ApplicationForm.OpenForms.Count;
            if (numberForms > 0)
            {
                return AutenticateForm(requestUri, callbackUri);
            }
            else if (Application.Current != null)
            {
                return AutenticateWindow(requestUri, callbackUri);
            }
            else
            {
                // Your code shouldn't reach this exception.
                throw new Exception("Cannot identify the current application. Please review your main app");
            }
        }

        public async Task<AuthenticationResult> AutenticateWindow(Uri requestUri, Uri callbackUri)
        {
            PopupWPF popupWindow;
            var taskCompletionSource = new TaskCompletionSource<AuthenticationResult>();
            popupWindow = new PopupWPF(callbackUri);
            popupWindow.Closed += (sender, e) =>
            {
                taskCompletionSource.SetResult(HandleExit(popupWindow.ActualUrl));
            };

            popupWindow.Show();
            popupWindow.NavigateTo(requestUri.AbsoluteUri);
            return await taskCompletionSource.Task;
        }

        public async Task<AuthenticationResult> AutenticateForm(Uri requestUri, Uri callbackUri)
        {
            PopupForm popupForm;
            var taskCompletionSource = new TaskCompletionSource<AuthenticationResult>();
            popupForm = new PopupForm(callbackUri);
            popupForm.FormClosed += (sender, e) =>
            {
                taskCompletionSource.SetResult(HandleExit(popupForm.ActualUrl));
            };

            popupForm.Show();
            popupForm.NavigateTo(requestUri.AbsoluteUri);
            return await taskCompletionSource.Task;
        }

        private AuthenticationResult HandleExit(Uri actualUrl)
        {
            var result = new AuthenticationResult();
            if (actualUrl != null)
            {
                var query = System.Web.HttpUtility.ParseQueryString(actualUrl.Query);

                result.ResponseData = query.ToString();
                result.ResponseStatus = AuthenticationResultStatus.Success;
            }
            else
            {
                result.ResponseStatus = AuthenticationResultStatus.ErrorHttp;
            }

            return result;
        }
    }
}
