using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mmosoft.DependencyInversion
{
    public static class DI
    {
        // type container
        private static Dictionary<Type, Tuple<Type, RegisterOption>> _container;        
        private static MethodInfo _genericResolver;
        private static Dictionary<Type, MethodInfo> _resolverCache;
        // cached instance
        private static Dictionary<Type, object> _cached;

        static DI()
        {
            _container = new Dictionary<Type, Tuple<Type, RegisterOption>>();
            _genericResolver = typeof(DI).GetMethod("Resolve");
            _resolverCache = new Dictionary<Type, MethodInfo>();

            _cached = new Dictionary<Type, object>();
        }

        public static void Register<InterfaceType, ImplementType>(RegisterOption option)
        {
            _container[typeof(InterfaceType)] = Tuple.Create(typeof(ImplementType), option);
        }

        public static InterfaceType Resolve<InterfaceType>()
        {
            Type @interface = typeof(InterfaceType);
            if (_container.ContainsKey(@interface))
            {
                var implType = _container[@interface].Item1;
                var createOption = _container[@interface].Item2;
                if (createOption == RegisterOption.NoCache)
                {
                    return (InterfaceType) resolveType(implType);
                }
                else
                {
                    if (!_cached.ContainsKey(@interface))
                        _cached[@interface] = resolveType(implType);
                    return (InterfaceType) _cached[@interface];
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private static object resolveType(Type implType)
        {
            ConstructorInfo ctorInfos = implType.GetConstructors().FirstOrDefault();

            if (ctorInfos != null)
            {
                ParameterInfo[] paramInfos = ctorInfos.GetParameters();
                object[] @params = new object[paramInfos.Length];

                for (int i = 0; i < paramInfos.Length; i++)
                {
                    if (!_resolverCache.ContainsKey(paramInfos[i].ParameterType))
                    {
                        _resolverCache[paramInfos[i].ParameterType] = _genericResolver.MakeGenericMethod(paramInfos[i].ParameterType);
                    }
                    
                    @params[i] = _resolverCache[paramInfos[i].ParameterType].Invoke(
                        null, // invoking static method doesn't require object instance => pass null
                        null // Resolve method don't have any parameters => pass null
                    );
                }

                return ctorInfos.Invoke(@params);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}