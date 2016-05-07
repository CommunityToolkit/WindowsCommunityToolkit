using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Windows.Toolkit.VisualStudio.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Windows.Toolkit.VisualStudio.ViewModels
{
  internal class AuthenticatorViewModel : ConnectedServiceAuthenticator
  {
    //private string linkText;
    //private string userName;
    //private ICommand changeAuthentication;

    public AuthenticatorViewModel()
    {
      //this.LinkText = "Sign In";
      //this.changeAuthentication = new ChangeAuthenticationCommand(this);
            this.View = new AuthenticatorView();
            this.View.DataContext = this;
            this.IsAuthenticated = true;
    }

    /// <summary>
    /// Gets or sets the text of the hyperlink.
    /// Set with properties to support localization and toggling the state
    /// </summary>
    //public string LinkText
    //{
    //  get { return this.linkText; }
    //  set
    //  {
    //    this.linkText = value;
    //    this.OnPropertyChanged();
    //  }
    //}

    /// <summary>
    /// Gets or sets the currently logged in user's name.
    /// </summary>
    //public string UserName
    //{
    //  get { return this.userName; }
    //  set
    //  {
    //    this.userName = value;
    //    this.OnPropertyChanged();
    //  }
    //}

    /// <summary>
    /// The command that will be invoked when the user clicks the hyperlink.
    /// </summary>
    //public ICommand ChangeAuthentication
    //{
    //  get { return this.changeAuthentication; }
    //}

    /// <summary>
    /// Fake out the signing in and out process by just toggling whether the
    /// user is signed in or out.
    /// </summary>
    //private void ExecuteChangeAuthentication()
    //{
    //  if (this.IsAuthenticated)
    //  {
    //    // Already authenticated, user is triggering Sign Out
    //    this.IsAuthenticated = false;
    //  }
    //  else
    //  {
    //    // If using some custom auth, you'll likely pop some UI
    //    // possibly in a browser page, to redirect login to your service
    //    // Capture the token/result of that login and display the current users identity
    //    this.IsAuthenticated = MessageBox.Show("Login with some UI",
    //        "Login", System.Windows.MessageBoxButton.YesNo,
    //        System.Windows.MessageBoxImage.Information,
    //        System.Windows.MessageBoxResult.Yes) == MessageBoxResult.Yes;
    //  }
    //  if (this.IsAuthenticated)
    //  {
    //    this.LinkText = "Sign out";
    //    this.UserName = "someone@live.com";
    //    this.IsAuthenticated = true;
    //  }
    //  else
    //  {
    //    this.LinkText = "Sign In";
    //    this.UserName = null;
    //    this.IsAuthenticated = false;
    //  }

    //  this.OnAuthenticationChanged(new AuthenticationChangedEventArgs());
    //}

    //private class ChangeAuthenticationCommand : ICommand
    //{
    //  private AuthenticatorViewModel authenticator;

    //  public ChangeAuthenticationCommand(AuthenticatorViewModel authenticator)
    //  {
    //    this.authenticator = authenticator;
    //  }

    //  public event EventHandler CanExecuteChanged { add { } remove { } }

    //  public bool CanExecute(object parameter)
    //  {
    //    return true;
    //  }

    //  public void Execute(object parameter)
    //  {
    //    this.authenticator.ExecuteChangeAuthentication();
    //  }
    //}
  }
}
