using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Reflection;

namespace NotifyPropertyChanged.PostSharp
{
    /// <summary>
    /// Calls RaisePropertyChanged (needs to be implemented on declaring type) of any property with this attribute applied, when its value is changed.
    /// Optionally it also raises property changed on properties whose names are given inside the constructor.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class NotifyPropertyChangedAttribute : LocationInterceptionAspect
    {
        public MethodInfo RaisePropertyChanged;
        readonly string[] _additionalAffectedProperties;

        public NotifyPropertyChangedAttribute()
        {
        }

        /// <summary>
        /// Only use this constructor when applying this attribute on Property level, otherwise the PropertyChange for the additional properties will 
        /// be invoked anytime any other property in the view model is set.
        /// </summary>
        /// <param name="additionalAffectedProperties">The properties that are affected by the change to the property to which this attribute is applied.</param>
        public NotifyPropertyChangedAttribute(params string[] additionalAffectedProperties)
        {
            _additionalAffectedProperties = additionalAffectedProperties;
        }

        public override void CompileTimeInitialize(LocationInfo locationInfo, AspectInfo aspectInfo)
        {
            RaisePropertyChanged = locationInfo.DeclaringType.GetMethod(
                "RaisePropertyChanged", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, 
                new[] { typeof(string) }, 
                null);
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            args.ProceedSetValue();

            RaisePropertyChanged.Invoke(args.Instance, new object[] { args.Location.Name });
            
            if (_additionalAffectedProperties != null)
            {
                foreach (var affected in _additionalAffectedProperties)
                {
                    RaisePropertyChanged.Invoke(args.Instance, new object[] { affected });
                }
            }
        }
    }
}