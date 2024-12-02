using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
	internal partial class AssemblyBuildingEmitter
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter
		internal void EmitServiceWrapper(
			//ReSharper disable once UnusedParameter.Local
			TypeBuilder serviceWrapperTypeBuilder)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression
		{
			//TODO: write code here to build service wrapper.

			//interface info: InterfaceInfo
			var serviceType = serviceWrapperTypeBuilder.GetInterfaces()
				.First(i => i != typeof(IServiceWrapperCommunicate));

			ImplementSystemInterface(serviceWrapperTypeBuilder);

			var interfaceField = GenerateInterfaceInfoField(serviceWrapperTypeBuilder, serviceType);
			var constructor = GenerateConstructor(serviceWrapperTypeBuilder, serviceType, interfaceField);

			throw new Exception();
		}

		private void ImplementSystemInterface(TypeBuilder type)
		{
			type.SetParent(typeof(ServiceWrapperBase));
		}

		private FieldInfo GenerateInterfaceInfoField(TypeBuilder type, Type interfaceType)
		{
			var fieldName = RandomizedName.GetRandomizedName("ServiceObject");
			var field = type.DefineField(fieldName, interfaceType, FieldAttributes.Private);

			return field;
		}

		/// <summary>
		/// Generate constructor for the service wrapper.
		/// </summary>
		/// <param name="type">The <see cref="TypeBuilder"/> instance.</param>
		/// <param name="interfaceType">The represented interface type.</param>
		/// <param name="interfaceField">The field used to store the interface instance.</param>
		/// <returns></returns>
		private ConstructorInfo GenerateConstructor(TypeBuilder type, Type interfaceType, FieldInfo interfaceField)
		{
			var method = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [interfaceType]);
			method.DefineParameter(1, ParameterAttributes.None, "info");

			var g = method.GetILGenerator();
			g.Emit(OpCodes.Ldarg_0); // this
			g.Emit(OpCodes.Ldarg_1); // arg info
			g.Emit(OpCodes.Stfld, interfaceField); // this.<Field> = info
			g.Emit(OpCodes.Ret);

			return method;
		}
	}

	internal class ServiceWrapperBase : IServiceWrapperCommunicate
	{
		public void CloseRequestedByManagingObject(bool sendSpecialCommand)
		{
			throw new NotImplementedException();
		}

		public SendOneWayMessageCallback SendOneWaySpecialCommandMessageCallback { get; set; }
		public CreateEmptyMessageCallback CreateEmptyMessageCallback { get; set; }

		public IRemoteAgencyMessage ProcessMethodMessage(IRemoteAgencyMessage message, out Exception exception,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public void ProcessOneWayMethodMessage(IRemoteAgencyMessage message,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public void ProcessEventAddingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public void ProcessEventRemovingMessage(IRemoteAgencyMessage message,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public SendTwoWayMessageCallback SendEventMessageCallback { get; set; }
		public SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }

		public IRemoteAgencyMessage ProcessPropertyGettingMessage(IRemoteAgencyMessage message, out Exception exception,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public void ProcessOneWayPropertyGettingMessage(IRemoteAgencyMessage message,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public IRemoteAgencyMessage ProcessPropertySettingMessage(IRemoteAgencyMessage message, out Exception exception,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public void ProcessOneWayPropertySettingMessage(IRemoteAgencyMessage message,
			out LocalExceptionHandlingMode localExceptionHandlingMode)
		{
			throw new NotImplementedException();
		}

		public void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId = null)
		{
			throw new NotImplementedException();
		}
	}
}
