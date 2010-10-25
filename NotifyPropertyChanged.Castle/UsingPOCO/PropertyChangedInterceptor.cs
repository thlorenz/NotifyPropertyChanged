using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Castle.DynamicProxy;
using WPFCoreTools;

namespace NotifyPropertyChanged.Castle.UsingPOCO
{
    public class PropertyChangedInterceptor : IInterceptor
    {
        public static Dictionary<string, PropertyChangedEventArgs> _cache =
            new Dictionary<string, PropertyChangedEventArgs>();

        public IDictionary<string, IEnumerable<string>> NotifyingProperties;
        PropertyChangedEventHandler propertyChangedEventHandler = delegate { };

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "add_PropertyChanged")
            {
                AddEventSubscription(invocation);
                return;
            }

            if (invocation.Method.Name == "remove_PropertyChanged")
            {
                RemoveEventSubsciption(invocation);
                return;
            }

            if (invocation.Method.Name.StartsWith("set_"))
            {
                if (NotifyingProperties == null)
                    FindNotifyingProperties(invocation.Method.DeclaringType);

                if (NotifyingProperties.Count == 0)
                    InvokeSetAndRaiseNotifyPropertyChangedEvent(invocation);
                else
                    RaiseEventOnlyIfAttributeIsPresent(invocation);
            }
            else
                invocation.Proceed();
        }

        static PropertyChangedEventArgs RetrievePropertyChangedArg(string methodName)
        {
            PropertyChangedEventArgs arg;
            if (!_cache.TryGetValue(methodName, out arg))
            {
                var propertyName = methodName.StartsWith("set_") ? methodName.Replace("set_", string.Empty) : methodName;
                arg = new PropertyChangedEventArgs(propertyName);
                _cache.Add(methodName, arg);
            }

            return arg;
        }

        void AddEventSubscription(IInvocation invocation)
        {
            propertyChangedEventHandler = (PropertyChangedEventHandler)
                Delegate.Combine(propertyChangedEventHandler, (Delegate)invocation.Arguments[0]);
            invocation.ReturnValue = propertyChangedEventHandler;
        }

        void FindNotifyingProperties(Type type)
        {
            NotifyingProperties = new Dictionary<string, IEnumerable<string>>();

            PropertyInfo[] props = type.GetProperties();

            foreach (var propertyInfo in props)
            {
                var attribs = propertyInfo.GetCustomAttributes(false);
                foreach (var attrib in attribs)
                {
                    if (attrib is NotifyPropertyChangedAttribute)
                    {
                        var mainPropName = propertyInfo.Name;
                        var namesToRaise = new List<string> { mainPropName };

                        ((NotifyPropertyChangedAttribute)attrib).AdditionalAffectedProperties.ForEach(namesToRaise.Add);
                        NotifyingProperties["set_" + mainPropName] = namesToRaise;
                    }
                }
            }
        }

        void InvokeSetAndRaiseNotifyPropertyChangedEvent(IInvocation invocation)
        {
                invocation.Proceed();
                propertyChangedEventHandler(invocation.Proxy, RetrievePropertyChangedArg(invocation.Method.Name));
        }

        void RaiseEventOnlyIfAttributeIsPresent(IInvocation invocation)
        {
            if (NotifyingProperties.ContainsKey(invocation.Method.Name))
            {
                InvokeSetAndRaiseNotifyPropertyChangedEvent(invocation);
                NotifyingProperties[invocation.Method.Name].ForEach(
                    name => propertyChangedEventHandler(invocation.Proxy, RetrievePropertyChangedArg(name)));
            }
        }

        void RemoveEventSubsciption(IInvocation invocation)
        {
            propertyChangedEventHandler = (PropertyChangedEventHandler)
                Delegate.Remove(propertyChangedEventHandler, (Delegate)invocation.Arguments[0]);
            invocation.ReturnValue = propertyChangedEventHandler;
        }
    }
}