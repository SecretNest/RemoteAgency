using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency
{
	static class PlainEntityBodyBuilder
	{
		internal static void BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding)
		{
			typeBuilder.GenerateExplicitImplementation(typeof(IRemoteAgencyMessage).GetTypeInfo());

			foreach (var p in entityBuilding.Properties)
			{
				typeBuilder.GenerateAutoImplementedProperty(p.Name, p.Type);
			}
		}

		/// <summary>
		/// Generates a property with auto implemented Get/Set support.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="name">The name of the property.</param>
		/// <param name="propertyType">The value type of the property.</param>
		private static void GenerateAutoImplementedProperty(this TypeBuilder typeBuilder, string name,
			Type propertyType)
		{
			var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, propertyType, null);
			var backField = typeBuilder.GeneratePropertyBackField(propertyBuilder);

			typeBuilder.GenerateSetMethodCore($"set_{name}", propertyBuilder, backField);
			typeBuilder.GenerateGetMethodCore($"get_{name}", propertyBuilder, backField);

		}

		/// <summary>
		/// Explicitly implements an interface for a type.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="interfaceInfo">The interface which should be explicitly implemented.</param>
		private static void GenerateExplicitImplementation(this TypeBuilder typeBuilder, TypeInfo interfaceInfo)
		{
			// Declare that the type should implement this interface
			typeBuilder.AddInterfaceImplementation(interfaceInfo);

			// Implement all properties
			foreach (var p in interfaceInfo.DeclaredProperties)
			{
				typeBuilder.GenerateExplicitImplementationProperty(p);
			}
		}

		/// <summary>
		/// Explicitly implements a property from a interface for a type.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="propertyInfo">The property should be explicitly implemented.</param>
		private static void GenerateExplicitImplementationProperty(this TypeBuilder typeBuilder, PropertyInfo propertyInfo)
		{
			// Generate back field
			var field = typeBuilder.GeneratePropertyBackField(propertyInfo);

			// Define property
			var propertyBuilder = typeBuilder.DefineProperty($"{propertyInfo.DeclaringType!.Name}.{propertyInfo.Name}",
				PropertyAttributes.HasDefault, propertyInfo.PropertyType, null);

			// Generate property getter and setter
			typeBuilder.GenerateInterfaceImpGetMethodForField(propertyInfo, propertyBuilder, field);
			typeBuilder.GenerateInterfaceImpSetMethodForField(propertyInfo, propertyBuilder, field);
		}

		/// <summary>
		/// Generates a auto-implemented SET method for a property.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="methodName">The name of the internal method.</param>
		/// <param name="propertyBuilder">The property which the method is related to.</param>
		/// <param name="field">The field which the method should access to.</param>
		/// <returns>The generated SET method.</returns>
		private static MethodBuilder GenerateSetMethodCore(this TypeBuilder typeBuilder, string methodName,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			var setMethod = typeBuilder.DefineMethod(methodName,
				MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
				MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.SpecialName, null,
				new[] { propertyBuilder.PropertyType });

			setMethod.GenerateSetBodyForProperty(field);
			propertyBuilder.SetSetMethod(setMethod);

			return setMethod;
		}

		/// <summary>
		/// Generates a auto-implemented GET method for a property.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="methodName">The name of the internal method.</param>
		/// <param name="propertyBuilder">The property which the method is related to.</param>
		/// <param name="field">The field which the method should access to.</param>
		/// <returns>The generated GET method.</returns>
		private static MethodBuilder GenerateGetMethodCore(this TypeBuilder typeBuilder, string methodName,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			var getMethod = typeBuilder.DefineMethod(methodName,
				MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Final |
				MethodAttributes.Virtual | MethodAttributes.SpecialName, propertyBuilder.PropertyType, null);

			getMethod.GenerateGetBodyForProperty(field);
			propertyBuilder.SetGetMethod(getMethod);

			return getMethod;
		}

		/// <summary>
		/// Implements the SET method for an explicitly implemented interface property.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="propertyInfo">The property which should be explicitly implemented.</param>
		/// <param name="propertyBuilder">The new property which is used to implement the <paramref name="propertyInfo"/>.</param>
		/// <param name="field">The backend field related with the new property.</param>
		private static void GenerateInterfaceImpSetMethodForField(this TypeBuilder typeBuilder, PropertyInfo propertyInfo,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			// Method name: {Interface Name}.set_{Property Name}
			var methodName = $"{propertyInfo.DeclaringType!.FullName}.{propertyInfo.SetMethod!.Name}";
			var setMethod = typeBuilder.GenerateSetMethodCore(methodName, propertyBuilder, field);

			typeBuilder.DefineMethodOverride(setMethod, propertyInfo.SetMethod);

		}

		/// <summary>
		/// Implements the GET method for an explicitly implemented interface property.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="propertyInfo">The property which should be explicitly implemented.</param>
		/// <param name="propertyBuilder">The new property which is used to implement the <paramref name="propertyInfo"/>.</param>
		/// <param name="field">The backend field related with the new property.</param>
		private static void GenerateInterfaceImpGetMethodForField(this TypeBuilder typeBuilder, PropertyInfo propertyInfo,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			// Method name: {Interface Name}.get_{Property Name}
			var methodName = $"{propertyInfo.DeclaringType!.FullName}.{propertyInfo.GetMethod!.Name}";
			var getMethod = typeBuilder.GenerateGetMethodCore(methodName, propertyBuilder, field);

			typeBuilder.DefineMethodOverride(getMethod, propertyInfo.GetMethod);
		}

		/// <summary>
		/// Generates the backend field for an auto implemented property.
		/// </summary>
		/// <param name="builder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="propertyInfo">The property which need to generated the backend field.</param>
		/// <returns>The generated backend field associated with <paramref name="propertyInfo"/>.</returns>
		private static FieldInfo GeneratePropertyBackField(this TypeBuilder builder, PropertyInfo propertyInfo)
		{
			var fieldName = $"{propertyInfo.DeclaringType!.FullName}.{propertyInfo.Name}_$BackField$";

			return builder.DefineField(fieldName, propertyInfo.PropertyType,
				FieldAttributes.Private | FieldAttributes.HasDefault);
		}

		/// <summary>
		/// Generates the method body for a simple GET method.
		/// </summary>
		/// <param name="getMethod">The <see cref="MethodBuilder"/> instance.</param>
		/// <param name="backField">The backend field related with the <paramref name="getMethod"/>.</param>
		private static void GenerateGetBodyForProperty(this MethodBuilder getMethod,
			FieldInfo backField)
		{
			var g = getMethod.GetILGenerator();
			g.Emit(OpCodes.Ldarg_0); // this
			g.Emit(OpCodes.Ldfld, backField);
			g.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Generates the method body for a simple SET method.
		/// </summary>
		/// <param name="setMethod">The <see cref="MethodBuilder"/> instance.</param>
		/// <param name="backField">The backend field related with the <paramref name="setMethod"/>.</param>
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
