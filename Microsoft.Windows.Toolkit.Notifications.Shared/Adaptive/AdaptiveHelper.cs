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

namespace Microsoft.Windows.Toolkit.Notifications.Adaptive
{
    internal static class AdaptiveHelper
    {
        internal static object ConvertToElement(object obj)
        {
            if (obj is AdaptiveText)
                return (obj as AdaptiveText).ConvertToElement();

            else if (obj is AdaptiveImage)
                return (obj as AdaptiveImage).ConvertToElement();

            else if (obj is AdaptiveGroup)
                return (obj as AdaptiveGroup).ConvertToElement();

            else if (obj is AdaptiveSubgroup)
                return (obj as AdaptiveSubgroup).ConvertToElement();

            else
                throw new NotImplementedException("Unknown object: " + obj.GetType());
        }
    }
}
