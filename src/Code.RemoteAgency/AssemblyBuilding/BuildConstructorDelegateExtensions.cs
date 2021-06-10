using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    static class BuildConstructorDelegateExtensions
    {
        private static readonly Type[] EmptyType = Array.Empty<Type>();

        internal static TDelegate BuildConstructorDelegate<TDelegate>(this Type type)
            where TDelegate : Delegate
        {
            var methodName = Guid.NewGuid();
            var dynMethod = new DynamicMethod($"FastActivatorMethod_{methodName:N}", type, EmptyType, type);
            var ilGen = dynMethod.GetILGenerator();
            ilGen.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(EmptyType) ?? throw new InvalidOperationException());
            ilGen.Emit(OpCodes.Ret);
            return (TDelegate)dynMethod.CreateDelegate(typeof(TDelegate));
        }

        internal static TDelegate BuildConstructorDelegate<TDelegate>(this Type type, Type argType)
            where TDelegate : Delegate
        {
            var argTypes = new[] {argType};
            var methodName = Guid.NewGuid();
            var dynMethod = new DynamicMethod($"FastActivatorMethod_{methodName:N}", type, argTypes, type);
            var ilGen = dynMethod.GetILGenerator();
            ilGen.Emit(OpCodes.Ldarg, 0); //one parameter
            ilGen.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(argTypes) ?? throw new InvalidOperationException());
            ilGen.Emit(OpCodes.Ret);
            return (TDelegate)dynMethod.CreateDelegate(typeof(TDelegate));
        }

        //internal static TDelegate BuildConstructorDelegate<TDelegate>(this Type type, params Type[] argTypes)
        //    where TDelegate : Delegate
        //{
        //    var methodName = Guid.NewGuid();
        //    var dynMethod = new DynamicMethod($"FastActivatorMethod_{methodName:N}", type, argTypes, type);
        //    var ilGen = dynMethod.GetILGenerator();
        //    for (var argIdx = 0; argIdx < argTypes.Length; argIdx++)
        //    {
        //        ilGen.Emit(OpCodes.Ldarg, argIdx);
        //    }
        //    ilGen.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(argTypes) ?? throw new InvalidOperationException());
        //    ilGen.Emit(OpCodes.Ret);
        //    return (TDelegate)dynMethod.CreateDelegate(typeof(TDelegate));
        //}
    }
}
