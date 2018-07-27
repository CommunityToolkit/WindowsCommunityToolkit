// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation.Metadata;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    // .NET applications use a concept of Union WinMD, which is the union of all known types that exist in the Windows SDK
    // that correspond to the MaxVersionTested setting of the app. If you're running the app on a down-level platform,
    // ApiInformation will tell you the API doesn't exist, but .NET can still JIT  methods based on the Union WinMD and
    // perform some reflection tasks. If you actually try to call the API you will get a MissingMethodException at runtime
    // because the API doesn't really exist.
    // A different problem can occur if you include a higher-versioned .NET library inside a lower-versioned app and try to run
    // it on the higher-versioned build of the OS. In this case, ApiInformation will succeed because the type exists in the
    // system metadata, but .NET will throw a MissingMethodException at runtime because the type didn't exist in the Union WinMD
    // used to build the app.
    public static class ApiInformationExtensions
    {
        public static void ExecuteIfPropertyPresent(string typeName, string propertyName, Action action)
        {
            if (ApiInformation.IsPropertyPresent(typeName, propertyName))
            {
                try
                {
                    action();
                }
                catch (MissingMethodException)
                {
                }
            }
        }

        public static T ExecuteIfPropertyPresent<T>(string typeName, string propertyName, Func<T> action)
        {
            if (ApiInformation.IsPropertyPresent(typeName, propertyName))
            {
                try
                {
                    return action();
                }
                catch (MissingMethodException)
                {
                }
            }

            return default(T);
        }

        public static void ExecuteIfMethodPresent(string typeName, string methodName, Action action)
        {
            if (ApiInformation.IsMethodPresent(typeName, methodName))
            {
                try
                {
                    action();
                }
                catch (MissingMethodException)
                {
                }
            }
        }

        public static void ExecuteIfMethodPresent(string typeName, string methodName, uint inputParameterCount, Action action)
        {
            if (ApiInformation.IsMethodPresent(typeName, methodName, inputParameterCount))
            {
                try
                {
                    action();
                }
                catch (MissingMethodException)
                {
                }
            }
        }

        public static void ExecuteIfEventPresent(string typeName, string eventName, Action action)
        {
            if (ApiInformation.IsEventPresent(typeName, eventName))
            {
                try
                {
                    action();
                }
                catch (MissingMethodException)
                {
                }
            }
        }
    }
}
