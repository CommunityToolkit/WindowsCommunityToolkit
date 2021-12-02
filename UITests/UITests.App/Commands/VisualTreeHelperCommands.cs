// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using UITests.App.Pages;

namespace UITests.App.Commands
{
    public static class VisualTreeHelperCommands
    {
        private static DispatcherQueue Queue { get; set; }

        public static Func<Task<double>> GetRasterizationScale { get; private set; }

        private static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        };

        public static void Initialize(DispatcherQueue uiThread, Func<Task<double>> getRasterizationScale)
        {
            Queue = uiThread;
            GetRasterizationScale = getRasterizationScale;
        }

        public static async Task<string> FindElementProperty(string name, string propertyName)
        {
            string result = null;

            if (Queue == null)
            {
                Log.Error("VisualTreeHelper - Missing UI DispatcherQueue");
                return null;
            }

            await Queue.EnqueueAsync(() =>
            {
                // Dispatch?
                var content = App.CurrentWindow.Content as Frame;

                if (content == null)
                {
                    Log.Error("VisualTreeHelper.FindElementProperty - Window has no content.");
                    return;
                }

                Log.Comment("VisualTreeHelper.FindElementProperty('{0}', '{1}')", name, propertyName);

                // 1. Find Element in Visual Tree
                var element = content.FindDescendant(name);

                try
                {
                    Log.Comment("VisualTreeHelper.FindElementProperty - Found Element? {0}", element != null);

                    var typeinfo = element.GetType().GetTypeInfo();

                    Log.Comment("Element Type: {0}", typeinfo.FullName);

                    var prop = element.GetType().GetTypeInfo().GetProperty(propertyName);

                    if (prop == null)
                    {
                        Log.Error("VisualTreeHelper.FindElementProperty - Couldn't find Property named {0} on type {1}", propertyName, typeinfo.FullName);
                        return;
                    }

                    // 2. Get the property using reflection
                    var propValue = prop.GetValue(element);

                    // 3. Serialize and return the result
                    result = JsonSerializer.Serialize(propValue, SerializerOptions);
                }
                catch (Exception e)
                {
                    Log.Error("Error {0}", e.Message);
                    Log.Error("StackTrace:\n{0}", e.StackTrace);
                }
            });

            return result;
        }
    }
}
