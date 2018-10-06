using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    class AssemblyBuilderHelper
    {
        internal static void MessageHandlersWrapper(StringBuilder messageHandlerBuilder, bool needElse, MessageType messageType, string handlers,
            string assetNotFoundExceptionTypeName, string wrappedExceptionTypeName, string serializingHelperName, string communicateInterfaceTypeName,
            LocalExceptionHandlingMode localExceptionHandlingMode, string interfaceTypeName)
        {
            if (needElse) messageHandlerBuilder.Append("else ");
            messageHandlerBuilder.Append("if (messageType == SecretNest.RemoteAgency.MessageType.").Append(messageType.ToString()).AppendLine(")\n{");
            if (string.IsNullOrEmpty(handlers))
            {
                ReturnExceptionInMessageHandler(messageHandlerBuilder, assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeName);
            }
            else
            {
                messageHandlerBuilder.AppendLine("switch (assetName)\n{")
                    .Append(handlers)
                    .AppendLine("default:");
                ReturnExceptionInMessageHandler(messageHandlerBuilder, assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeName);
                messageHandlerBuilder.AppendLine("break;\n}");
            }
            messageHandlerBuilder.AppendLine("}");
        }

        static readonly string wrappedAssetNotFoundExceptionName = NamingHelper.GetRandomName("wrappedAssetNotFoundException");
        static void ReturnExceptionInMessageHandler(StringBuilder messageHandlerBuilder,
            string assetNotFoundExceptionTypeName, string wrappedExceptionTypeName, string serializingHelperName, string communicateInterfaceTypeName,
            LocalExceptionHandlingMode localExceptionHandlingMode, string interfaceTypeName)
        {
            messageHandlerBuilder.Append("if (!isOneWay)\n{var ")
                .Append(wrappedAssetNotFoundExceptionName).Append(" = ")
                .Append(wrappedExceptionTypeName).Append(".Create(new ")
                .Append(assetNotFoundExceptionTypeName).AppendLine("(messageType, assetName));");

            CodeBuilderHelper.BuildSendingExceptionCode(messageHandlerBuilder,
                serializingHelperName, localExceptionHandlingMode,
                communicateInterfaceTypeName, interfaceTypeName, wrappedExceptionTypeName,
                "messageType", wrappedAssetNotFoundExceptionName);

            messageHandlerBuilder.AppendLine("}");
        }

#if netcore
        static Lazy<TrustedPlatformAssemblies> tpa = new Lazy<TrustedPlatformAssemblies>(false);
#endif

        internal void ClearBuilderAssemblyCache()
        {
#if netcore
            tpa = new Lazy<TrustedPlatformAssemblies>(false);
#endif
            builder.ClearAssemblyCache();
        }

        CSharpRoslynAgency.Builder builder = new CSharpRoslynAgency.Builder();

        Action<AssemblyRequestingEventArgs> raiseMissingAssemblyRequestingCallback;

        internal AssemblyBuilderHelper(Action<AssemblyRequestingEventArgs> raiseMissingAssemblyRequestingCallback)
        {
            builder.MissingAssemblyResolving += Builder_MissingAssemblyResolving;
            this.raiseMissingAssemblyRequestingCallback = raiseMissingAssemblyRequestingCallback;
        }

        private void Builder_MissingAssemblyResolving(object sender, CSharpRoslynAgency.MissingAssemblyResolvingEventArgs e)
        {
            if (!e.IsModule)
            {
#if netcore
                //process with trusted platform
                var assemblyFullName = e.AssemblyName.FullName;
                var image = tpa.Value.LoadAssembly(assemblyFullName);
                if (image != null)
                {
                    e.MissingAssemblyImage = image;
                    return;
                }
#else
                //process with file loading
                var assembly = Assembly.Load(e.AssemblyName);
                var filename = assembly.Location;
                if (!string.IsNullOrEmpty(filename))
                {
                    e.MissingAssemblyImage = File.ReadAllBytes(filename);
                    return;
                }
#endif
            }

            //process with MissingAssemblyRequesting
            AssemblyRequestingEventArgs args = new AssemblyRequestingEventArgs(e);
            raiseMissingAssemblyRequestingCallback(args);

            if (args.IsLoaded)
                e.MissingAssemblyImage = args.MissingAssemblyImage;
        }

        struct AssemblyInfo
        {
            public AssemblyName AssemblyName;
            public bool IsModule;
            public bool EmbedInteropTypes;
            public List<string> Aliases;

            public AssemblyInfo(AssemblyName assemblyName, bool isModule, bool embedInteropTypes, List<string> aliases)
            {
                AssemblyName = assemblyName;
                IsModule = isModule;
                EmbedInteropTypes = embedInteropTypes;
                Aliases = aliases;
            }
        }

        byte[] LoadFile(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var imageToLoad = new byte[file.Length];
                file.Read(imageToLoad, 0, imageToLoad.Length);
                return imageToLoad;
            }
        }

        internal Assembly Build(string sourceCode, Dictionary<string, Tuple<AssemblyName, string>> usedAssemblies, IEnumerable<ImportAssemblyAttribute> otherAssemblies, bool csharpAssemblyRequired, string assemblyName, out byte[] image)
        {
            Dictionary<string, AssemblyInfo> allAssemblies = new Dictionary<string, AssemblyInfo>();

            foreach (var item in usedAssemblies)
            {
                if (item.Value.Item2 == null)
                    allAssemblies.Add(item.Key, new AssemblyInfo(item.Value.Item1, false, false, new List<string>()));
                else
                    allAssemblies.Add(item.Key, new AssemblyInfo(item.Value.Item1, false, false, new List<string>() { item.Value.Item2 }));
            }

            if (csharpAssemblyRequired)
            {
#if netcore
                var csharpAssemblyName = tpa.Value.CSharpAssemblyName;
                allAssemblies.Add(csharpAssemblyName.FullName, new AssemblyInfo(csharpAssemblyName, false, false, new List<string>()));
                //var runtimeAssemblyName = tpa.Value.RuntimeAssemblyName;
                //allAssemblies.Add(runtimeAssemblyName.FullName, new AssemblyInfo(runtimeAssemblyName, false, false, new List<string>()));
                //var collectionsAssemblyName = tpa.Value.CollectionsAssemblyName;
                //allAssemblies.Add(collectionsAssemblyName.FullName, new AssemblyInfo(collectionsAssemblyName, false, false, new List<string>()));
                var linqExpressionsAssemblyName = tpa.Value.LinqExpressionsAssemblyName;
                allAssemblies.Add(linqExpressionsAssemblyName.FullName, new AssemblyInfo(linqExpressionsAssemblyName, false, false, new List<string>()));
#else
                var dynamicAssembly = typeof(Microsoft.CSharp.RuntimeBinder.Binder).GetTypeInfo().Assembly;
                if (!allAssemblies.ContainsKey(dynamicAssembly.FullName))
                    allAssemblies.Add(dynamicAssembly.FullName, new AssemblyInfo(dynamicAssembly.GetName(), false, false, new List<string>()));
#endif
            }

            foreach (var otherAssembly in otherAssemblies)
            {
                AssemblyName assemblyNameToLoad = new AssemblyName(otherAssembly.AssemblyFullName);

                if (allAssemblies.TryGetValue(assemblyNameToLoad.FullName, out var matchedAssmembly))
                {
                    if (otherAssembly.EmbedInteropTypes)
                        matchedAssmembly.EmbedInteropTypes = true;
                    if (otherAssembly.Aliases != null)
                    {
                        foreach (var alias in otherAssembly.Aliases)
                        {
                            if (!matchedAssmembly.Aliases.Contains(alias))
                                matchedAssmembly.Aliases.Add(alias);
                        }
                    }
                }
                else
                {
                    List<string> aliases = new List<string>();
                    if (otherAssembly.Aliases != null && otherAssembly.Aliases.Length > 0)
                        aliases.AddRange(otherAssembly.Aliases);
                    matchedAssmembly = new AssemblyInfo(assemblyNameToLoad, otherAssembly.IsModule, otherAssembly.EmbedInteropTypes, aliases);
                    allAssemblies.Add(assemblyNameToLoad.FullName, matchedAssmembly);
                }

                if (otherAssembly.Preloading && !builder.CheckExistedInAssemblyCache(otherAssembly.AssemblyFullName))
                {
                    AssemblyRequestingEventArgs eventArgs = new AssemblyRequestingEventArgs(assemblyNameToLoad, matchedAssmembly.IsModule, matchedAssmembly.EmbedInteropTypes, matchedAssmembly.Aliases);
                    raiseMissingAssemblyRequestingCallback(eventArgs);
                    if (eventArgs.IsLoaded)
                    {
                        builder.LoadIntoAssemblyCache(otherAssembly.AssemblyFullName, eventArgs.MissingAssemblyImage);
                    }
                }
            }

            List<CSharpRoslynAgency.AssemblyReference> assemblyReferences =
                allAssemblies.Values.Select(i =>
                {
                    if (i.Aliases.Count == 0)
                        return new CSharpRoslynAgency.AssemblyReference(i.AssemblyName, i.IsModule, i.EmbedInteropTypes);
                    else
                        return new CSharpRoslynAgency.AssemblyReference(i.AssemblyName, i.IsModule, i.EmbedInteropTypes, i.Aliases.ToArray());
                }).ToList();

            if (!builder.Build(assemblyName, new string[] { sourceCode }, assemblyReferences, out image, out var errors))
            {
                var records = errors.Select(i => new TypeCreatingExceptionRecord(i.Id, i.Message)).ToArray();
                throw new TypeCreatingException(records);
            }
            else
            {
                Assembly assembly;
#if netcore
                using (var ms = new MemoryStream(image))
                {
                    assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
                }
#else
                assembly = Assembly.Load(image);
#endif
                return assembly;
            }
        }
    }
}