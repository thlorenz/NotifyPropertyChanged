using System;
using System.ComponentModel;
using System.Windows;
using Castle.DynamicProxy;
using NotifyPropertyChanged.Castle.UsingPOCO;
using NotifyPropertyChanged.Castle.UsingViewModelBase;

namespace NotifyPropertyChanged.Castle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly ProxyGenerator _generator = new ProxyGenerator();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var proxy = MakeINotifyPropertyChanged<CastleViewModelThatIsPOCO>();
            var view = new MainView(proxy);
            new Window { Content = view, Topmost = true, Width = 300, Height = 200 }.Show();
        }

        static CastleViewModelUsingViewModelBase CreateClassProxyUsingViewModelBase()
        {
            return _generator.CreateClassProxy<CastleViewModelUsingViewModelBase>(
                new LogCallsInterceptor(), new ViewModelInterceptor<CastleViewModelUsingViewModelBase>());
        }

        static T MakeINotifyPropertyChanged<T>() where T : class, new()
        {
            // Generates a proxy using our Interceptor and implementing INotifyPropertyChanged
            var proxy = _generator.CreateClassProxy(
                typeof(T), 
                new[] { typeof(INotifyPropertyChanged) }, 
                ProxyGenerationOptions.Default, 
                new NotifierInterceptor()
                );

            return proxy as T;
        }
    }

    public class LogCallsInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Intercepted {0}", invocation.Method);
            invocation.Proceed();
        }
    }
}