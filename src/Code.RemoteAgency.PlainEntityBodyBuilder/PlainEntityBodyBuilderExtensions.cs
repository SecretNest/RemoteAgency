using System.Reflection;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency
{
	static class PlainEntityBodyBuilderExtensions
	{
		internal static void BuildEntity(this TypeBuilder typeBuilder, EntityBuilding entityBuilding)
		{
			typeBuilder.GenerateExplicitImplementationForProperties(typeof(IRemoteAgencyMessage).GetTypeInfo());

			foreach (var p in entityBuilding.Properties)
			{
				typeBuilder.GenerateAutoImplementedProperty(p.Name, p.Type);
			}
		}
	}
}
