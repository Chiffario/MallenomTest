using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using MallenomTest.Client.Api;
using MallenomTest.Client.Api.Interfaces;
using MallenomTest.Client.Services;
using MallenomTest.Client.Services.Interfaces;
using MallenomTest.Client.ViewModels;
using MallenomTest.Client.Views;
using Microsoft.Extensions.DependencyInjection;
namespace MallenomTest.Client;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            var mainViewModel = new MainViewModel();
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            desktop.MainWindow = mainWindow;
            
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddSingleton<IImageApiProvider, ImageApiProvider>();
            serviceCollection.AddTransient<MainViewModel>();
            serviceCollection.AddSingleton<HttpClient>();
            serviceCollection.AddSingleton<IFilesService>(x => new FilesService(desktop.MainWindow));
            
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    public new static App? Current => Application.Current as App;
    
    public IServiceProvider? ServiceProvider { get; private set; }
}