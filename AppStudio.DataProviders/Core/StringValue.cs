using System;
using System.Reflection;
using AppStudio.DataProviders.Bing;

namespace AppStudio.DataProviders.Core
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
