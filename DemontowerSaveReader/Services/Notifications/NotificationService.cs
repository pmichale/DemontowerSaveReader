using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvaloniaTemplate.Models;
using FluentAvalonia.UI.Controls;

namespace FluentAvaloniaTemplate.Services;

public partial class NotificationService : ObservableObject
{
    private static NotificationService? _instance;

    public static NotificationService Instance => _instance ??= new NotificationService();

    [ObservableProperty] private AvaloniaList<NotificationModel> _notifications = [];

    private NotificationService()
    {
        // Private constructor to enforce singleton pattern
    }
    
    public static NotificationBuilder CreateNotification()
    {
        return new NotificationBuilder();
    }

    public void DisplayNotification(string title, string message, InfoBarSeverity type = InfoBarSeverity.Informational, TimeSpan? timeout = null)
    {
        var notification = new NotificationModel
        {
            Title = title,
            Message = message,
            Type = type,
            IsOpen = true
        };

        notification.NotificationClosed += RemoveNotification;

        Notifications.Add(notification);
        
        if (timeout.HasValue)
        {
            DismissAfterTimeout(notification, timeout.Value);
        }
    }

    private void RemoveNotification(NotificationModel notification)
    {
        Notifications.Remove(notification);
    }
    
    private async void DismissAfterTimeout(NotificationModel notification, TimeSpan timeout)
    {
        await Task.Delay(timeout);

        if (notification.IsOpen)
        {
            notification.IsOpen = false;
        }
    }
}
