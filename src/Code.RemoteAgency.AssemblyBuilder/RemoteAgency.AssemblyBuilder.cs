using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using SecretNest.RemoteAgency.TypeBuilding;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        
#if !net461
        static Lazy<TrustedPlatformAssemblies> _tpa = new Lazy<TrustedPlatformAssemblies>(false);
#endif

        void InitializeAssemblyBuilder()
        {
            _metadataReferenceResolver = new Lazy<MetadataReferenceResolver>(() => new MetadataReferenceResolver() {GetMissingAssemblyCallback = GetMissingAssembly});
        }

        private Lazy<MetadataReferenceResolver> _metadataReferenceResolver; 

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
                syntaxTrees,
                metadataReferences,
                new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    metadataReferenceResolver: _metadataReferenceResolver.Value));
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
                    var errors = failures.Select(i => new TypeCreatingExceptionRecord(i.Id, i.GetMessage())).ToList();
                    buildingError = new TypeCreatingException(errors);
                    assemblyImage = null;
                    return false;
                }
                else
                {
                    //ms.Seek(0, SeekOrigin.Begin);
                    assemblyImage = ms.ToArray();
                    buildingError = null;
                    return true;
                }
            }
        }

        /// <summary>
        /// Occurs when a missing assembly / module needs to be resolved. Property MissingAssemblyImage in parameter e should be set before returning if the assembly / module is resolved.
        /// </summary>
        /// <remarks>This event is not present in Neat release.</remarks>
        public event EventHandler<MissingAssemblyResolvingEventArgs> MissingAssemblyResolving;

        /// <summary>
        /// Occurs when a missing assembly / module needs to be resolved before querying from cache. Property MissingAssemblyImage in parameter e should be set before returning if the assembly / module is resolved.
        /// </summary>
        /// <remarks>This event is not present in Neat release.</remarks>
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

            if (!assemblyReference.IsModule)
            {
#if !net461
                //process with trusted platform
                image = _tpa.Value.LoadAssembly(assemblyReference.Name);
#else
                //process with file loading
                var assembly = Assembly.Load(assemblyReference.Name);
                var filename = assembly.Location;
                if (!string.IsNullOrEmpty(filename))
                {
                    image = File.ReadAllBytes(filename);
                }
#endif
                if (image != null)
                {
                    _assemblyImageCache[fullName] = image;
                    var result = assemblyReference.BuildPortableExecutableReference(image);
                    return result;
                }
            }

            //process with MissingAssemblyRequesting
            return GetFromEvent(MissingAssemblyResolving, display, fullName, assemblyReference);
        }

        readonly Dictionary<string, byte[]> _assemblyImageCache = new Dictionary<string, byte[]>();

        /// <summary>
        /// Clears the assembly cache.
        /// </summary>
        /// <remarks>This method is not present in Neat release.</remarks>
        public void ClearAssemblyCache()
        {
#if !net461
            _tpa = new Lazy<TrustedPlatformAssemblies>(false);
#endif
            //metadataReferenceCache.Clear();
            _assemblyImageCache.Clear();
        }

        /// <summary>
        /// Loads image into assembly cache.
        /// </summary>
        /// <param name="fullName">Full name of the assembly / module</param>
        /// <param name="image">PE format image of the assembly / module</param>
        /// <remarks>This method is not present in Neat release.</remarks>
        public void LoadIntoAssemblyCache(string fullName, byte[] image)
        {
            _assemblyImageCache[fullName] = image;
        }

        /// <summary>
        /// Returns names of all cached assemblies and modules.
        /// </summary>
        /// <returns>Names of all cached assemblies and modules</returns>
        /// <remarks>This method is not present in Neat release.</remarks>
        public IEnumerable<string> GetAllCachedAssemblyNames()
        {
            return _assemblyImageCache.Keys;
        }

        /// <summary>
        /// Removes an cached assembly / module.
        /// </summary>
        /// <param name="fullName">Full name of the assembly / module</param>
        /// <returns>Whether the element is successfully found and removed.</returns>
        /// <remarks>This method is not present in Neat release.</remarks>
        public bool RemoveFromAssemblyCache(string fullName)
        {
            return _assemblyImageCache.Remove(fullName);
        }

        /// <summary>
        /// Checks whether the assembly / module specified is cached.
        /// </summary>
        /// <param name="fullName">Full name of the assembly / module</param>
        /// <returns>Whether the assembly / module specified is cached.</returns>
        /// <remarks>This method is not present in Neat release.</remarks>
        public bool IsPresentInAssemblyCache(string fullName)
        {
            return _assemblyImageCache.ContainsKey(fullName);
        }
    }
}
