using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using FluentAvaloniaTemplate.Common;
using FluentAvaloniaTemplate.Services;
using Material.Icons;

namespace FluentAvaloniaTemplate.Pages.Home;

public partial class HomePageViewModel(IFileService fileService) : ProjectPageBase("Home", MaterialIconKind.Home, 0)
{
    [ObservableProperty] private string _filePath = string.Empty;
    
    [ObservableProperty] private string _firstAltar = string.Empty;
    [ObservableProperty] private string _secondAltar = string.Empty;
    [ObservableProperty] private string _thirdAltar = string.Empty;
    [ObservableProperty] private string _fourthAltar = string.Empty;

    private float? _firstAltarValue;
    private float? _secondAltarValue;
    private float? _thirdAltarValue;
    private float? _fourthAltarValue;

    [RelayCommand]
    private async Task AutoDetectSaveFile()
    {
        string path;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                "AppData", "LocalLow", "Infinite Fall", "Night in the Woods");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), 
                "Library", "Application Support", "Infinite Fall", "Night in the Woods");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Use $XDG_CONFIG_HOME or fallback to a common path if it's not set
            var configHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? 
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config");
        
            path = Path.Combine(configHome, "unity3d", "Infinite Fall", "Night in the Woods");
        }
        else
        {
            throw new NotSupportedException("Can't get OS type or OS not supported.");
        }

        var saveFilePath = Path.Combine(path, "player.dat");
        
        if (File.Exists(saveFilePath))
        {
            FilePath = saveFilePath;
            await ReadFileAsync(FilePath);
            AssignAltarValues();
        }
        else
        {
            NotificationService.CreateNotification()
                .WithTitle("Save file not found")
                .WithMessage(saveFilePath + "\n\nLocate the file manually.")
                .OfType(InfoBarSeverity.Error)
                .Show();
        }
    }

    [RelayCommand]
    private async Task OpenFile()
    {
        var file = await fileService.OpenFileAsync();
        if (file is null) return;
        FilePath = file.Path.LocalPath;
        
        await ReadFileAsync(FilePath);
        AssignAltarValues();
    }

    private void AssignAltarValues()
    {
        FirstAltar = _firstAltarValue is null ? "Not Found" : $"{_firstAltarValue}";
        SecondAltar = _secondAltarValue is null ? "Not Found" : $"{_secondAltarValue}";
        ThirdAltar = _thirdAltarValue is null ? "Not Found" : $"{_thirdAltarValue}";
        FourthAltar = _fourthAltarValue is null ? "Not Found" : $"{_fourthAltarValue}";

        if (_firstAltarValue is null || _secondAltarValue is null || _thirdAltarValue is null ||
            _fourthAltarValue is null)
        {
            NotificationService.CreateNotification()
                .WithTitle("Conversion Failed")
                .WithMessage("Make sure the file is a Night In The Woods save file.")
                .OfType(InfoBarSeverity.Error)
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Show();
        }
        else
        {
            NotificationService.CreateNotification()
                .WithTitle("Conversion Successful")
                .WithMessage("The file was successfully converted.")
                .OfType(InfoBarSeverity.Success)
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Show();
        }
    }

    private async Task ReadFileAsync(string file)
    {
        byte[] fileBytes;
        using (var reader = new BinaryReader(File.OpenRead(file)))
        {
            fileBytes = await Task.Run(() => reader.ReadBytes((int)reader.BaseStream.Length));
        }
        
        var code0 = "demontower_altar_code_0"u8.ToArray();
        var code1 = "demontower_altar_code_1"u8.ToArray();
        var code2 = "demontower_altar_code_2"u8.ToArray();
        var code3 = "demontower_altar_code_3"u8.ToArray();
        
        var positions0 = ExtractAltarBytes(fileBytes, code0);
        var positions1 = ExtractAltarBytes(fileBytes, code1);
        var positions2 = ExtractAltarBytes(fileBytes, code2);
        var positions3 = ExtractAltarBytes(fileBytes, code3);
        
        _firstAltarValue = HexToLittleEndianFloat(positions0);
        _secondAltarValue = HexToLittleEndianFloat(positions1);
        _thirdAltarValue = HexToLittleEndianFloat(positions2);
        _fourthAltarValue = HexToLittleEndianFloat(positions3);
    }

    private static float? HexToLittleEndianFloat(byte[]? byteArray)
    {
        if (byteArray == null || byteArray.Length < 4) return null;

        var result = BitConverter.ToSingle(byteArray, 0);

        return result;
    }
    
    private static byte[]? ExtractAltarBytes(byte[] fileBytes, byte[] pattern)
    {
        var positions = FindHexPattern(fileBytes, pattern);
        var offset = pattern.Length;

        foreach (var position in positions)
        {
            var firstBytePosition = position + offset;
            var secondBytePosition = firstBytePosition + 1;
            var thirdBytePosition = firstBytePosition + 2;
            var fourthBytePosition = firstBytePosition + 3;

            if (firstBytePosition < fileBytes.Length && 
                secondBytePosition < fileBytes.Length && 
                thirdBytePosition < fileBytes.Length && 
                fourthBytePosition < fileBytes.Length)
            {
                return
                [
                    fileBytes[firstBytePosition], 
                    fileBytes[secondBytePosition], 
                    fileBytes[thirdBytePosition], 
                    fileBytes[fourthBytePosition]
                ];
            }
        }
    
        return null;
    }
    
    private static List<int> FindHexPattern(byte[] fileBytes, byte[] hexPattern)
    {
        List<int> positions = [];
        var fileLength = fileBytes.Length;
        var patternLength = hexPattern.Length;

        for (var i = 0; i <= fileLength - patternLength; i++)
        {
            var found = true;
            for (var j = 0; j < patternLength; j++)
            {
                if (fileBytes[i + j] == hexPattern[j]) continue;
                found = false;
                break;
            }

            if (found)
            {
                positions.Add(i);
            }
        }

        return positions;
    }
}