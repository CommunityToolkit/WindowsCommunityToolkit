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
        public AuthenticatorViewModel()
        {
            this.View = new AuthenticatorView();
            this.View.DataContext = this;
            this.IsAuthenticated = true;
        }
    }
}
