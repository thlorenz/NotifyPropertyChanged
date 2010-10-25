using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using LinFu.AOP.Interfaces;
using LinFu.Proxy;
using LinFu.Proxy.Interfaces;

namespace NotifyPropertyChanged.LinFu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = new MainViewModel();
            var proxy = CreateProxy(viewModel);
            proxy.InitializeCommand();
            var view = new MainView(proxy);
            new Window { Content = view, Topmost = true, Width = 300, Height = 200 }.Show();
        }

        MainViewModel CreateProxy(MainViewModel mainViewModel)
        {
            var proxyFactory = new ProxyFactory();
            return proxyFactory.CreateProxy<MainViewModel>(new MainViewModelWrapper(mainViewModel));
        }

    }

    public class MainViewModelInterceptor : IInterceptor
    {
        public MainViewModelInterceptor(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            RaisePropertyChanged = _mainViewModel.GetType().GetMethod(
                "RaisePropertyChanged", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, 
                new[] { typeof(string) }, 
                null);
        }

        public MethodInfo RaisePropertyChanged;
        readonly MainViewModel _mainViewModel;

        public object Intercept(IInvocationInfo info)
        {
            Console.WriteLine("Intercepted: {0}", info.TargetMethod);
            return info.TargetMethod.Invoke(_mainViewModel, info.Arguments);
        }

    }

    public class MainViewModelWrapper : IInvokeWrapper
    {

        public MainViewModelWrapper(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            RaisePropertyChanged = _mainViewModel.GetType().GetMethod(
                "RaisePropertyChanged", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, 
                new[] { typeof(string) }, 
                null);
        }

        public MethodInfo RaisePropertyChanged;
        readonly MainViewModel _mainViewModel;

        public void BeforeInvoke(IInvocationInfo info)
        {
            Console.WriteLine("BeforeInvoke {0}", info.TargetMethod);
        }

        public void AfterInvoke(IInvocationInfo info, object returnValue)
        {
            Console.WriteLine("AfterInvoke {0}", info.TargetMethod);
            RaisePropertyChanged.Invoke(_mainViewModel, new object[] { "Counter" });
        }

        public object DoInvoke(IInvocationInfo info)
        {
            Console.WriteLine("DoInvoke {0}", info.TargetMethod);
          return info.TargetMethod.Invoke(_mainViewModel, info.Arguments);
        }
    }
}