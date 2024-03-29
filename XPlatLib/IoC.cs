﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XPlatXampLib
{
    /// <summary>
    /// A rudimentary IoC container with 
    /// </summary>
    /// <remarks>
    /// Stolen from MvxMod: https://github.com/MvxMod/MvxMod/blob/master/Cirrious/Cirrious.MvvmCross/IoC/MvxSimpleIoCContainer.cs
    /// Available under the Microsoft Public License.
    /// http://opensource.org/licenses/ms-pl.html
    /// 
    /// PJD - Added BaseResolver with constructor, parameter, and field injection.
    /// </remarks>
    public class IoC
    {
        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();

        private static IoC _container;
        public static IoC Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new IoC();
                }
                return _container;
            }
        }


        #region Public Methods

        public bool CanResolve<T>()
            where T : class
        {
            lock (this)
            {
                return _resolvers.ContainsKey(typeof(T));
            }
        }

        public bool CanResolve(Type type)
        {
            lock (this)
            {
                return _resolvers.ContainsKey(type);
            }
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            lock (this)
            {
                IResolver resolver;
                if (!_resolvers.TryGetValue(typeof(T), out resolver))
                {
                    resolved = default(T);
                    return false;
                }


                var raw = resolver.Resolve();
                if (!(raw is T))
                {
                    throw new Exception(string.Format("Resolver returned object type {0} which does not support interface {1}",
                                            raw.GetType().FullName, typeof(T).FullName));
                }
                resolved = (T)raw;
                return true;
            }
        }

        public bool TryResolve(Type type, out object resolved)
        {
            lock (this)
            {
                IResolver resolver;
                if (!_resolvers.TryGetValue(type, out resolver))
                {
                    resolved = Activator.CreateInstance(type);
                    return false;
                }
                object raw = resolver.Resolve();
                if ((raw.GetType() != type) && !type.IsAssignableFrom(raw.GetType()))
                {
                    throw new Exception(string.Format("Resolver returned object type {0} which does not support interface {1}",
                                            raw.GetType().FullName, type.FullName));
                }
                resolved = raw;
                return true;
            }
        }

        public T Resolve<T>()
            where T : class
        {
            lock (this)
            {
                T resolved;
                if (!this.TryResolve(out resolved))
                {
                    throw new Exception(string.Format("Failed to resolve type {0}", typeof(T).FullName));
                }
                return resolved;
            }
        }

        public object Resolve(Type type)
        {
            lock (this)
            {
                object resolved;
                if (!this.TryResolve(type, out resolved))
                {
                    throw new Exception(string.Format("Failed to resolve type {0}", type.FullName));
                }
                return resolved;
            }
        }

        public void RegisterServiceType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class
        {
            lock (this)
            {
                _resolvers[typeof(TInterface)] = new ConstructingResolver(typeof(TToConstruct));
            }
        }

        public void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class
        {
            lock (this)
            {
                _resolvers[typeof(TInterface)] = new SingletonResolver(theObject);
            }
        }

        public void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            lock (this)
            {
                _resolvers[typeof(TInterface)] = new ConstructingSingletonResolver(() => (object)theConstructor());
            }
        }


        public void DeregisterType<TInterface>()
        {
            lock (this)
            {
                _resolvers.Remove(typeof(TInterface));
            }
        }

        public void DeregisterType(Type tInterface)
        {
            lock (this)
            {
                _resolvers.Remove(tInterface);
            }
        }

        #endregion

        #region Resolver Helper Classes

        private interface IResolver
        {
            object Resolve();
        }

        private class BaseResolver : IResolver
        {

            public Type ObjectType { get; protected set; }

            #region Implementation of IResolver

            public virtual object Resolve()
            {
                throw new NotImplementedException("You must override this in the derived class.");
            }

            #endregion

            #region Protected Methods

            protected void PopulateFieldsAndProperties(object instance)
            {
                PropertyInfo[] props = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (PropertyInfo pi in props)
                {
                    if (pi.GetCustomAttributes(true).Any(p=>p is NoIoCResolveAttribute))
                    {
                        continue;
                    }
                    if (_container.CanResolve(pi.PropertyType) && pi.CanWrite && pi.GetValue(instance, null) == null)
                    {
                        pi.SetValue(instance, _container.Resolve(pi.PropertyType), null);
                    }
                }
                foreach (FieldInfo fi in fields)
                {
                    if (fi.GetCustomAttributes(true).Any(p => p is NoIoCResolveAttribute))
                    {
                        continue;
                    }
                    if (_container.CanResolve(fi.FieldType) && !fi.IsInitOnly && fi.GetValue(instance) == null)
                    {
                        fi.SetValue(instance, _container.Resolve(fi.FieldType));
                    }
                }
            }

            protected bool RequiresParameters()
            {
                ConstructorInfo[] ci = ObjectType.GetConstructors();
                if (ci.Count() > 0)
                {
                    if (ci.Where(p => p.GetParameters().Length == 0).FirstOrDefault() != null)
                    {
                        return false;
                    }
                }
                if (ci.Count() == 0)
                {
                    return false;
                }
                return true;
            }

            protected object CreateParameterizedInstance()
            {
                ConstructorInfo[] ci = ObjectType.GetConstructors();
                if (ci.Length > 1)
                {
                    throw new ArgumentException("Type " + ObjectType.FullName + " has more than once constructor");
                }

                ParameterInfo[] paramInfos = ci[0].GetParameters();
                List<object> paramValues = new List<object>();
                foreach (ParameterInfo pi in paramInfos)
                {
                    if (_container.CanResolve(pi.ParameterType))
                    {
                        paramValues.Add(_container.Resolve(pi.ParameterType));
                    }
                    else
                    {
                        throw new ArgumentException("Unable to resolve type " + pi.ParameterType.FullName + " as parameter for constructor of type " + ObjectType.FullName);
                    }
                }

                return Activator.CreateInstance(ObjectType, paramValues.ToArray());
            }

            #endregion
        }


        private class ConstructingResolver : BaseResolver
        {
            public ConstructingResolver(Type type)
            {
                ObjectType = type;
            }

            #region Implementation of IResolver


            public override object Resolve()
            {
                object instance = null;
                if (RequiresParameters())
                {
                    instance = CreateParameterizedInstance();
                }
                else
                {
                    instance = Activator.CreateInstance(ObjectType);
                }
                PopulateFieldsAndProperties(instance);
                return instance;
            }

            #endregion
        }


        private class SingletonResolver : BaseResolver
        {
            private readonly object _theObject;


            public SingletonResolver(object theObject)
            {
                _theObject = theObject;
            }

            #region Implementation of IResolver


            public override object Resolve()
            {
                return _theObject;
            }

            #endregion
        }


        private class ConstructingSingletonResolver : BaseResolver
        {
            private readonly Func<object> _theConstructor;
            private object _theObject;


            public ConstructingSingletonResolver(Func<object> theConstructor)
            {
                _theConstructor = theConstructor;
            }

            #region Implementation of IResolver


            public override object Resolve()
            {
                if (_theObject != null)
                    return _theObject;


                lock (_theConstructor)
                {
                    if (_theObject == null)
                    {
                        _theObject = _theConstructor();
                        PopulateFieldsAndProperties(_theObject);
                    }
                }


                return _theObject;
            }


            #endregion
        }

        #endregion
    }

    public class NoIoCResolveAttribute : Attribute
    {
    }

}
