﻿// ******************************************************************
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
    public sealed class ToastActivationOptions
    {
        /// <summary>
        /// If you are using <see cref="ToastActivationType.Protocol"/>, you can optionally specify the target PFN, so that regardless of whether multiple apps are registered to handle the same protocol uri, your desired app will always be launched.
        /// </summary>
        public string ProtocolActivationTargetApplicationPfn { get; set; }

        /// <summary>
        /// Not supported on Windows: Specifies the behavior that the toast should use when the user invokes this action. Note that this option only works on <see cref="ToastButton"/> and <see cref="ToastContextMenuItem"/>.
        /// </summary>
        [Obsolete("Windows does not support AfterActivationBehavior. If a future version of Windows supports this, we will undeprecate the property when support is added.")]
        public ToastAfterActivationBehavior AfterActivationBehavior { get; set; } = ToastAfterActivationBehavior.Default;

        internal void PopulateElement(IElement_ToastActivatable el)
        {
            // If protocol PFN is specified but protocol activation isn't used, throw exception
            if (ProtocolActivationTargetApplicationPfn != null && el.ActivationType != Element_ToastActivationType.Protocol)
            {
                throw new InvalidOperationException($"You cannot specify {nameof(ProtocolActivationTargetApplicationPfn)} without using ActivationType of Protocol.");
            }

            el.ProtocolActivationTargetApplicationPfn = ProtocolActivationTargetApplicationPfn;
#pragma warning disable 618
            el.AfterActivationBehavior = AfterActivationBehavior;
#pragma warning restore 618
        }
    }
}
