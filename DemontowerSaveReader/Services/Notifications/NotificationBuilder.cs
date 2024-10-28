using FluentAvalonia.UI.Controls;
using FluentAvaloniaTemplate.Services;

namespace FluentAvaloniaTemplate.Models;

public class NotificationBuilder
{
    private string _title = string.Empty;
    private string _message = string.Empty;
    private InfoBarSeverity _type = InfoBarSeverity.Informational;
    private TimeSpan? _timeout = null;

    public NotificationBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public NotificationBuilder WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public NotificationBuilder OfType(InfoBarSeverity type)
    {
        _type = type;
        return this;
    }

    public NotificationBuilder WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    public void Show()
    {
        NotificationService.Instance.DisplayNotification(_title, _message, _type, _timeout);
    }
}