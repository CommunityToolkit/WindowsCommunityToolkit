using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
  public sealed class WebViewControlLongRunningScriptDetectedEventArgs : EventArgs
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs _args;

    internal WebViewControlLongRunningScriptDetectedEventArgs(Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args)
    {
      _args = args ?? throw new ArgumentNullException(nameof(args));
    }

    public TimeSpan ExecutionTime => _args.ExecutionTime;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "StopPage")]
    public bool StopPageScriptExecution
    {
      get => _args.StopPageScriptExecution;
      set => _args.StopPageScriptExecution = value;
    }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlLongRunningScriptDetectedEventArgs(
      Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args)
    {
      return new WebViewControlLongRunningScriptDetectedEventArgs(args);
    }
  }
}
