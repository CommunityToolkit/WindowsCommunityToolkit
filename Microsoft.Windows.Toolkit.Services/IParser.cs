using System.Collections.Generic;

namespace Microsoft.Windows.Toolkit.Services
{   
    public interface IParser<T> where T : SchemaBase
    {
        IEnumerable<T> Parse(string data);
    }
}
