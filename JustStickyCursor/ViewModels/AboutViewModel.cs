using System.Media;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using JustStickyCursor.Helpers;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

namespace JustStickyCursor.ViewModels;

public partial class AboutViewModel : ObservableRecipient
{

    [ObservableProperty]
    private string _versionDescription;

    public AboutViewModel()
    {
        _versionDescription = GetVersionDescription();
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMsix)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public void Splort(object sender, RoutedEventArgs e)
    {
        var installedPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

        var soundFile = Path.Join(installedPath, "Assets", "splort.wav");

        var player = new System.Media.SoundPlayer(soundFile);
        player.Play();
    }

    public void SplortButBackwards(object sender, RoutedEventArgs e)
    {
        var installedPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

        var soundFile = Path.Join(installedPath, "Assets", "trolps.wav");

        var player = new System.Media.SoundPlayer(soundFile);
        player.Play();
    }
}
