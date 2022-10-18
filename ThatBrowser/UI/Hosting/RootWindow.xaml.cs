using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using ThatBrowser.UI.Controls;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ThatBrowser.UI.Hosting;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
sealed partial class RootWindow : ModernWindow
{
    private readonly AppWindow _window;

    public RootWindow()
    {
        this.InitializeComponent();
        TrySetSystemBackdrop();

        _window = GetAppWindowForCurrentWindow();
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = _window.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
        }
        else
        {
            // AppTitleBar.Visibility = Visibility.Collapsed;
        }
    }

    private void WebViewOnCoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
    {
        var core = sender.CoreWebView2;
        core.ContextMenuRequested += CoreWebView2OnContextMenuRequested;
        
    }

    private void CoreWebView2OnContextMenuRequested(CoreWebView2 sender, CoreWebView2ContextMenuRequestedEventArgs args)
    {
    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }
}