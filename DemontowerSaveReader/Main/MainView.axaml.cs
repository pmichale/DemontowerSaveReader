using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvaloniaTemplate.Common;
using FluentAvaloniaTemplate.Services;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Material.Icons.Avalonia;

namespace FluentAvaloniaTemplate.Main;

public partial class MainView : AppWindow
{
    private PageNavigationService? _navigationService;
    private MainViewModel? _mainVm;
    
    public MainView()
    {
        InitializeComponent();
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (DataContext is not MainViewModel mainVm) return;
        _mainVm = mainVm;
        
        _navigationService = _mainVm.NavigationService;
        
        var menuItems = new List<NavigationViewItemBase>(_mainVm.MenuPages.Count);
        var footerItems = new List<NavigationViewItemBase>(_mainVm.FooterPages.Count);

        Dispatcher.UIThread.Post(() =>
        {
            var converter = new MaterialIconKindToGeometryConverter();
            
            foreach (var page in _mainVm.MenuPages)
            {
                var geometry = converter.Convert(page.Icon, typeof(Geometry), null, CultureInfo.InvariantCulture) as Geometry;
                
                var icon = new PathIconSource() { Data = geometry };

                var navigationItem = new NavigationViewItem
                {
                    IconSource = icon,
                    Content = page.DisplayName,
                    Tag = page
                };
                
                menuItems.Add(navigationItem);
            }
            
            foreach (var page in mainVm.FooterPages)
            {
                var geometry = converter.Convert(page.Icon, typeof(Geometry), null, CultureInfo.InvariantCulture) as Geometry;
                
                var icon = new PathIconSource() { Data = geometry };

                var navigationItem = new NavigationViewItem
                {
                    IconSource = icon,
                    Content = page.DisplayName,
                    Tag = page
                };
                
                footerItems.Add(navigationItem);
            }
            
            NavView.MenuItemsSource = menuItems;
            NavView.FooterMenuItemsSource = footerItems;
            
            NavView.SelectedItem = menuItems[0];
            
            NavView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
            
            NavView.ItemInvoked += OnNavigationViewItemInvoked;
        });
    }
    
    private void OnNavigationViewItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
    {
        if (e.InvokedItemContainer is not NavigationViewItem navigationItem) return;
        
        var tag = navigationItem.Tag;
        if (tag == null) return;
        
        var tagType = tag.GetType();
        if (!typeof(ProjectPageBase).IsAssignableFrom(tagType)) return;
        
        var method = _navigationService?.GetType()?.GetMethod("RequestNavigation")?.MakeGenericMethod(tagType);
        method?.Invoke(_navigationService, null);
    }
}