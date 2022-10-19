using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Graphics;
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

    private AppWindow GetAppWindowForCurrentWindow()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    [DllImport("Shcore.dll", SetLastError = true)]
    private static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    internal enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    private double GetScaleAdjustment()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        var result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out var dpiX, out var _);
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    public SafeArea GetSafeArea()
    {
        var scaleAdjustment = GetScaleAdjustment();
        if (AppWindowTitleBar.IsCustomizationSupported()
            && _window.TitleBar.ExtendsContentIntoTitleBar)
        {
            return new SafeArea(_window.TitleBar.LeftInset, _window.TitleBar.RightInset, scaleAdjustment);
        }
        else
        {
            return new SafeArea(0, 0, scaleAdjustment);
        }
    }

    public void SetDragArea(RectInt32[] areas)
    {
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            _window.TitleBar.SetDragRectangles(areas);
        }
    }

    public record SafeArea(int LeftTitleBar, int RightTitleBar, double ScaleAdjustment);
}