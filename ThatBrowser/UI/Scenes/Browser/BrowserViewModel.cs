using System;
using System.Collections.ObjectModel;
using Windows.System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;
using ThatBrowser.UI.Hosting;

namespace ThatBrowser.UI.Scenes.Browser;

[ObservableObject]
partial class BrowserViewModel
{
    public ObservableCollection<TabViewModel> Tabs { get; } = new();

    [ObservableProperty] private TabViewModel? _selectedTab = null;
    [ObservableProperty] private string _addressBarText = string.Empty;
    private bool _isControlKeyDown = false;
    
    [RelayCommand]
    private void AddTab()
    {
        var text = AddressBarText;
        AddressBarText = string.Empty;
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        if (_isControlKeyDown)
        {
            text = $"https://www.{text}.com";
        }
        else if (text.StartsWith("www.") && text.EndsWith(".com"))
        {
            text = $"https://{text}";
        }
        else if (!text.StartsWith("http://") && !text.StartsWith("https://"))
        {
            text = $"https://www.google.com/search?q={text}";
        }

        var tab = new TabViewModel(text);
        Tabs.Add(tab);
        SelectedTab = tab;
    }

    [RelayCommand]
    private void OnSearchBoxKeyDown(KeyRoutedEventArgs e)
    {
        _isControlKeyDown = e.Key == VirtualKey.Control;
    }

    [RelayCommand]
    private void OnSearchBoxKeyUp(KeyRoutedEventArgs e)
    {
        _isControlKeyDown = false;
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
    
    [RelayCommand]
    private void ClearSelectedTab()
    {
        SelectedTab = null;
    }
}