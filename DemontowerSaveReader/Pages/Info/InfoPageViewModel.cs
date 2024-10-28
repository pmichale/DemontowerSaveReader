using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using FluentAvaloniaTemplate.Common;
using FluentAvaloniaTemplate.Services;
using Material.Icons;

namespace FluentAvaloniaTemplate.Pages.Info;

public partial class InfoPageViewModel() : ProjectPageBase("Info", MaterialIconKind.InformationOutline, 1)
{
    [ObservableProperty] private string _notificationTitle = "Test Notification";
    [ObservableProperty] private string _notificationMessage = "This is a test notification.";

    [RelayCommand]
    private void AddNotification()
    {
        NotificationService.CreateNotification()
            .WithTitle(NotificationTitle)
            .WithMessage(NotificationMessage)
            .OfType(InfoBarSeverity.Success)
            .WithTimeout(TimeSpan.FromSeconds(5))
            .Show();
    }
}