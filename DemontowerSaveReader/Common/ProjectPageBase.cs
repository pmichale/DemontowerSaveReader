﻿using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;

namespace FluentAvaloniaTemplate.Common;

public abstract partial class ProjectPageBase(string displayName, MaterialIconKind icon, int index = 0) : ObservableObject
{
    [ObservableProperty] private string _displayName = displayName;

    [ObservableProperty] private MaterialIconKind _icon = icon;

    [ObservableProperty] private int _index = index;
}