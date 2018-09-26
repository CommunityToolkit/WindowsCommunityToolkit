// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace DifferencesGen
{
    public class Program
    {
        private static HashSet<string> enumTypes = new HashSet<string>();
        private static HashSet<string> typeEvents = new HashSet<string>();

        public static void Main(string[] args)
        {
            string min = null;
            string max = null;

            foreach (var arg in args)
            {
                if (arg.StartsWith("/min:"))
                {
                    min = arg.Replace("/min:", string.Empty);
                }
                else if (arg.StartsWith("/max:"))
                {
                    max = arg.Replace("/max:", string.Empty);
                }
            }

            Version.TryParse(min, out Version minVersion);
            Version.TryParse(max, out Version maxVersion);

            if (minVersion == null || maxVersion == null)
            {
                Console.WriteLine("The differences generator needs to be run as follows:");
                Console.WriteLine("DifferencesGen /min:4.0.0.0 /max:5.0.0.0");

                return;
            }

            string folderPath = @"C:\Program Files (x86)\Windows Kits\10\References";

            string universalApiFile = "Windows.Foundation.UniversalApiContract.winmd";

            string universalApiDifferencesCompressedFile = "Differences-{0}.gz";

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, eventArgs) => Assembly.ReflectionOnlyLoad(eventArgs.Name);
            WindowsRuntimeMetadata.ReflectionOnlyNamespaceResolve += (sender, eventArgs) =>
            {
                string path =
                    WindowsRuntimeMetadata.ResolveNamespace(eventArgs.NamespaceName, Enumerable.Empty<string>())
                        .FirstOrDefault();
                if (path == null)
                {
                    return;
                }

                eventArgs.ResolvedAssemblies.Add(Assembly.ReflectionOnlyLoadFrom(path));
            };

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            FileInfo[] files = directoryInfo.GetFiles(universalApiFile, SearchOption.AllDirectories);

            List<Tuple<Version, Assembly>> assemblyList = new List<Tuple<Version, Assembly>>();

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var assembly = Assembly.ReflectionOnlyLoadFrom(file.FullName);

                    var nameParts = assembly.FullName.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                    var versionParts = nameParts[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    var version = Version.Parse(versionParts[1]);

                    if (version >= minVersion && version <= maxVersion)
                    {
                        assemblyList.Add(new Tuple<Version, Assembly>(version, assembly));
                    }
                }
            }

            if (assemblyList.Count >= 2)
            {
                var orderedList = assemblyList.OrderBy(t => t.Item1).ToList();

                for (int i = 1; i < orderedList.Count; i++)
                {
                    var previousVersionAssembly = orderedList[i - 1].Item2;
                    var newerVersionAssembly = orderedList[i].Item2;

                    var version = orderedList[i].Item1;

                    var previousVersionTypes = ProcessAssembly(previousVersionAssembly);
                    var newerVersionTypes = ProcessAssembly(newerVersionAssembly);

                    var addedTypes = new Dictionary<string, List<string>>();

                    foreach (var type in newerVersionTypes)
                    {
                        if (!previousVersionTypes.ContainsKey(type.Key))
                        {
                            addedTypes.Add(type.Key, null);

                            if (enumTypes.Contains(type.Key))
                            {
                                System.Diagnostics.Debug.WriteLine($"New enum {type.Key}");
                            }

                            continue;
                        }

                        HashSet<string> previousVersionTypeMembers = new HashSet<string>(previousVersionTypes[type.Key]);
                        HashSet<string> newerVersionTypeMembers = new HashSet<string>(type.Value);

                        newerVersionTypeMembers.ExceptWith(previousVersionTypeMembers);

                        if (newerVersionTypeMembers.Count == 0)
                        {
                            continue;
                        }

                        if (enumTypes.Contains(type.Key))
                        {
                            System.Diagnostics.Debug.WriteLine($"Enum {type.Key} has new members: {string.Join(",", newerVersionTypeMembers)}");
                        }

                        foreach (var member in newerVersionTypeMembers)
                        {
                            if (typeEvents.Contains($"{type.Key}-{member}"))
                            {
                                System.Diagnostics.Debug.WriteLine($"Type {type.Key} has new event: {member}");
                            }
                        }

                        addedTypes.Add(type.Key, newerVersionTypeMembers.ToList());
                    }

                    StringBuilder stringBuilder = new StringBuilder();

                    using (var compressedFS = File.Create(Path.Combine(AssemblyDirectory, string.Format(universalApiDifferencesCompressedFile, version.ToString()))))
                    {
                        using (var compressionFS = new GZipStream(compressedFS, CompressionMode.Compress))
                        {
                            using (var writer = new StreamWriter(compressionFS))
                            {
                                foreach (var addedType in addedTypes)
                                {
                                    stringBuilder.Clear();

                                    stringBuilder.Append(addedType.Key);

                                    if (addedType.Value != null && addedType.Value.Count > 0)
                                    {
                                        stringBuilder.Append(':');
                                        stringBuilder.Append(string.Join(",", addedType.Value));
                                    }

                                    writer.WriteLine(stringBuilder.ToString());
                                }
                            }
                        }
                    }

                    stringBuilder.Length = 0;
                }
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static Dictionary<string, List<string>> ProcessAssembly(Assembly assembly)
        {
            int pos = assembly.FullName.IndexOf(", Culture");

            string fileName = $"{assembly.FullName.Substring(0, pos)}.json";

            Dictionary<string, List<string>> types = new Dictionary<string, List<string>>();

            foreach (var exportedType in assembly.ExportedTypes)
            {
                List<string> members = new List<string>();

                if (exportedType.IsEnum)
                {
                    if (!enumTypes.Contains(exportedType.FullName))
                    {
                        enumTypes.Add(exportedType.FullName);
                    }

                    foreach (var member in exportedType.GetFields())
                    {
                        if (member.Name.Equals("value__"))
                        {
                            continue;
                        }

                        members.Add(member.Name);
                    }
                }
                else
                {
                    foreach (var methodInfo in exportedType.GetMethods())
                    {
                        if (!methodInfo.IsPublic)
                        {
                            continue;
                        }

                        if (methodInfo.Name.StartsWith("get_") ||
                            methodInfo.Name.StartsWith("set_") ||
                            methodInfo.Name.StartsWith("put_") ||
                            methodInfo.Name.StartsWith("add_") ||
                            methodInfo.Name.StartsWith("remove_"))
                        {
                            continue;
                        }

                        members.Add($"{methodInfo.Name}#{methodInfo.GetParameters().Length}");
                    }

                    foreach (var propertyInfo in exportedType.GetProperties())
                    {
                        members.Add(propertyInfo.Name);
                    }

                    foreach (var eventInfo in exportedType.GetEvents())
                    {
                        typeEvents.Add($"{exportedType.FullName}-{eventInfo.Name}");
                        members.Add(eventInfo.Name);
                    }
                }

                types.Add(exportedType.FullName, members);
            }

            return types;
        }
    }
}
