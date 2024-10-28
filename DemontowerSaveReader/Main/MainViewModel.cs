using System;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvaloniaTemplate.Common;
using FluentAvaloniaTemplate.Models;
using FluentAvaloniaTemplate.Pages.Factories;
using FluentAvaloniaTemplate.Services;

namespace FluentAvaloniaTemplate.Main;

public partial class MainViewModel : ObservableObject
{
    public IAvaloniaList<ProjectPageBase> MenuPages { get; }
    public IAvaloniaList<ProjectPageBase> FooterPages { get; }
    public PageNavigationService NavigationService { get; set; }
    
    [ObservableProperty] private ProjectPageBase? _activePage;
    [ObservableProperty] private AvaloniaList<NotificationModel> _notifications = [];

    public MainViewModel(PageNavigationService navSvc, NotificationService notificationService, MenuPagesFactory menuPagesFactory)
    {
        NavigationService = navSvc;
        
        Notifications = notificationService.Notifications;
            
        MenuPages = menuPagesFactory.CreateMenuPages();
        FooterPages = menuPagesFactory.CreateFooterPages();
        
        var allPages = MenuPages.Concat(FooterPages).ToList();
        
        navSvc.NavigationRequested += t =>
        {
            var page = allPages.FirstOrDefault(x => x.GetType() == t);
            if (page is null || _activePage?.GetType() == t) return;
            ActivePage = page;
        };
        
        ActivePage = MenuPages.FirstOrDefault();
    }
}