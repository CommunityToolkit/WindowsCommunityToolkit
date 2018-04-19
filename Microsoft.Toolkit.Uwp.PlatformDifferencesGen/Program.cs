using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace DifferencesGen
{
    class Program
    {
        static string path = @"D:\UwpApi";

        static void Main(string[] args)
        {
            string min = null;
            string max = null;

            foreach (var arg in args)
            {
                if (arg.StartsWith("/min:"))
                {
                    min = arg.Replace("/min:", "");
                }
                else if (arg.StartsWith("/max:"))
                {
                    max = arg.Replace("/max:", "");
                }
            }

            Version minVersion = null;
            Version maxVersion = null;

            Version.TryParse(min, out minVersion);
            Version.TryParse(max, out maxVersion);

            if (minVersion == null || maxVersion == null)
            {
                Console.WriteLine("The differences generator needs to be run as follows:");
                Console.WriteLine("DifferencesGen /min:4.0.0.0 /max:5.0.0.0");

                return;
            }
            
            string folderPath = @"C:\Program Files (x86)\Windows Kits\10\References";

            string universalApiFile = "Windows.Foundation.UniversalApiContract.winmd";

            string universalApiDifferencesFile = "Differences-{0}.json";
            string universalApiDifferencesCompressedFile = "Differences-{0}.json.gz";

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, eventArgs) => Assembly.ReflectionOnlyLoad(eventArgs.Name);
            WindowsRuntimeMetadata.ReflectionOnlyNamespaceResolve += (sender, eventArgs) =>
            {
                string path =
                    WindowsRuntimeMetadata.ResolveNamespace(eventArgs.NamespaceName, Enumerable.Empty<string>())
                        .FirstOrDefault();
                if (path == null) return;

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

                            continue;
                        }

                        HashSet<string> previousVersionTypeMembers = new HashSet<string>(previousVersionTypes[type.Key]);
                        HashSet<string> newerVersionTypeMembers = new HashSet<string>(type.Value);

                        newerVersionTypeMembers.ExceptWith(previousVersionTypeMembers);

                        if (newerVersionTypeMembers.Count == 0)
                        {
                            continue;
                        }

                        addedTypes.Add(type.Key, newerVersionTypeMembers.ToList());
                    }

                    File.WriteAllText(Path.Combine(path, string.Format(universalApiDifferencesFile, version.ToString())), JsonConvert.SerializeObject(addedTypes, Formatting.Indented));

                    using (var compressedFS = File.Create(Path.Combine(path, string.Format(universalApiDifferencesCompressedFile, version.ToString()))))
                    {
                        using (var compressionFS = new GZipStream(compressedFS, CompressionMode.Compress))
                        {
                            using (var writer = new StreamWriter(compressionFS))
                            {
                                writer.Write(JsonConvert.SerializeObject(addedTypes, Formatting.Indented));
                            }
                        }
                    }
                }
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

                //var memberInfos = exportedType.FindMembers(MemberTypes.Method | MemberTypes.Property, BindingFlags.Public, null, null);

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
                        methodInfo.Name.StartsWith("remove_")
                        )
                    {
                        continue;
                    }

                    members.Add(methodInfo.Name);
                }

                foreach (var propertyInfo in exportedType.GetProperties())
                {
                    members.Add(propertyInfo.Name);
                }

                types.Add(exportedType.FullName, members);
            }

            File.WriteAllText(Path.Combine(path, fileName), JsonConvert.SerializeObject(types, Formatting.Indented));

            return types;
        }
    }
}
