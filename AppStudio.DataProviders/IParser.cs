using System.Collections.Generic;

namespace AppStudio.DataProviders
{   
    public interface IParser<T> where T : SchemaBase
    {
        IEnumerable<T> Parse(string data);
    }
}
