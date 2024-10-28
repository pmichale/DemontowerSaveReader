using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.UI.Controls;

namespace FluentAvaloniaTemplate.Models;

public partial class NotificationModel : ObservableObject
{
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _message = string.Empty;
    [ObservableProperty] private bool _isOpen;
    [ObservableProperty] private InfoBarSeverity _type = InfoBarSeverity.Informational;
    
    partial void OnIsOpenChanged(bool value)
    {
        if (!value)
        {
            NotificationClosed?.Invoke(this);
        }
    }

    public event Action<NotificationModel>? NotificationClosed;
}