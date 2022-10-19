using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThatBrowser.UI.Hosting;

namespace ThatBrowser.UI.Scenes.Browser;

[ObservableObject]
partial class BrowserViewModel
{
    public ObservableCollection<TabViewModel> Tabs { get; } = new();

    [ObservableProperty] private TabViewModel? _selectedTab = null;
    [ObservableProperty] private string _addressBarText = string.Empty;
    
    [RelayCommand]
    private void AddTab()
    {
        var tab = new TabViewModel(AddressBarText);
        Tabs.Add(tab);
        SelectedTab = tab;
        AddressBarText = string.Empty;
    }
    
    [RelayCommand]
    private void Back()
    {
        SelectedTab?.Back();
    }
    
    [RelayCommand]
    private void Forward()
    {
        SelectedTab?.Forward();
    }
    
    [RelayCommand]
    private void Reload()
    {
        SelectedTab?.Reload();
    }
}