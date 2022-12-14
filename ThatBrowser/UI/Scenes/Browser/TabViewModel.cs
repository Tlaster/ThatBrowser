using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace ThatBrowser.UI.Scenes.Browser;

[ObservableObject]
internal partial class TabViewModel : IDisposable
{
    private readonly ICloseTabHandler _closeTabHandler;
    public WebView2 Content { get; } = new();
    [ObservableProperty]
    private string _title = string.Empty;

    public TabViewModel(string url, ICloseTabHandler closeHandler)
    {
        _closeTabHandler = closeHandler;
        Content.Source = new Uri(url);
        Content.CoreWebView2Initialized += ContentOnCoreWebView2Initialized;
    }
    
    public void Back() => Content.GoBack();
    public void Forward() => Content.GoForward();
    public void Reload() => Content.Reload();

    private void ContentOnCoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
    {
        var core = sender.CoreWebView2;
        core.Settings.IsPasswordAutosaveEnabled = true;
        core.Settings.IsGeneralAutofillEnabled = true;
        core.Settings.AreDefaultScriptDialogsEnabled = true;
        core.Settings.AreDevToolsEnabled = true;
        core.Settings.AreDefaultContextMenusEnabled = true;
        core.Settings.AreHostObjectsAllowed = true;
        core.Settings.AreBrowserAcceleratorKeysEnabled = true;
        
        core.DocumentTitleChanged += CoreOnDocumentTitleChanged;
        // core.ContextMenuRequested += CoreOnContextMenuRequested;
        core.NewWindowRequested += CoreOnNewWindowRequested;
        core.WindowCloseRequested += CoreOnWindowCloseRequested;
        core.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
        core.WebResourceRequested += CoreOnWebResourceRequested;
    }

    private void CoreOnDocumentTitleChanged(CoreWebView2 sender, object args)
    {
        Title = sender.DocumentTitle;
    }

    private void CoreOnWebResourceRequested(CoreWebView2 sender, CoreWebView2WebResourceRequestedEventArgs args)
    {
    }

    private void CoreOnWindowCloseRequested(CoreWebView2 sender, object args)
    {
        _closeTabHandler.Close(this);
    }

    private void CoreOnNewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
    {
        
    }

    private void CoreOnContextMenuRequested(CoreWebView2 sender, CoreWebView2ContextMenuRequestedEventArgs args)
    {
        args.Handled = true;
    }
    
    [RelayCommand]
    private void Close() => _closeTabHandler.Close(this);

    public void Dispose()
    {
        Content.Close();
    }
}


internal interface ICloseTabHandler
{
    void Close(TabViewModel tab);
}