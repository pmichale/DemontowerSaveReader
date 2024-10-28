using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FluentAvaloniaTemplate.Main;
using FluentAvaloniaTemplate.Pages.Factories;
using FluentAvaloniaTemplate.Pages.Home;
using FluentAvaloniaTemplate.Pages.Info;
using FluentAvaloniaTemplate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FluentAvaloniaTemplate;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    public Window? MainWindow;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _serviceProvider = ConfigureServices();

            var viewLocator = Current?.DataTemplates.First(x => x is ViewLocator);

            var mainVm = _serviceProvider?.GetRequiredService<MainViewModel>();

            MainWindow = viewLocator?.Build(mainVm) as Window;

            desktop.MainWindow = MainWindow;

            // Instantiate FileService after the main window is created
            var fileService = _serviceProvider?.GetRequiredService<IFileService>();
            if (fileService != null && MainWindow is not null)
            {
                fileService.Initialize(MainWindow);
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<IFileService>(new FileService());
        services.AddSingleton<PageNavigationService>();
        services.AddSingleton(_ => NotificationService.Instance);
        services.AddSingleton<MenuPagesFactory>();
        
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<HomePageViewModel>();
        services.AddSingleton<InfoPageViewModel>();
        
        return services.BuildServiceProvider();
    }
}