using Microsoft.Toolkit.Uwp.SampleApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.HamburgerMenuBinding
{
    public class SampleMenuItem : BindableBase
    {
        public string ImagePath { get; internal set; }
        public string Label { get; internal set; }

        private char symbolAsChar;
        public char SymbolAsChar
        {
            get
            {
                return symbolAsChar;
            }
            set
            {
                symbolAsChar = value;
                RaisePropertyChanged();
            }
        }
    }
}