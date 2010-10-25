using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Castle.DynamicProxy;
using WPFCoreTools;

namespace NotifyPropertyChanged.Castle.UsingPOCO
{
    public class NotifierInterceptor : IInterceptor
    {
        public static Dictionary<string, PropertyChangedEventArgs> _cache =
            new Dictionary<string, PropertyChangedEventArgs>();

        public IDictionary<string, IEnumerable<string>> NotifyingProperties;
        PropertyChangedEventHandler propertyChangedEventHandler;
       
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "add_PropertyChanged")
            {
                propertyChangedEventHandler = (PropertyChangedEventHandler)
                    Delegate.Combine(propertyChangedEventHandler, (Delegate)invocation.Arguments[0]);
                invocation.ReturnValue = propertyChangedEventHandler;
            }
            else if (invocation.Method.Name == "remove_PropertyChanged")
            {
                propertyChangedEventHandler = (PropertyChangedEventHandler)
                    Delegate.Remove(propertyChangedEventHandler, (Delegate)invocation.Arguments[0]);
                invocation.ReturnValue = propertyChangedEventHandler;
            }
            else if ((NotifyingProperties ?? FindNotifyingProperties(invocation.Method.DeclaringType)).ContainsKey(invocation.Method.Name))
            {
                invocation.Proceed();
                if (propertyChangedEventHandler != null)
                {
                    propertyChangedEventHandler(invocation.Proxy, RetrievePropertyChangedArg(invocation.Method.Name));
                    NotifyingProperties[invocation.Method.Name].ForEach(name =>
                        {
                            propertyChangedEventHandler(invocation.Proxy, RetrievePropertyChangedArg(name));
                        Console.WriteLine("Invoked for {0}", name);
                        });
                }
            }
            else
                invocation.Proceed();
        }

        static PropertyChangedEventArgs RetrievePropertyChangedArg(string methodName)
        {
            PropertyChangedEventArgs arg = null;
            if (!_cache.TryGetValue(methodName, out arg))
            {
                arg = new PropertyChangedEventArgs(methodName.Substring(4));
                _cache.Add(methodName, arg);
            }

            return arg;
        }

        IDictionary<string, IEnumerable<string>> FindNotifyingProperties(Type type)
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

            return NotifyingProperties;
        }
    }
}