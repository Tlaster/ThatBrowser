using Microsoft.UI.Xaml;
using ThatBrowser.UI.Hosting;

namespace ThatBrowser;

partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        new RootWindow().Activate();
    }

}