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
		internal static void BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding)
		{
			typeBuilder.GenerateExplicitImplementation(typeof(IRemoteAgencyMessage).GetTypeInfo());

			foreach (var p in entityBuilding.Properties)
			{
				typeBuilder.GenerateAutoImplementedProperty(p.Name, p.Type);
			}
		}

		/// <summary>
		/// 为类型实现一个自动实现的简单属性。
		/// </summary>
		/// <param name="typeBuilder">类型。</param>
		/// <param name="name">属性名称。</param>
		/// <param name="propertyType">属性的值的类型。</param>
		private static void GenerateAutoImplementedProperty(this TypeBuilder typeBuilder, string name,
			Type propertyType)
		{
			var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, propertyType, null);
			var backField = typeBuilder.GeneratePropertyBackField(propertyBuilder);

			typeBuilder.GenerateSetMethodCore($"set_{name}", propertyBuilder, backField);
			typeBuilder.GenerateGetMethodCore($"get_{name}", propertyBuilder, backField);

		}

		/// <summary>
		/// 为类型隐式实现接口。
		/// </summary>
		/// <param name="typeBuilder">类型。</param>
		/// <param name="interfaceInfo">要隐式实现的接口。</param>
		private static void GenerateExplicitImplementation(this TypeBuilder typeBuilder, TypeInfo interfaceInfo)
		{
			// 声明类型实现接口
			typeBuilder.AddInterfaceImplementation(interfaceInfo);

			// 实现所有属性
			foreach (var p in interfaceInfo.DeclaredProperties)
			{
				typeBuilder.GenerateExplicitImplementationProperty(p);
			}
		}

		/// <summary>
		/// 为类型实现某个属性的隐式接口实现。
		/// </summary>
		/// <param name="typeBuilder">类型对象。</param>
		/// <param name="propertyInfo">需要隐式实现的属性。</param>
		private static void GenerateExplicitImplementationProperty(this TypeBuilder typeBuilder, PropertyInfo propertyInfo)
		{
			var field = typeBuilder.GeneratePropertyBackField(propertyInfo);

			var propertyBuilder = typeBuilder.DefineProperty($"{propertyInfo.DeclaringType.Name}.{propertyInfo.Name}",
				PropertyAttributes.HasDefault, propertyInfo.PropertyType, null);

			typeBuilder.GenerateInterfaceImpGetMethodForField(propertyInfo, propertyBuilder, field);
			typeBuilder.GenerateInterfaceImpSetMethodForField(propertyInfo, propertyBuilder, field);
		}

		/// <summary>
		/// 为类型创建属性的自动 set 方法。
		/// </summary>
		/// <param name="typeBuilder">类型。</param>
		/// <param name="methodName">set 方法的内部名称。</param>
		/// <param name="propertyBuilder">该方法关联的属性。</param>
		/// <param name="field">该方法使用的内部字段。</param>
		/// <returns>创建的 set 方法。</returns>
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
		/// 为类型创建属性的自动 get 方法。
		/// </summary>
		/// <param name="typeBuilder">类型。</param>
		/// <param name="methodName">set 方法的内部名称。</param>
		/// <param name="propertyBuilder">该方法关联的属性。</param>
		/// <param name="field">该方法使用的内部字段。</param>
		/// <returns>创建的 get 方法。</returns>
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
		/// 为类型实现隐式接口实现的属性对应的 set 方法。
		/// </summary>
		/// <param name="typeBuilder">类型。</param>
		/// <param name="propertyInfo">接口中定义的属性。</param>
		/// <param name="propertyBuilder">类型定义的属性。</param>
		/// <param name="field">属性对应的内部字段。</param>
		private static void GenerateInterfaceImpSetMethodForField(this TypeBuilder typeBuilder, PropertyInfo propertyInfo,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			// 方法名：{接口名称}.{set_属性名}
			var methodName = $"{propertyInfo.DeclaringType.FullName}.{propertyInfo.SetMethod.Name}";
			var setMethod = typeBuilder.GenerateSetMethodCore(methodName, propertyBuilder, field);

			typeBuilder.DefineMethodOverride(setMethod, propertyInfo.SetMethod);

		}

		/// <summary>
		/// 为类型实现隐式接口实现的属性对应的 get 方法。
		/// </summary>
		/// <param name="typeBuilder">类型。</param>
		/// <param name="propertyInfo">接口中定义的属性。</param>
		/// <param name="propertyBuilder">类型定义的属性。</param>
		/// <param name="field">属性对应的内部字段。</param>
		private static void GenerateInterfaceImpGetMethodForField(this TypeBuilder typeBuilder, PropertyInfo propertyInfo,
			PropertyBuilder propertyBuilder, FieldInfo field)
		{
			// 方法名：{接口名称}.{set_属性名}
			var methodName = $"{propertyInfo.DeclaringType.FullName}.{propertyInfo.GetMethod.Name}";
			var getMethod = typeBuilder.GenerateGetMethodCore(methodName, propertyBuilder, field);

			typeBuilder.DefineMethodOverride(getMethod, propertyInfo.GetMethod);
		}

		/// <summary>
		/// 生成属性对应的内部字段对象。
		/// </summary>
		/// <param name="builder">类型。</param>
		/// <param name="propertyInfo">要生成内部字段的属性。</param>
		/// <returns>属性对应的内部字段对象。</returns>
		private static FieldInfo GeneratePropertyBackField(this TypeBuilder builder, PropertyInfo propertyInfo)
		{
			var fieldName = $"{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}_$BackField$";

			return builder.DefineField(fieldName, propertyInfo.PropertyType,
				FieldAttributes.Private | FieldAttributes.HasDefault);
		}

		/// <summary>
		/// 生成简单属性的 get 方法体。
		/// </summary>
		/// <param name="getMethod"></param>
		/// <param name="backField"></param>
		private static void GenerateGetBodyForProperty(this MethodBuilder getMethod,
			FieldInfo backField)
		{
			var g = getMethod.GetILGenerator();
			g.Emit(OpCodes.Ldarg_0); // this
			g.Emit(OpCodes.Ldfld, backField);
			g.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// 生成简单属性的 set 方法体。
		/// </summary>
		/// <param name="setMethod"></param>
		/// <param name="backField"></param>
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
