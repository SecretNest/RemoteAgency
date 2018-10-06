using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using System.Reflection;

namespace SecretNest.RemoteAgency
{
    class MetadataReferenceResolver : Microsoft.CodeAnalysis.MetadataReferenceResolver
    {
        public override bool ResolveMissingAssemblies => true;

        public override bool Equals(object other)
        {
            return other.Equals(this);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override ImmutableArray<PortableExecutableReference> ResolveReference(string reference, string baseFilePath, MetadataReferenceProperties properties)
        {
            throw new NotImplementedException();
        }

        public override PortableExecutableReference ResolveMissingAssembly(MetadataReference definition, AssemblyIdentity referenceIdentity)
        {
            AssemblyName name = new AssemblyName(referenceIdentity.Name)
            {
                ContentType = referenceIdentity.ContentType,
                CultureName = referenceIdentity.CultureName,
                Flags = referenceIdentity.Flags,
                Version = referenceIdentity.Version
            };
            Assembly assembly = Assembly.Load(name);
            string location = assembly.Location;
            return MetadataReference.CreateFromFile(location);
        }
    }
}
