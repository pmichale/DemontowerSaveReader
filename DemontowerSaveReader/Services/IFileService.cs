using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace FluentAvaloniaTemplate.Services;

public interface IFileService
{
    public void Initialize(Window target);
    public Task<IStorageFile?> OpenFileAsync();
}