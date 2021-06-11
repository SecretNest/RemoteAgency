using System.Collections.Generic;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency
{
    static partial class AssemblyBuildingEmitExtensions
    {
        internal static void EmitAttributePassThroughAttributes(this GenericTypeParameterBuilder typeBuilder, IEnumerable<CustomAttributeBuilder> passThroughAttributes)
        {
            foreach (var customAttribute in passThroughAttributes)
            {
                typeBuilder.SetCustomAttribute(customAttribute);
            }
        }

        internal static void EmitAttributePassThroughAttributes(this TypeBuilder typeBuilder, IEnumerable<CustomAttributeBuilder> passThroughAttributes)
        {
            foreach (var customAttribute in passThroughAttributes)
            {
                typeBuilder.SetCustomAttribute(customAttribute);
            }
        }

        //ReSharper disable once UnusedMember.Local
        internal static void EmitParameterPassThroughAttributes(this ParameterBuilder typeBuilder, IEnumerable<CustomAttributeBuilder> passThroughAttributes)
        {
            foreach (var customAttribute in passThroughAttributes)
            {
                typeBuilder.SetCustomAttribute(customAttribute);
            }
        }
    }
}
