using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Graphics;
using Windows.System;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using ThatBrowser.UI.Scenes.Settings;
using ThatBrowser.UI.Scenes.Spotlight;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ThatBrowser.UI.Scenes.Browser;

internal sealed partial class BrowserScene : UserControl
{
    private BrowserViewModel ViewModel => (BrowserViewModel)DataContext;
    public BrowserScene()
    {
        InitializeComponent();
    }

    private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        RootSplitView.IsPaneOpen = true;
    }

    private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
        RootSplitView.IsPaneOpen = false;
    }

    private void OnSafeAreaElementEvent(object sender, RoutedEventArgs e)
    {
        UpdateSafeArea();
    }

    private void UpdateSafeArea()
    {
        var safeArea = App.RootWindow.GetSafeArea();
        List<RectInt32> dragRectsList = new();
        RectInt32 dragRect1;
        var draggableArea1Point = TransformToVisual(DraggableArea1).TransformPoint(new Point());
        dragRect1.X = Convert.ToInt32(Math.Abs(draggableArea1Point.X) * safeArea.ScaleAdjustment);
        dragRect1.Y = Convert.ToInt32(Math.Abs(draggableArea1Point.Y) * safeArea.ScaleAdjustment);
        dragRect1.Height = (int)(DraggableArea1.ActualHeight * safeArea.ScaleAdjustment);
        dragRect1.Width = (int)(DraggableArea1.ActualWidth * safeArea.ScaleAdjustment);
        dragRectsList.Add(dragRect1);

        RectInt32 dragRect2;
        var draggableArea2Point = TransformToVisual(DraggableArea2).TransformPoint(new Point());
        dragRect2.X = Convert.ToInt32(Math.Abs(draggableArea2Point.X) * safeArea.ScaleAdjustment);
        dragRect2.Y = Convert.ToInt32(Math.Abs(draggableArea2Point.Y) * safeArea.ScaleAdjustment);
        dragRect2.Height = (int)(DraggableArea2.ActualHeight * safeArea.ScaleAdjustment);
        dragRect2.Width = (int)(DraggableArea2.ActualWidth * safeArea.ScaleAdjustment);
        dragRectsList.Add(dragRect2);

        App.RootWindow.SetDragArea(dragRectsList.ToArray());
    }

    private void BrowserScene_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Menu)
        {
            e.Handled = true;
            var safeArea = App.RootWindow.GetSafeArea();
            RectInt32 dragRect;
            dragRect.X = 0;
            dragRect.Y = 0;
            dragRect.Height = (int)(ActualHeight * safeArea.ScaleAdjustment);
            dragRect.Width = (int)(ActualWidth * safeArea.ScaleAdjustment);
            App.RootWindow.SetDragArea(new[] { dragRect });
        }
    }

    private void BrowserScene_OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Menu)
        {
            e.Handled = true;
            UpdateSafeArea();
        }
    }

    private void Settings_Clicked(object sender, RoutedEventArgs e)
    {
        new SettingsScene
        {
            XamlRoot = this.XamlRoot
        }.ShowAsync();
    }

    private void AppBar_OnOpened(object? sender, object e)
    {
        App.RootWindow.SetDragArea(new[] { new RectInt32() });
    }

    private void AppBar_OnClosed(object? sender, object e)
    {
        UpdateSafeArea();
    }

    private void NewTabKeyboardAccelerator_OnInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        new SpotlightDialog
        {
            XamlRoot = this.XamlRoot
        }.ShowAsync();
    }

    private void BrowserScene_OnLoaded(object sender, RoutedEventArgs e)
    {
        MainAutoSuggestBox.Focus(FocusState.Programmatic);
    }
}