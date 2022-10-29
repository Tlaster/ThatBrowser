using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Graphics;
using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace ThatBrowser.UI.Scenes.Settings;

internal partial class SettingsScene : ContentDialog
{
    public SettingsScene()
    {
        InitializeComponent();
    }

    private void ContactButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string url } && !string.IsNullOrEmpty(url))
        {
            Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}