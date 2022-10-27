using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ThatBrowser.UI.Scenes.Browser;

sealed partial class BrowserScene : UserControl
{
    public BrowserScene()
    {
        this.InitializeComponent();
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
        RightPaddingColumn.Width = new GridLength(safeArea.RightTitleBar / safeArea.ScaleAdjustment);
        List<Windows.Graphics.RectInt32> dragRectsList = new();
        Windows.Graphics.RectInt32 dragRect1;
        var draggableArea1Point = TransformToVisual(DraggableArea1).TransformPoint(new Point());
        dragRect1.X = Convert.ToInt32(Math.Abs(draggableArea1Point.X) * safeArea.ScaleAdjustment);
        dragRect1.Y = Convert.ToInt32(Math.Abs(draggableArea1Point.Y) * safeArea.ScaleAdjustment);
        dragRect1.Height = (int)(DraggableArea1.ActualHeight * safeArea.ScaleAdjustment);
        dragRect1.Width = (int)(DraggableArea1.ActualWidth * safeArea.ScaleAdjustment);
        dragRectsList.Add(dragRect1);
        
        // Windows.Graphics.RectInt32 dragRect2;
        // var draggableArea2Point = TransformToVisual(DraggableArea2).TransformPoint(new Point());
        // dragRect2.X = Convert.ToInt32(Math.Abs(draggableArea2Point.X) * safeArea.ScaleAdjustment);
        // dragRect2.Y = Convert.ToInt32(Math.Abs(draggableArea2Point.Y) * safeArea.ScaleAdjustment);
        // dragRect2.Height = (int)(DraggableArea2.ActualHeight * safeArea.ScaleAdjustment);
        // dragRect2.Width = (int)(DraggableArea2.ActualWidth * safeArea.ScaleAdjustment);
        // dragRectsList.Add(dragRect2);

        App.RootWindow.SetDragArea(dragRectsList.ToArray());
    }
    
    private void BrowserScene_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Menu)
        {
            e.Handled = true;
            var safeArea = App.RootWindow.GetSafeArea();
            Windows.Graphics.RectInt32 dragRect;
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
}