using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    public interface ISpeechRecognizer {
        event RecognizedEventHandler Recognized;
    }
}
