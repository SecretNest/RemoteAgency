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
			}
		}

	}

	/// <summary>
	/// Base class used for implementing the <see cref="IProxyCommunicate"/> interface with helper supported.
	/// </summary>
	internal class ProxyBaseWithHelper : IProxyCommunicate
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
	internal class ProxyBaseWithoutHelper : IProxyCommunicate
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