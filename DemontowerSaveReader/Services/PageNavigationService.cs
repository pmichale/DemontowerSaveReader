using FluentAvaloniaTemplate.Common;

namespace FluentAvaloniaTemplate.Services;

public class PageNavigationService
{
    public Action<Type>? NavigationRequested {  get; set; }

    public void RequestNavigation<T>() where T : ProjectPageBase
    {
        NavigationRequested?.Invoke(typeof(T));
    }
}

