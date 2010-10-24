namespace NotifyPropertyChanged.PostSharp
{
    // Below is the notify property changed impementation that comes with the PostSharp samples, but it uses features of the professional PostSharp version.
    // It has the advantage over the alternative approach, that it can add the PropertyChanged method (thus encapsulating the entire INotifyPropertyChanged behavior).
    // I am trying to stick with the community version though - see NotifyPropertyChangedAttribute
    
    //[Serializable]
    //[IntroduceInterface(typeof(INotifyPropertyChanged), OverrideAction = InterfaceOverrideAction.Ignore)]
    //[MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Strict)]
    //public sealed class NotifyPropertyChangedProfessionalOnlyAttribute : InstanceLevelAspect, INotifyPropertyChanged
    //{
    //    [ImportMember("OnPropertyChanged", IsRequired = false, Order = ImportMemberOrder.AfterIntroductions)]
    //    public Action<string> OnPropertyChangedMethod;

    //    [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore)]
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    [IntroduceMember(Visibility = Visibility.Family, IsVirtual = true, OverrideAction = MemberOverrideAction.Ignore)]
    //    public void OnPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(Instance, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    [OnLocationSetValueAdvice]
    //    [MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
    //    public void OnPropertySet(LocationInterceptionArgs args)
    //    {
    //        // Don't go further if the new value is equal to the old one.
    //        // (Possibly use object.Equals here).
    //        if (args.Value == args.GetCurrentValue())
    //            return;

    //        // Actually sets the value.
    //        args.ProceedSetValue();

    //        OnPropertyChangedMethod.Invoke(args.Location.Name);
    //    }
    //}
}