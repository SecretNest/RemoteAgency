using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

using SecretNest.RemoteAgency.Attributes;
using SecretNest.RemoteAgency.Injection.EventHelper;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
	internal partial class AssemblyBuildingEmitter
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter
		internal void EmitProxy(
			//ReSharper disable once UnusedParameter.Local
			TypeBuilder proxyTypeBuilder)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression
		{
			//TODO: write code here to build proxy

			//interface info: InterfaceInfo
			ExplicitImplementIProxyCommunicate(proxyTypeBuilder);
			ExplicitImplementUserInterface(proxyTypeBuilder);

			foreach (var propertyInfo in InterfaceInfo.Properties)
			{
				ImplementProperty(proxyTypeBuilder, propertyInfo);
			}

			foreach (var methodInfo in InterfaceInfo.Methods)
			{
				ImplementMethod(proxyTypeBuilder, methodInfo);
			}
		}


		/// <summary>
		/// Implements the user defined <see cref="RemoteAgencyInterfaceInfo.SourceInterface"/> properties from <see cref="InterfaceInfo"/>.
		/// </summary>
		/// <param name="proxyTypeBuilder">The type represents the generated agent type.</param>
		private void ExplicitImplementUserInterface(TypeBuilder proxyTypeBuilder)
		{
			proxyTypeBuilder.GenerateExplicitImplementationForProperties(InterfaceInfo.SourceInterface);
		}

		/// <summary>
		/// Implements the <see cref="IProxyCommunicate"/> interface for the generated type.
		/// </summary>
		/// <param name="proxyTypeBuilder">The <see cref="TypeBuilder"/> represents the generated type.</param>
		private void ExplicitImplementIProxyCommunicate(TypeBuilder proxyTypeBuilder)
		{
			var baseType =
				InterfaceInfo.NeedEventHelper
					? typeof(ProxyBaseWithHelper)
					: typeof(ProxyBaseWithoutHelper);

			// Set base type
			proxyTypeBuilder.SetParent(baseType);
		}

		/// <summary>
		/// Implements a property instance for Remote Agency.
		/// </summary>
		/// <param name="proxyTypeBuilder"></param>
		/// <param name="propertyInfo"></param>
		private void ImplementProperty(TypeBuilder proxyTypeBuilder, RemoteAgencyPropertyInfo propertyInfo)
		{


			var propertyBuilder = proxyTypeBuilder.DefineProperty(propertyInfo.Asset.Name, PropertyAttributes.None,
				propertyInfo.DataType, null);

			propertyBuilder.ApplyCustomAttributes(propertyInfo.AssetLevelPassThroughAttributes);

			MethodBuilder ApplyGetAndSetMethod(string name, RemoteAgencyMethodBodyInfo methodBodyInfo, IEnumerable<CustomAttributeBuilder> methodAttributes, IEnumerable<CustomAttributeBuilder> returnValueAttributes)
			{
				var methodBuilder = proxyTypeBuilder.DefineMethod(
					name,
					MethodAttributes.SpecialName,
					methodBodyInfo.ReturnType,
					methodBodyInfo.Parameters.Select(i => i.ParameterType).ToArray());

				// Method parameters and attributes
				for (var i = 1; i <= methodBodyInfo.Parameters.Length; i++)
				{
					var originalParameterInfo = methodBodyInfo.Parameters[i];

					var p = methodBuilder.DefineParameter(i, ParameterAttributes.None, originalParameterInfo.Name);
					p.ApplyCustomAttributes(propertyInfo.MethodParameterPassThroughAttributes[originalParameterInfo.Name]);
				}

				// Return attributes
				var returnParameter = methodBuilder.DefineParameter(0, ParameterAttributes.None, null);
				returnParameter.ApplyCustomAttributes(returnValueAttributes);

				// Method attribute
				methodBuilder.ApplyCustomAttributes(methodAttributes);

				return methodBuilder;
			}


			if (propertyInfo.IsGettable)
			{
				var methodBuilder = ApplyGetAndSetMethod(
					$"get_{propertyInfo.Asset.Name}",
					propertyInfo.GettingMethodBodyInfo,
					propertyInfo.GettingMethodPassThroughAttributes,
					propertyInfo.GettingMethodReturnValuePassThroughAttributes);

				if (propertyInfo.IsIgnored)
				{
					if (propertyInfo.WillThrowExceptionWhileCalling)
					{
						methodBuilder.GenerateException<IgnoredAssetException>();
					}

					foreach (var returnValueEntityProperty in propertyInfo.GettingMethodBodyInfo.ReturnValueEntityProperties)
					{
						if (returnValueEntityProperty.ReturnValueSource ==
							RemoteAgencyReturnValueSource.ReturnValueDefaultValue)
						{
							// return default(DataType)
							methodBuilder.GenerateDefaultValue(returnValueEntityProperty.DataType);
							break;
						}

						throw new InvalidOperationException(
							$"Ignored Property {propertyInfo.Asset.Name} for type {proxyTypeBuilder.FullName} does not define a default return value.");

					}
				}
				else
				{
					var requestType = ExpandGenericType(propertyInfo.GettingMethodBodyInfo.ParameterEntity);
					var assetName = propertyInfo.AssetName;

					var requestMessage = GenerateRequestCore(methodBuilder, propertyInfo.GettingMethodBodyInfo,
						requestType, assetName);

				}
			}
		}

		/// <summary>
		/// Implements a method instance for Remote Agency.
		/// </summary>
		/// <param name="proxyTypeBuilder"></param>
		/// <param name="methodInfo"></param>
		private void ImplementMethod(TypeBuilder proxyTypeBuilder, RemoteAgencyMethodInfo methodInfo)
		{
			var methodName = methodInfo.Asset.Name;
			var methodBuilder = proxyTypeBuilder.DefineMethod(methodName, MethodAttributes.Public);

			void GenerateReturnValue()
			{
				switch (methodInfo.AsyncMethodOriginalReturnValueDataTypeClass)
				{
					case AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod:
						if (methodInfo.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType != null)
						{
							methodBuilder.GenerateDefaultValue(methodInfo
								.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType);
						}
						else
						{
							methodBuilder.End();
						}

						break;
					case AsyncMethodOriginalReturnValueDataTypeClass.Task:
						methodBuilder.ReturnStaticValue(typeof(Task), nameof(Task.CompletedTask));
						break;
					case AsyncMethodOriginalReturnValueDataTypeClass.ValueTask:
						methodBuilder.GenerateDefaultValue(typeof(ValueTask));
						break;
					case AsyncMethodOriginalReturnValueDataTypeClass.TaskOfType:
						methodBuilder.ReturnTaskOfT(methodInfo.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType);
						break;
					case AsyncMethodOriginalReturnValueDataTypeClass.ValueTaskOfType:
						methodBuilder.ReturnValueTaskOfT(methodInfo
							.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType);
						break;

					default:
						throw new InvalidOperationException(
							$"The {nameof(methodInfo.AsyncMethodOriginalReturnValueDataTypeClass)} of method {methodInfo.Asset.Name} is not supported.");
				}
			}

			// Add generic parameters
			if (methodInfo.IsGenericMethod)
			{
				var genericTypeParameterBuilders =
					methodBuilder.DefineGenericParameters(methodInfo.AssetLevelGenericParameters.Select(i => i.Name)
						.ToArray());

				foreach (var genericTypeParameterBuilder in genericTypeParameterBuilders)
				{
					genericTypeParameterBuilder.EmitAttributePassThroughAttributes(
						methodInfo.AssetLevelGenericParameterPassThroughAttributes[genericTypeParameterBuilder.Name]);
				}
			}

			// Method parameters
			if (methodInfo.MethodBodyInfo.Parameters.Any())
			{
				var position = 1;

				foreach (var parameterInfo in methodInfo.MethodBodyInfo.Parameters)
				{
					var parameterBuilder =
						methodBuilder.DefineParameter(position, parameterInfo.Attributes, parameterInfo.Name);

					parameterBuilder.ApplyCustomAttributes(methodInfo.ParameterPassThroughAttributes[parameterInfo.Name]);
					position++;

				}
			}

			// Method return types
			if (methodInfo.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType != null)
			{
				var returnParameter = methodBuilder.DefineParameter(0, ParameterAttributes.Retval, null);
				returnParameter.ApplyCustomAttributes(methodInfo.ReturnValuePassThroughAttributes);
			}

			if (methodInfo.IsIgnored)
			{
				foreach (var returnValueProperty in methodInfo.MethodBodyInfo.ReturnValueEntityProperties)
				{
					var realParameters = methodBuilder.GetParameters().Where(i => !i.IsRetval).ToArray();

					var parameterMatched =
						realParameters.FirstOrDefault(i => i.ParameterType == returnValueProperty.DataType);

					if (parameterMatched != null)
					{
						methodBuilder.SetOutParameterDefaultValue(parameterMatched);
					}
					else
					{
						throw new InvalidOperationException(
							$"The value property of type {returnValueProperty.DataType} does not have a matched argument and the out put value cannot be returned.");
					}
				}

				if (methodInfo.WillThrowExceptionWhileCalling)
				{
					methodBuilder.GenerateException<IgnoredAssetException>();
				}
				else
				{
					GenerateReturnValue();
				}
			}
			else
			{
				var requestType = ExpandGenericType(methodInfo.MethodBodyInfo.ParameterEntity, methodInfo);
				var assetName = methodInfo.AssetName;

				var g = methodBuilder.GetILGenerator();
				var requestMessage = GenerateRequestCore(methodBuilder, methodInfo.MethodBodyInfo, requestType, assetName);

				if (methodInfo.IsOneWay)
				{
					var sendOneWayCallback =
						typeof(IProxyCommunicate).GetProperty(nameof(IProxyCommunicate.SendOneWayMethodMessageCallback),
							BindingFlags.Public | BindingFlags.Instance)!;

					// ((IProxyCommunicate)this).SendOneWayMethodMessageCallback(requestMessage)

					g.Emit(OpCodes.Ldloc, requestMessage);
					g.Emit(OpCodes.Ldarg_0);

					g.Emit(OpCodes.Ldarg_0);
					g.Emit(OpCodes.Castclass, typeof(IProxyCommunicate));
					g.Emit(OpCodes.Call, sendOneWayCallback.GetMethod);

					g.EmitCalli(OpCodes.Calli, CallingConventions.HasThis, null, new[] { typeof(IRemoteAgencyMessage) },
						null);

					foreach (var returnValueInfo in methodInfo.MethodBodyInfo.ReturnValueEntityProperties)
					{
						switch (returnValueInfo.ReturnValueSource)
						{
							case RemoteAgencyReturnValueSource.ParameterDefaultValue:

								var argumentDefault = g.DeclareLocal(returnValueInfo.DataType);
								var relatedArgument =
									methodBuilder.GetParameters()
										.SingleOrDefault(i => i.ParameterType == returnValueInfo.DataType) ??
									throw new InvalidOperationException(
										$"Cannot find a method argument with type {returnValueInfo.DataType} and thus the return value cannot be produced.");

								// <argument> = default(<Type>)
								g.Emit(OpCodes.Ldloca, argumentDefault);
								g.Emit(OpCodes.Initobj);
								g.Emit(OpCodes.Ldloc, argumentDefault);
								g.Emit(OpCodes.Starg, relatedArgument.Position + 1);
								break;

							case RemoteAgencyReturnValueSource.ReturnValueDefaultValue:
								GenerateReturnValue();
								break;
							default:
								throw new InvalidOperationException(
									$"The return value source type {returnValueInfo.ReturnValueSource} is not supported.");
						}
					}
				}
				else
				{
					var sendMessageCallback = typeof(IProxyCommunicate).GetProperty(
						nameof(IProxyCommunicate.SendMethodMessageCallback),
						BindingFlags.Public | BindingFlags.Instance)!;

					// ((IProxyCommunicate)this).SendMethodMessageCallback(requestMessage, <timeout>)
					g.Emit(OpCodes.Ldloc, requestMessage);
					g.Emit(OpCodes.Ldc_I4, methodInfo.MethodBodyInfo.Timeout);
					g.Emit(OpCodes.Ldarg_0);

					g.Emit(OpCodes.Ldarg_0);
					g.Emit(OpCodes.Call, sendMessageCallback.GetMethod);

					g.EmitCalli(OpCodes.Calli, CallingConventions.HasThis, typeof(IRemoteAgencyMessage),
						new[] { typeof(IRemoteAgencyMessage), typeof(int) }, null);

					// responseMessage = <return_value>
					var responseMessage = g.DeclareLocal(typeof(IRemoteAgencyMessage));
					g.Emit(OpCodes.Stloc, responseMessage);

					// response = (<Type>)responseMessage
					var responseType = ExpandGenericType(methodInfo.MethodBodyInfo.ReturnValueEntity);
					var response = g.DeclareLocal(responseType);
					g.Emit(OpCodes.Ldloc, responseMessage);
					g.Emit(OpCodes.Castclass, responseType);
					g.Emit(OpCodes.Stloc, response);


					// if (responseMessage.Exception == null ...)
					var exceptionProperty =
						(typeof(IRemoteAgencyMessage)).GetProperty(nameof(IRemoteAgencyMessage.Exception))!;
					g.Emit(OpCodes.Ldloc, responseMessage);
					g.Emit(OpCodes.Call, exceptionProperty.GetMethod);

					var nonExceptionLabel = g.DefineLabel();
					g.Emit(OpCodes.Brfalse, nonExceptionLabel);

					foreach (var returnValueInfo in methodInfo.MethodBodyInfo.ReturnValueEntityProperties)
					{
						switch (returnValueInfo.ReturnValueSource)
						{
							case RemoteAgencyReturnValueSource.Parameter:
								if (((RemoteAgencyReturnValueInfoFromParameter)returnValueInfo).IsIncludedWhenExceptionThrown)
								{
									var responseProperty = responseType.GetProperty(returnValueInfo.PropertyName) ??
														   throw new InvalidOperationException(
															   $"Cannot find property {returnValueInfo.PropertyName} on type {responseType.FullName}");
									var parameter = methodBuilder.GetParameters()
														.SingleOrDefault(i =>
															i.ParameterType == returnValueInfo.DataType) ??
													throw new InvalidOperationException(
														$"Cannot find an argument with the type {returnValueInfo.DataType.FullName}");

									g.Emit(OpCodes.Ldloc, response);
									g.Emit(OpCodes.Call, responseProperty.GetMethod);
									g.Emit(OpCodes.Starg, parameter.Position + 1);
								}
								break;
						}
					}




				}
			}
		}

		/// <summary>
		/// Expand a generic type if <see cref="InterfaceInfo"/> has generic arguments.
		/// </summary>
		/// <param name="type">The source type.</param>
		/// <returns>The expanded type.</returns>
		Type ExpandGenericType(Type type)
		{
			return !InterfaceInfo.IsSourceInterfaceGenericType
				? type
				: type.MakeGenericType(InterfaceInfo.SourceInterfaceGenericArguments);
		}


		/// <summary>
		/// Expand a generic type if <see cref="InterfaceInfo"/> or <paramref name="methodInfo"/> has generic arguments.
		/// </summary>
		/// <param name="type">The source type.</param>
		/// <param name="methodInfo">The additional method context.</param>
		/// <returns>The expanded type.</returns>
		Type ExpandGenericType(Type type, RemoteAgencyMethodInfo methodInfo)
		{
			if (InterfaceInfo.IsSourceInterfaceGenericType || methodInfo.IsGenericMethod)
			{
				var genericArguments =
					InterfaceInfo.InterfaceLevelGenericParameters.Concat(methodInfo.AssetLevelGenericParameters);

				return type.MakeGenericType(genericArguments.ToArray());
			}

			return type;
		}


		private static LocalBuilder GenerateRequestCore(MethodBuilder methodBuilder,
			RemoteAgencyMethodBodyInfo methodBodyInfo, Type requestType, string assetName)
		{
			var g = methodBuilder.GetILGenerator();

			var request = g.DeclareLocal(requestType);
			var ctor = requestType.GetConstructor(Type.EmptyTypes);

			// request = new <RequestType>()
			g.Emit(OpCodes.Newobj, ctor);
			g.Emit(OpCodes.Stloc, request);


			foreach (var parameterInfo in methodBodyInfo.ParameterEntityProperties)
			{
				var requestProperty = requestType.GetProperty(parameterInfo.PropertyName,
										  BindingFlags.Public | BindingFlags.Instance)
									  ?? throw new InvalidOperationException(
										  $"Cannot find property {parameterInfo.PropertyName} on type {requestType.FullName}.");

				var valueParameter = methodBuilder.GetParameters()
					.First(i => i.Name == parameterInfo.ParameterName);

				// this.<PropertyName> = <argument>
				g.Emit(OpCodes.Ldarg, valueParameter.Position + 1); // 0 = this
				g.Emit(OpCodes.Ldarg_0);
				g.Emit(OpCodes.Call, (MethodInfo)requestProperty.SetMethod);
			}

			var requestMessage = g.DeclareLocal(typeof(IRemoteAgencyMessage));

			// requestMessage = (IRemoteAgencyMessage)request
			g.Emit(OpCodes.Ldloc, request);
			g.Emit(OpCodes.Castclass, typeof(IRemoteAgencyMessage));
			g.Emit(OpCodes.Stloc, requestMessage);

			var assetNameProperty = typeof(IRemoteAgencyMessage).GetProperty(nameof(IRemoteAgencyMessage.AssetName),
				BindingFlags.Public | BindingFlags.Instance)!;

			// requestMessage.AssetName = <AssetName>
			g.Emit(OpCodes.Ldstr, assetName);
			g.Emit(OpCodes.Ldloc, requestMessage);
			g.Emit(OpCodes.Call, assetNameProperty.SetMethod);

			return requestMessage;
		}
	}

	internal abstract class ProxyBase : IProxyCommunicate
	{
		public RemoteAgencyInterfaceInfo InterfaceInfo { get; set; }

		#region Unsupported Method

		Guid IProxyCommunicate.InstanceId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendTwoWayMessageCallback IProxyCommunicate.SendMethodMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendOneWayMessageCallback IProxyCommunicate.SendOneWayMethodMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendTwoWayMessageCallback IProxyCommunicate.SendEventAddingMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendTwoWayMessageCallback IProxyCommunicate.SendEventRemovingMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendTwoWayMessageCallback IProxyCommunicate.SendPropertyGetMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendOneWayMessageCallback IProxyCommunicate.SendOneWayPropertyGetMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendTwoWayMessageCallback IProxyCommunicate.SendPropertySetMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendOneWayMessageCallback IProxyCommunicate.SendOneWayPropertySetMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		SendOneWayMessageCallback IManagedObjectCommunicate.SendOneWaySpecialCommandMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		CreateEmptyMessageCallback IManagedObjectCommunicate.CreateEmptyMessageCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		#endregion

		void IManagedObjectCommunicate.CloseRequestedByManagingObject(bool sendSpecialCommand)
		{
			throw new NotImplementedException();
		}

		string[] IProxyCommunicate.GetInitOnlyPropertyNames()
		{
			var result = new List<string>();

			foreach (var i in InterfaceInfo.Properties)
			{
				if (i.IsSetMarkedAsInit)
				{
					result.Add(i.AssetName);
				}
			}

			return result.Any() ? result.ToArray() : null;
		}

		void IProxyCommunicate.OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
		{
			throw new NotImplementedException();
		}

		IRemoteAgencyMessage IProxyCommunicate.ProcessEventRaisingMessage(IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		void IProxyCommunicate.ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		void IProxyCommunicate.SetInitOnlyPropertyValue(string propertyName, object value)
		{
			var propertyInfo = this.GetType().GetProperty(propertyName);
			propertyInfo.SetValue(this, value);
		}
	}

	/// <summary>
	/// Base class used for implementing the <see cref="IProxyCommunicate"/> interface with helper supported.
	/// </summary>
	internal class ProxyBaseWithHelper : ProxyBase, IProxyCommunicate
	{
		/// <summary>
		/// The internal <see cref="ProxyEventHelper"/> instance.
		/// </summary>
		private ProxyEventHelper Helper { get; }

		#region Constructors

		public ProxyBaseWithHelper()
		{
		}

		public ProxyBaseWithHelper(ProxyEventHelper helper)
		{
			Helper = helper;
		}

		#endregion

		#region Direct properties

		Guid IProxyCommunicate.InstanceId { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendMethodMessageCallback { get; set; }

		SendOneWayMessageCallback IProxyCommunicate.SendOneWayMethodMessageCallback { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendPropertyGetMessageCallback { get; set; }

		SendOneWayMessageCallback IProxyCommunicate.SendOneWayPropertyGetMessageCallback { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendPropertySetMessageCallback { get; set; }

		SendOneWayMessageCallback IProxyCommunicate.SendOneWayPropertySetMessageCallback { get; set; }

		#endregion

		#region Indirect properties

		SendTwoWayMessageCallback IProxyCommunicate.SendEventAddingMessageCallback
		{
			get => Helper.SendEventAddingMessageCallback;
			set => Helper.SendEventAddingMessageCallback = value;
		}

		SendTwoWayMessageCallback IProxyCommunicate.SendEventRemovingMessageCallback
		{
			get => Helper.SendEventRemovingMessageCallback;
			set => Helper.SendEventRemovingMessageCallback = value;
		}

		SendOneWayMessageCallback IManagedObjectCommunicate.SendOneWaySpecialCommandMessageCallback
		{
			get => Helper.SendOneWaySpecialCommandMessageCallback;
			set => Helper.SendOneWaySpecialCommandMessageCallback = value;
		}

		CreateEmptyMessageCallback IManagedObjectCommunicate.CreateEmptyMessageCallback
		{
			get => Helper.CreateEmptyMessageCallback;
			set => Helper.CreateEmptyMessageCallback = value;
		}

		#endregion

		#region Methods

		IRemoteAgencyMessage IProxyCommunicate.ProcessEventRaisingMessage(IRemoteAgencyMessage message,
			out Exception exception,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			return Helper.ProcessEventRaisingMessage(message, out exception, out localExceptionHandlingMode);
		}

		void IProxyCommunicate.ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			Helper.ProcessOneWayEventRaisingMessage(message, out localExceptionHandlingMode);
		}

		void IProxyCommunicate.OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
		{
			Helper.OnRemoteServiceWrapperClosing(siteId, serviceWrapperInstanceId);
		}

		void IManagedObjectCommunicate.CloseRequestedByManagingObject(bool sendSpecialCommand)
		{
			Helper.CloseRequestedByManagingObject(sendSpecialCommand);
		}

		#endregion
	}

	/// <summary>
	///     Base class for generated proxy type without helper.
	/// </summary>
	internal class ProxyBaseWithoutHelper : ProxyBase, IProxyCommunicate
	{
		SendOneWayMessageCallback IManagedObjectCommunicate.SendOneWaySpecialCommandMessageCallback { get; set; }

		CreateEmptyMessageCallback IManagedObjectCommunicate.CreateEmptyMessageCallback { get; set; }

		Guid IProxyCommunicate.InstanceId { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendMethodMessageCallback { get; set; }

		SendOneWayMessageCallback IProxyCommunicate.SendOneWayMethodMessageCallback { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendEventAddingMessageCallback { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendEventRemovingMessageCallback { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendPropertyGetMessageCallback { get; set; }

		SendOneWayMessageCallback IProxyCommunicate.SendOneWayPropertyGetMessageCallback { get; set; }

		SendTwoWayMessageCallback IProxyCommunicate.SendPropertySetMessageCallback { get; set; }

		SendOneWayMessageCallback IProxyCommunicate.SendOneWayPropertySetMessageCallback { get; set; }

		void IManagedObjectCommunicate.CloseRequestedByManagingObject(bool sendSpecialCommand)
		{
		}

		IRemoteAgencyMessage IProxyCommunicate.ProcessEventRaisingMessage(IRemoteAgencyMessage message,
			out Exception exception,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			exception = null;
			localExceptionHandlingMode = LocalExceptionHandlingMode.Throw;
			return null;
		}

		void IProxyCommunicate.ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			localExceptionHandlingMode = LocalExceptionHandlingMode.Throw;
		}

		void IProxyCommunicate.OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
		{
		}
	}
}