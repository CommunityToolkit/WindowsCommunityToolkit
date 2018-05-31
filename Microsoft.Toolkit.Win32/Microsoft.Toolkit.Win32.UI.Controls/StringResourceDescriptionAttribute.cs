// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class StringResourceDescriptionAttribute : DescriptionAttribute
    {
        private bool _replaced;

        public StringResourceDescriptionAttribute(string description)
            : base(description)
        {
        }

        public override string Description
        {
            get
            {
                if (!_replaced)
                {
                    _replaced = true;
                    DescriptionValue = StringResource.GetString(Description);
                }

                return base.Description;
            }
        }
    }
}