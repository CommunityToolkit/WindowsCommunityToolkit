// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Management;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared
{
    public static class ProcessExtensions
    {
        // Define an extension method for type System.Process that returns the command
        // line via WMI.
        public static string GetCommandLine(this Process process)
        {
            string cmdLine = null;
            using (var searcher = new ManagementObjectSearcher($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
            {
                // By definition, the query returns at most 1 match, because the process
                // is looked up by ID (which is unique by definition).
                var matchEnum = searcher.Get().GetEnumerator();
                if (matchEnum.MoveNext()) // Move to the 1st item.
                {
                    cmdLine = matchEnum.Current["CommandLine"]?.ToString();
                }
            }
            if (cmdLine == null)
            {
                // Not having found a command line implies 1 of 2 exceptions, which the
                // WMI query masked:
                // An "Access denied" exception due to lack of privileges.
                // A "Cannot process request because the process (<pid>) has exited."
                // exception due to the process having terminated.
                // We provoke the same exception again simply by accessing process.MainModule.
                var dummy = process.MainModule; // Provoke exception.
            }
            return cmdLine;
        }

    }
}