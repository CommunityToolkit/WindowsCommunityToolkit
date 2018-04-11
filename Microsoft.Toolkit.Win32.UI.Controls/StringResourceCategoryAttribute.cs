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