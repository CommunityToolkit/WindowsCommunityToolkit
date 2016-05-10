using System;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StringValueAttribute : Attribute
    {
        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }

}
