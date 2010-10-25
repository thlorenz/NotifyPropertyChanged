using System;
using System.Collections.Generic;

namespace NotifyPropertyChanged.Castle
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class NotifyPropertyChangedAttribute : Attribute
    {
        public readonly IEnumerable<string> AdditionalAffectedProperties = new string[0];

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
            AdditionalAffectedProperties = additionalAffectedProperties;
        }
    }
}