using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Activator faster than built in version.
    /// </summary>
    public static class FastActivator
    {
        static ConcurrentDictionary<Type, Func<object>> constructorCache = new ConcurrentDictionary<Type, Func<object>>();

        /// <summary>
        /// Creates an instance of the type specified
        /// </summary>
        /// <param name="type">The type of the instance to be created.</param>
        /// <returns>The created instance.</returns>
        public static object CreateInstance(Type type)
        {
            return GetConstructor(type)();
        }

        static Func<object> GetConstructor(Type objType)
        {
            return constructorCache.GetOrAdd(objType, i => (Func<object>)BuildConstructorDelegate(i, typeof(Func<object>), new Type[] { }));
        }

        internal static object BuildConstructorDelegate(Type type, Type delegateType, Type[] argTypes)
        {
            var dynMethod = new DynamicMethod(string.Format("FastActivatorMethod_{0}_{1}", type.Name, argTypes.Length), type, argTypes, type);
            ILGenerator ilGen = dynMethod.GetILGenerator();
            for (int argIdx = 0; argIdx < argTypes.Length; argIdx++)
            {
                ilGen.Emit(OpCodes.Ldarg, argIdx);
            }
            ilGen.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(argTypes));
            ilGen.Emit(OpCodes.Ret);
            return dynMethod.CreateDelegate(delegateType);
        }
    }

    /// <summary>
    /// Activator faster than built in version.
    /// </summary>
    /// <typeparam name="TArg">Type of the argument of the constructor.</typeparam>
    public static class FastActivator<TArg>
    {
        static ConcurrentDictionary<Type, Func<TArg, object>> constructorCache = new ConcurrentDictionary<Type, Func<TArg, object>>();
        /// <summary>
        /// Creates an instance of the type specified
        /// </summary>
        /// <param name="type">The type of the instance to be created.</param>
        /// <param name="arg1">The argument which will be passed to the constructor.</param>
        /// <returns>The created instance.</returns>
        public static object CreateInstance(Type type, TArg arg1)
        {
            return GetConstructor(type, new Type[] { typeof(TArg) })(arg1);
        }
        static Func<TArg, object> GetConstructor(Type objType, Type[] argTypes)
        {
            return constructorCache.GetOrAdd(objType, i => (Func<TArg, object>)FastActivator.BuildConstructorDelegate(i, typeof(Func<TArg, object>), argTypes));
        }
    }
}