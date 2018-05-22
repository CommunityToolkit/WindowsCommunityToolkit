// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class MicrosoftGraphServiceAdapter : MicrosoftGraphService
    {
        private static MicrosoftGraphServiceAdapter _instance;

        public static new MicrosoftGraphServiceAdapter Instance => _instance ?? (_instance = new MicrosoftGraphServiceAdapter());

        public string ClientId => AppClientId;

        public string[] Scopes => DelegatedPermissionScopes;

        public new bool IsInitialized => base.IsInitialized;
    }
}
