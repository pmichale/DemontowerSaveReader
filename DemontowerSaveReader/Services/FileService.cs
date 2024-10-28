using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace FluentAvaloniaTemplate.Services;

public class FileService : IFileService
{
    private Window? _target;

    public void Initialize(Window target)
    {
        _target = target ?? throw new NullReferenceException("Target is null.");
    }

    public async Task<IStorageFile?> OpenFileAsync()
    {
        var file = await _target?.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            AllowMultiple = false
        })!;

        return file.Count >= 1 ? file[0] : null;
    }
}