using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

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
}