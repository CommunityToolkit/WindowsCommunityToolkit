using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Text_InfiniteCanvasInputHelper
    {
        [TestCategory("InfiniteCanvas")]
        [TestMethod]
        public void InfiniteCanvas_InputRecieved_TextBlocksCreated()
        {
            var input = @"{\rtf1\fbidis\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}{\f1\fnil Segoe UI;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 10.0.16299}\viewkind4\uc1 
\pard\tx720\cf1\f0\fs23\lang3081 bjhj \b bn\i bn \fs25 sasa\b0\i0\f1\fs23\par
}";

            var result = InfiniteCanvas.processText(input);
        }
    }
}
