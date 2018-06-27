using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Microsoft.Windows.Interop
{
    public class DynamicForwardedEventArgs : DynamicObject
    {
        private IDictionary<string, object> _members = new Dictionary<string, object>();

        public object this[string index] { get => _members[index]; set => _members[index] = value; }

        public DynamicForwardedEventArgs(object original)
        {
            Original = original;
            foreach (var aProperty in original.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    this[aProperty.Name] = aProperty.GetGetMethod().Invoke(original, null);
                }
                catch (Exception) { }
            }
        }

        public object Original { get; private set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var returnResult = this._members.ContainsKey(binder.Name);
            result = null;
            if (returnResult)
            {
                result = _members[binder.Name];
            }

            return returnResult;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var returnResult = false;
            try
            {
                _members[binder.Name] = value;
                returnResult = true;
            }
            catch (Exception) { }
            return returnResult;
        }
    }
}
