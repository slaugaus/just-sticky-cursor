using JustStickyCursor.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace JustStickyCursor.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }
}
