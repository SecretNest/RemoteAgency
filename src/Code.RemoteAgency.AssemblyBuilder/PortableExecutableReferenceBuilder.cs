using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using SecretNest.RemoteAgency.TypeBuilding;

namespace SecretNest.RemoteAgency
{
    static class PortableExecutableReferenceBuilder
    {
        internal static PortableExecutableReference BuildPortableExecutableReference(this AssemblyReference assemblyReference, byte[] image)
        {
            if (assemblyReference == null) return null;
            MetadataReferenceProperties properties = new MetadataReferenceProperties(
                assemblyReference.IsModule ? MetadataImageKind.Module : MetadataImageKind.Assembly,
                assemblyReference.Aliases?.ToImmutableArray() ?? default,
                assemblyReference.EmbedInteropTypes);

            PortableExecutableReference reference = MetadataReference.CreateFromImage(image.ToImmutableArray(), properties);

            return reference;
        }
    }
}
