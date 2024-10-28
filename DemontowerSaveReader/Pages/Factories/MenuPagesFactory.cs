using Avalonia.Collections;
using FluentAvaloniaTemplate.Common;
using FluentAvaloniaTemplate.Pages.Home;
using FluentAvaloniaTemplate.Pages.Info;
using Microsoft.Extensions.DependencyInjection;

namespace FluentAvaloniaTemplate.Pages.Factories;
public class MenuPagesFactory(IServiceProvider serviceProvider)
{
    private IAvaloniaList<ProjectPageBase> MenuPages { get; set; } = new AvaloniaList<ProjectPageBase>();
    private IAvaloniaList<ProjectPageBase> FooterPages { get; set; } = new AvaloniaList<ProjectPageBase>();
    
    public IAvaloniaList<ProjectPageBase> CreateMenuPages()
    {
        List<ProjectPageBase> menuPages =
        [
            serviceProvider.GetRequiredService<HomePageViewModel>(),
        ];

        MenuPages = new AvaloniaList<ProjectPageBase>(menuPages.OrderBy(x => x.Index).ThenBy(x => x.DisplayName));

        return MenuPages;
    }

    public IAvaloniaList<ProjectPageBase> CreateFooterPages()
    {
        List<ProjectPageBase> footerPages =
        [
            serviceProvider.GetRequiredService<InfoPageViewModel>(),
        ];

        FooterPages = new AvaloniaList<ProjectPageBase>(footerPages.OrderBy(x => x.Index).ThenBy(x => x.DisplayName));
        
        return FooterPages;
    }
    
}
