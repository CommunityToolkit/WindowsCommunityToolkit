using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkSynchronizer
    {
        private global::Windows.UI.Input.Inking.InkSynchronizer uwpInstance;

        public InkSynchronizer(global::Windows.UI.Input.Inking.InkSynchronizer instance)
        {
            uwpInstance = instance;
        }

        public IReadOnlyList<InkStroke> BeginDry() => uwpInstance.BeginDry().Cast<InkStroke>().ToList();

        public void EndDry() => uwpInstance.EndDry();

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkSynchronizer"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkSynchronizer(
            global::Windows.UI.Input.Inking.InkSynchronizer args)
        {
            return FromInkSynchronizer(args);
        }

        /// <summary>
        /// Creates a <see cref="InkSynchronizer"/> from <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkSynchronizer"/> instance containing the event data.</param>
        /// <returns><see cref="InkSynchronizer"/></returns>
        public static InkSynchronizer FromInkSynchronizer(global::Windows.UI.Input.Inking.InkSynchronizer args)
        {
            return new InkSynchronizer(args);
        }
    }
}