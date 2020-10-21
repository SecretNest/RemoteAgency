using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

namespace SecretNest.RemoteAgency
{
	static class PlainEntityBodyBuilder
	{
		internal static Type BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding)
		{
			typeBuilder.GenerateExplicitImplementation(typeof(IRemoteAgencyMessage).GetTypeInfo());
			return typeBuilder;
		}

		private static void GenerateExplicitImplementation(this TypeBuilder typeBuilder, TypeInfo interfaceInfo)
		{
			// Add Interface impl
			typeBuilder.AddInterfaceImplementation(interfaceInfo);

			foreach (var p in interfaceInfo.DeclaredProperties)
			{
				typeBuilder.GenerateExplicitImplementationProperty(p);
			}
		}

		private static void GenerateExplicitImplementationProperty(this TypeBuilder typeBuilder, PropertyInfo propertyInfo)
		{
			var field = typeBuilder.GeneratePropertyBackField(propertyInfo);

			var propertyBuilder = typeBuilder.DefineProperty($"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}",
				PropertyAttributes.HasDefault, propertyInfo.PropertyType, null);

			typeBuilder.GenerateGetMethodForField(propertyInfo, propertyBuilder, field);
			typeBuilder.GenerateSetMethodForField(propertyInfo, propertyBuilder, field);
		}

		private static void GenerateSetMethodForField(this TypeBuilder typeBuilder, PropertyInfo propertyInfo,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			var setMethod = typeBuilder.DefineMethod($"{propertyInfo.DeclaringType.Name}.{propertyBuilder.SetMethod.Name}",
				MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Final |
				MethodAttributes.Virtual | MethodAttributes.SpecialName, null, new[] { propertyBuilder.PropertyType });

			setMethod.GenerateSetBodyForProperty(field);
			typeBuilder.DefineMethodOverride(setMethod, propertyInfo.SetMethod);

			propertyBuilder.SetSetMethod(setMethod);
		}

		private static void GenerateGetMethodForField(this TypeBuilder typeBuilder, PropertyInfo propertyInfo,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			var getMethod = typeBuilder.DefineMethod($"{propertyInfo.DeclaringType.Name}.{propertyBuilder.GetMethod.Name}",
				MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Final |
				MethodAttributes.Virtual | MethodAttributes.SpecialName, propertyBuilder.PropertyType, null);

			getMethod.GenerateGetBodyForProperty(field);
			typeBuilder.DefineMethodOverride(getMethod, propertyInfo.GetMethod);

			propertyBuilder.SetGetMethod(getMethod);
		}

		private static FieldInfo GeneratePropertyBackField(this TypeBuilder builder, PropertyInfo propertyInfo)
		{
			var fieldName = $"{propertyInfo.Name}_$BackField$";

			return builder.DefineField(fieldName, propertyInfo.PropertyType,
				FieldAttributes.Private | FieldAttributes.HasDefault);
		}

		private static void GenerateGetBodyForProperty(this MethodBuilder getMethod,
			FieldInfo backField)
		{
			var g = getMethod.GetILGenerator();
			g.Emit(OpCodes.Ldarg_0); // this
			g.Emit(OpCodes.Ldfld, backField);
			g.Emit(OpCodes.Ret);
		}

		private static void GenerateSetBodyForProperty(this MethodBuilder setMethod,
			FieldInfo backField)
		{
			var g = setMethod.GetILGenerator();
			g.Emit(OpCodes.Ldarg_0); // this
			g.Emit(OpCodes.Ldarg_1); // value
			g.Emit(OpCodes.Stfld, backField); // set value
			g.Emit(OpCodes.Ret);
		}
	}
}
