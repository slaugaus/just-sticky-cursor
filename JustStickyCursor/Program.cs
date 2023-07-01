using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using System.Windows;

namespace JustStickyCursor;

// Single-instancing code by Jingwei Zhang at https://blogs.windows.com/windowsdeveloper/2022/01/28/making-the-app-single-instanced-part-3/
class Program
{
    [STAThread]
    static async Task<int> Main(string[] args)
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();
        // Only start the real app if it's not already running.
        var isRedirect = await DecideRedirection();
        if (!isRedirect)
        {
            // TODO this is probably where you can async start the functional part?

            // Presumably, this is what the default main method looks like
            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(
                    DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                new App();
            });
        }
        else
        {
            // TODO find and focus the right window
        }

        return 0;
    }

    /// <summary>
    /// Determines whether another instance of the app is already running,
    /// then returns that.
    /// </summary>
    private static async Task<bool> DecideRedirection()
    {
        var isRedirect = false;

        var args = AppInstance.GetCurrent().GetActivatedEventArgs();
        var kind = args.Kind;

        var keyInstance = AppInstance.FindOrRegisterForKey("randomKey");

        if (keyInstance.IsCurrent)
        {
            keyInstance.Activated += OnActivated;
        }
        else
        {
            isRedirect = true;
            await keyInstance.RedirectActivationToAsync(args);
        }

        return isRedirect;
    }

    /// <summary>
    /// I'm not sure what this does tbh. Is it a stub?
    /// </summary>
    private static void OnActivated(object? sender, AppActivationArguments args)
    {
        var kind = args.Kind;
    }
}