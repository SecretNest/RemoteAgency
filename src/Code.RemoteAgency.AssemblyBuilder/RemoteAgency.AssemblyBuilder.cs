using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        void InitializeAssemblyBuilder()
        {
            _metadataReferenceResolver = new MetadataReferenceResolver() { GetMissingAssemblyCallback = GetMissingAssembly };
        }

        bool BuildAssembly(string assemblyName, IEnumerable<string> sourceCode, IEnumerable<AssemblyReference> references, out byte[] assemblyImage, out TypeCreatingException buildingError)
        {
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            foreach (var code in sourceCode)
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(code));
            List<MetadataReference> metadataReferences = new List<MetadataReference>();
            foreach (var name in references)
            {
                var item = GetMissingAssembly(name.Name.FullName, name);

                if (item != null)
                {
                    metadataReferences.Add(item);
                }
            }
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: syntaxTrees,
                references: metadataReferences,
                options: new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    metadataReferenceResolver: _metadataReferenceResolver));
#if DEBUG
            Console.WriteLine("Begin Emit");
#endif
            using (MemoryStream ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    var errors = failures.Select(i => new TypeCreatingExceptionRecord(i.Id, i.GetMessage())).ToArray();
                    buildingError = new TypeCreatingException(errors);
                    assemblyImage = null;
                    return false;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assemblyImage = ms.ToArray();
                    buildingError = null;
                    return true;
                }
            }
        }

        MetadataReferenceResolver _metadataReferenceResolver;

        /// <summary>
        /// Occurs when a missing assembly / module needs to be resolved. Property MissingAssemblyImage in parameter e should be set before returning if the assembly / module is resolved.
        /// </summary>
        public event EventHandler<MissingAssemblyResolvingEventArgs> MissingAssemblyResolving;

        /// <summary>
        /// Occurs when a missing assembly / module needs to be resolved before querying from cache. Property MissingAssemblyImage in parameter e should be set before returning if the assembly / module is resolved.
        /// </summary>
        public event EventHandler<MissingAssemblyResolvingEventArgs> MissingAssemblyResolvingBeforeCache;

        PortableExecutableReference GetFromEvent(EventHandler<MissingAssemblyResolvingEventArgs> handler, string display, string fullName, AssemblyReference assemblyReference)
        {
            if (handler != null)
            {
                var e = new MissingAssemblyResolvingEventArgs(display, assemblyReference);
                handler(this, e);
                if (e.MissingAssemblyImage != null)
                {
                    LoadIntoAssemblyCache(fullName, e.MissingAssemblyImage);
                    var result = assemblyReference.BuildPortableExecutableReference(e.MissingAssemblyImage);
#if DEBUG
                    Console.WriteLine("Got Assembly: " + fullName);
#endif
                    //metadataReferenceCache[fullName] = result;
                    return result;
                }
            }
            return null;
        }

        PortableExecutableReference GetMissingAssembly(string display, AssemblyReference assemblyReference)
        {
            string fullName = assemblyReference.Name.FullName;

            var beforeCache = GetFromEvent(MissingAssemblyResolvingBeforeCache, display, fullName, assemblyReference);
            if (beforeCache != null)
            {
                return beforeCache;
            }

            if (_assemblyImageCache.TryGetValue(fullName, out var image))
            {
                var result = assemblyReference.BuildPortableExecutableReference(image);
                //metadataReferenceCache[fullName] = result;
                return result;
            }

            return GetFromEvent(MissingAssemblyResolving, display, fullName, assemblyReference);
        }

        readonly Dictionary<string, byte[]> _assemblyImageCache = new Dictionary<string, byte[]>();

        /// <summary>
        /// Clears the assembly cache.
        /// </summary>
        public void ClearAssemblyCache()
        {
            //metadataReferenceCache.Clear();
            _assemblyImageCache.Clear();
        }

        /// <summary>
        /// Loads image into assembly cache.
        /// </summary>
        /// <param name="fullName">Full name of the assembly / module</param>
        /// <param name="image">PE format image of the assembly / module</param>
        public void LoadIntoAssemblyCache(string fullName, byte[] image)
        {
            _assemblyImageCache[fullName] = image;
        }

        /// <summary>
        /// Returns names of all cached assemblies and modules.
        /// </summary>
        /// <returns>Names of all cached assemblies and modules</returns>
        public IEnumerable<string> GetAllCachedAssemblyNames()
        {
            return _assemblyImageCache.Keys;
        }

        /// <summary>
        /// Removes an cached assembly / module.
        /// </summary>
        /// <param name="fullName">Full name of the assembly / module</param>
        /// <returns>Whether the element is successfully found and removed.</returns>
        public bool RemoveFromAssemblyCache(string fullName)
        {
            return _assemblyImageCache.Remove(fullName);
        }

        /// <summary>
        /// Checks whether the assembly / module specified is cached.
        /// </summary>
        /// <param name="fullName">Full name of the assembly / module</param>
        /// <returns>Whether the assembly / module specified is cached.</returns>
        public bool IsPresentInAssemblyCache(string fullName)
        {
            return _assemblyImageCache.ContainsKey(fullName);
        }
    }
}
