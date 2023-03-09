using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SecretNest.RemoteAgency
{
	/// <summary>
	/// Provide extension methods of Emit. This class is static.
	/// </summary>
	internal static class EmitExtensions
	{
		#region Attribute Applier

		public static void ApplyCustomAttributes(this MethodBuilder methodBuilder,
			IEnumerable<CustomAttributeBuilder> attributeBuilders)
		{
			foreach (var attributeBuilder in attributeBuilders)
			{
				methodBuilder.SetCustomAttribute(attributeBuilder);
			}
		}


		public static void ApplyCustomAttributes(this PropertyBuilder propertyBuilder,
			IEnumerable<CustomAttributeBuilder> attributeBuilders)
		{
			foreach (var attributeBuilder in attributeBuilders)
			{
				propertyBuilder.SetCustomAttribute(attributeBuilder);
			}
		}


		public static void ApplyCustomAttributes(this ParameterBuilder parameterBuilder,
			IEnumerable<CustomAttributeBuilder> attributeBuilders)
		{
			foreach (var attributeBuilder in attributeBuilders)
			{
				parameterBuilder.SetCustomAttribute(attributeBuilder);
			}
		}


		public static void ApplyCustomAttributes(this TypeBuilder typeBuilder,
			IEnumerable<CustomAttributeBuilder> attributeBuilders)
		{
			foreach (var attributeBuilder in attributeBuilders)
			{
				typeBuilder.SetCustomAttribute(attributeBuilder);
			}
		}


		public static void ApplyCustomAttributes(this FieldBuilder fieldBuilder,
			IEnumerable<CustomAttributeBuilder> attributeBuilders)
		{
			foreach (var attributeBuilder in attributeBuilders)
			{
				fieldBuilder.SetCustomAttribute(attributeBuilder);
			}
		}


		#endregion

		/// <summary>
		/// Generates a property with auto implemented Get/Set support.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="name">The name of the property.</param>
		/// <param name="propertyType">The value type of the property.</param>
		public static void GenerateAutoImplementedProperty(this TypeBuilder typeBuilder, string name,
			Type propertyType)
		{
			var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, propertyType, null);
			var backField = typeBuilder.GeneratePropertyBackField(propertyBuilder);

			typeBuilder.GenerateSetMethodCore($"set_{name}", propertyBuilder, backField);
			typeBuilder.GenerateGetMethodCore($"get_{name}", propertyBuilder, backField);

		}

		/// <summary>
		/// Explicitly implements all properties of an interface for a type.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="interfaceInfo">The interface which should be explicitly implemented.</param>
		public static void GenerateExplicitImplementationForProperties(this TypeBuilder typeBuilder, Type interfaceInfo)
		{
			// Declare that the type should implement this interface
			typeBuilder.AddInterfaceImplementation(interfaceInfo);

			// Implement all properties
			foreach (var p in interfaceInfo.GetTypeInfo().DeclaredProperties)
			{
				typeBuilder.GenerateExplicitImplementationProperty(p);
			}
		}

		/// <summary>
		/// Explicitly implements a property from a interface for a type.
		/// </summary>
		/// <param name="typeBuilder">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="propertyInfo">The property should be explicitly implemented.</param>
		public static void GenerateExplicitImplementationProperty(this TypeBuilder typeBuilder,
			PropertyInfo propertyInfo)
		{
			// Generate back field
			var field = typeBuilder.GeneratePropertyBackField(propertyInfo);

			// Define property
			var propertyBuilder = typeBuilder.DefineProperty(
				$"{propertyInfo.DeclaringType!.FullName}.{propertyInfo.Name}",
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
				MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
				MethodAttributes.Final |
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
		private static void GenerateInterfaceImpSetMethodForField(this TypeBuilder typeBuilder,
			PropertyInfo propertyInfo,
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
		private static void GenerateInterfaceImpGetMethodForField(this TypeBuilder typeBuilder,
			PropertyInfo propertyInfo,
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
		/// Simpely end the method code with a <see cref="OpCodes.Ret"/> instruction.
		/// </summary>
		/// <param name="method"></param>
		public static void End(this MethodBuilder method)
		{
			var g = method.GetILGenerator();
			g.Emit(OpCodes.Ret);
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

		public static void SetOutParameterDefaultValue(this MethodBuilder method, ParameterInfo parameter)
		{
			var g = method.GetILGenerator();
			
			// Default value
			var local = g.DeclareLocal(parameter.ParameterType);

			g.Emit(OpCodes.Ldloca, local);
			g.Emit(OpCodes.Initobj);
			g.Emit(OpCodes.Localloc, local);
			g.Emit(OpCodes.Starg, parameter.Position - 1); // Push local to arg N
		}

		/// <summary>
		/// Generate a default(T) as return value for <paramref name="method"/>.
		/// </summary>
		/// <param name="method">The <see cref="MethodBuilder"/> instance.</param>
		/// <param name="type">The type for the default value.</param>
		public static void GenerateDefaultValue(this MethodBuilder method, Type type)
		{
			var g = method.GetILGenerator();
			var local = g.DeclareLocal(type);

			g.Emit(OpCodes.Ldloca, local);
			g.Emit(OpCodes.Initobj, type);
			g.Emit(OpCodes.Ldloc, local);
			g.Emit(OpCodes.Ret);
		}

		public static void ReturnTaskOfT(this MethodBuilder method, Type taskInnerType)
		{
			var g = method.GetILGenerator();
			var local = g.DeclareLocal(taskInnerType);

			var fromResultMethod =
				typeof(Task).GetMethod(nameof(Task.FromResult), BindingFlags.Public | BindingFlags.Static);

			g.Emit(OpCodes.Ldtoken, typeof(Task));
			g.Emit(OpCodes.Ldloca, local);
			g.Emit(OpCodes.Initobj);
			g.Emit(OpCodes.Ldloc, local);
			g.EmitCall(OpCodes.Call, fromResultMethod, new[] {taskInnerType});
			g.Emit(OpCodes.Ret);
		}

		public static void ReturnValueTaskOfT(this MethodBuilder method, Type taskInnerType)
		{
			#if netfx
            throw new NotSupportedException("ValueTask is not supported by .NET framework.");
			#else
			var realType = typeof(ValueTask<>).MakeGenericType(taskInnerType);
          	method.GenerateDefaultValue(realType);
            #endif
		}

		public static void GenerateException(this MethodBuilder method, Type exceptionType)
		{
			var g = method.GetILGenerator();

			var constructor = exceptionType.GetConstructor(Array.Empty<Type>());
			g.Emit(OpCodes.Newobj, constructor);
			g.Emit(OpCodes.Throw);
		}

		public static void ReturnStaticValue(this MethodBuilder method, Type containerType, string propertyName)
		{
			var propertyInfo = containerType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public);

			if (propertyInfo == null)
			{
				throw new InvalidOperationException(
					$"Cannot find static property {propertyName} from type {containerType.FullName}");
			}

			var g = method.GetILGenerator();
			g.Emit(OpCodes.Ldtoken, containerType);
			g.Emit(OpCodes.Call, propertyInfo.GetMethod);
			g.Emit(OpCodes.Ret);
		}

		public static void GenerateException<T>(this MethodBuilder method)
			where T : Exception
				=> method.GenerateException(typeof(T));
	}
}