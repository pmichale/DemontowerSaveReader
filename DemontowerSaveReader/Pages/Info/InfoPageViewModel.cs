using CommunityToolkit.Mvvm.Input;
using FluentAvaloniaTemplate.Common;
using FluentAvaloniaTemplate.Utilities;
using Material.Icons;

namespace FluentAvaloniaTemplate.Pages.Info;

public partial class InfoPageViewModel() : ProjectPageBase("Info", MaterialIconKind.InformationOutline, 1)
{
    [RelayCommand]
    private static void OpenUrl(string url) => UrlUtilities.OpenUrl(url);
}