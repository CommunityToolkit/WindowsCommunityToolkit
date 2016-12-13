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

using System;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// New in Creators Update: Additional options relating to activation.
    /// </summary>
    public class ToastActivationOptions
    {
        /// <summary>
        /// If you are using <see cref="ToastActivationType.Protocol"/>, you can optionally specify the target PFN, so that regardless of whether multiple apps are registered to handle the same protocol uri, your desired app will always be launched.
        /// </summary>
        public string ProtocolActivationTargetApplicationPfn { get; set; }

        /// <summary>
        /// Specifies the behavior that the toast should use when the user invokes this action.
        /// </summary>
        public ToastAfterActivationBehavior AfterActivationBehavior { get; set; } = ToastAfterActivationBehavior.Default;

        internal void PopulateElement(IElement_ToastActivatable el)
        {
            el.ProtocolActivationTargetApplicationPfn = ProtocolActivationTargetApplicationPfn;
            el.AfterActivationBehavior = AfterActivationBehavior;
        }
    }
}
