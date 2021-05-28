using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    /// <summary>
    /// Activator faster than built in version.
    /// </summary>
    internal static class FastActivator
    {
        static readonly ConcurrentDictionary<Type, Func<object>> ConstructorCache = new ();

        /// <summary>
        /// Creates an instance of the type specified.
        /// </summary>
        /// <param name="type">The type of the instance to be created.</param>
        /// <returns>The created instance.</returns>
        public static object CreateInstance(Type type)
        {
            return GetConstructor(type)();
        }

        ///// <summary>
        ///// Creates an instance of the type specified and cast it to the type specified.
        ///// </summary>
        ///// <typeparam name="T">Target type to be cast to.</typeparam>
        ///// <param name="type">The type of the instance to be created.</param>
        ///// <returns>The created instance.</returns>
        //public static T CreateInstance<T>(Type type) where T : class
        //{
        //    return CreateInstance(type) as T;
        //}

        static Func<object> GetConstructor(Type objType)
        {
            return ConstructorCache.GetOrAdd(objType, i => (Func<object>)BuildConstructorDelegate(i, typeof(Func<object>), Array.Empty<Type>()));
        }

        internal static object BuildConstructorDelegate(Type type, Type delegateType, Type[] argTypes)
        {
            Guid methodName = Guid.NewGuid();
            var dynMethod = new DynamicMethod($"FastActivatorMethod_{methodName:N}", type, argTypes, type);
            ILGenerator ilGen = dynMethod.GetILGenerator();
            for (int argIdx = 0; argIdx < argTypes.Length; argIdx++)
            {
                ilGen.Emit(OpCodes.Ldarg, argIdx);
            }
            ilGen.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(argTypes) ?? throw new InvalidOperationException());
            ilGen.Emit(OpCodes.Ret);
            return dynMethod.CreateDelegate(delegateType);
        }
    }

    /// <summary>
    /// Activator faster than built in version.
    /// </summary>
    /// <typeparam name="TArg">Type of the argument of the constructor.</typeparam>
    internal static class FastActivator<TArg>
    {
        private static readonly ConcurrentDictionary<Type, Func<TArg, object>> ConstructorCache = new ();

        /// <summary>
        /// Creates an instance of the type specified.
        /// </summary>
        /// <param name="type">The type of the instance to be created.</param>
        /// <param name="arg1">The argument which will be passed to the constructor.</param>
        /// <returns>The created instance.</returns>
        public static object CreateInstance(Type type, TArg arg1)
        {
            return GetConstructor(type, new[] { typeof(TArg) })(arg1);
        }

        /// <summary>
        /// Creates an instance of the type specified and cast it to the type specified.
        /// </summary>
        /// <typeparam name="T">Target type to be cast to.</typeparam>
        /// <param name="type">The type of the instance to be created.</param>
        /// <param name="arg1">The argument which will be passed to the constructor.</param>
        /// <returns>The created instance.</returns>
        public static T CreateInstance<T>(Type type, TArg arg1) where T : class
        {
            return CreateInstance(type, arg1) as T;
        }

        static Func<TArg, object> GetConstructor(Type objType, Type[] argTypes)
        {
            return ConstructorCache.GetOrAdd(objType, i => (Func<TArg, object>)FastActivator.BuildConstructorDelegate(i, typeof(Func<TArg, object>), argTypes));
        }
    }
}