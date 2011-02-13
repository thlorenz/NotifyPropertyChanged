using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using WPFCoreTools;

namespace NotifyPropertyChanged.Castle.UsingViewModelBase
{
    /// <summary>
    /// One way to do it, but Viewmodel needs to have a RaisePropertyChangeMethod
    /// </summary>
    /// <typeparam name="T">ViewModel needs to inherit from ViewModel base.</typeparam>
    public class ViewModelInterceptor<T> : IInterceptor where T : ViewModelBase
    {
        public MethodInfo RaisePropertyChanged;
        public IDictionary<string, IEnumerable<string>> NotifyingProperties = new Dictionary<string, IEnumerable<string>>();

        public ViewModelInterceptor()
        {
            RaisePropertyChanged = typeof(T).GetMethod(
                "RaisePropertyChanged", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, 
                new[] { typeof(string) }, 
                null);
            
            FindNotifyingProperties();
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
           
            if (NotifyingProperties.ContainsKey(invocation.Method.Name))
                NotifyingProperties[invocation.Method.Name].ForEach(name => RaisePropertyChanged.Invoke(invocation.InvocationTarget, new object[] { name }));
        }

        void FindNotifyingProperties()
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            
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
    }
}