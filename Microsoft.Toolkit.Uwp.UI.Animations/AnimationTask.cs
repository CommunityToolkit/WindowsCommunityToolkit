using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    internal class AnimationTask
    {
        public Task Task { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan Delay { get; set; }
    }
}
