using System;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StringValueAttribute : Attribute
    {
        private string value;

        public StringValueAttribute(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }

}
