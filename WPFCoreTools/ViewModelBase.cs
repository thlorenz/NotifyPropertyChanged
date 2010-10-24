using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WPFCoreTools
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged<TValue>(Expression<Func<TValue>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression != null)
            {
                RaisePropertyChanged(memberExpression.Member.Name);
            }
        }
    }
}