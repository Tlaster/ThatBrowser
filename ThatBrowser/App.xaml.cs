using Microsoft.UI.Xaml;
using ThatBrowser.UI.Hosting;

namespace ThatBrowser;

partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    internal static RootWindow RootWindow { get; } = new();

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        RootWindow.Activate();
    }

}