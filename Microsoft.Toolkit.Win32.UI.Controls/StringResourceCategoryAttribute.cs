// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class StringResourceCategoryAttribute : CategoryAttribute
    {
        public StringResourceCategoryAttribute(string category)
            : base(category)
        {
        }

        protected override string GetLocalizedString(string value) => StringResource.GetString(value);
    }
}