using System;
using System.Windows;
using System.Windows.Navigation;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Stores;
using SecretMessage.WPF.ViewModels;

namespace SecretMessage.WPF
{ 
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        
        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, service) =>
                {
                    string firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY") 
                                            ?? throw new ArgumentNullException("FIREBASE_API_KEY");

                    service.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey)));

                    service.AddSingleton<NavigationStore>();
                    service.AddSingleton<ModalNavigationStore>();
                    service.AddSingleton<AuthenticationStore>();

                    service.AddSingleton<NavigationService<RegisterViewModel>>(
                        (services) => new NavigationService<RegisterViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new RegisterViewModel(
                                services.GetRequiredService<FirebaseAuthProvider>(),
                                services.GetRequiredService<NavigationService<LoginViewModel>>())));

                    service.AddSingleton<NavigationService<LoginViewModel>>(
                        (services) => new NavigationService<LoginViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new LoginViewModel(
                                services.GetRequiredService<AuthenticationStore>(),
                                services.GetRequiredService<NavigationService<RegisterViewModel>>(),
                                services.GetRequiredService<NavigationService<HomeViewModel>>())));

                    service.AddSingleton<NavigationService<HomeViewModel>>(
                        (services) => new NavigationService<HomeViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new HomeViewModel(
                                services.GetRequiredService<AuthenticationStore>())));

                    service.AddSingleton<MainViewModel>();

                    service.AddSingleton<MainWindow>((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}