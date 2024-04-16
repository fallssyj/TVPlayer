using DryIoc;
using FlyleafLib;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Windows;
using TVPlayer.Common;
using TVPlayer.ViewModels;
using TVPlayer.ViewModels.Dialog;
using TVPlayer.Views;
using TVPlayer.Views.Dialogs;

namespace TVPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static IDialogService dialog { get; set; }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>();


            containerRegistry.RegisterForNavigation<ChannelView, ChannelViewModel>();
            containerRegistry.RegisterForNavigation<FavoriteView, FavoriteViewModel>();
            containerRegistry.RegisterForNavigation<ConfigurationView, ConfigurationViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Engine.Start(new EngineConfig()
            {
                FFmpegPath = ":FFmpeg",
                FFmpegDevices = true,    // Prevents loading avdevice/avfilter dll files. Enable it only if you plan to use dshow/gdigrab etc.

#if RELEASE
                FFmpegLogLevel      = FFmpegLogLevel.Quiet,
                LogLevel            = LogLevel.Quiet,

#else
                FFmpegLogLevel = FFmpegLogLevel.Warning,
                LogLevel = LogLevel.Debug,
                LogOutput = ":debug",
                //LogOutput         = ":console",
                //LogOutput         = @"C:\Flyleaf\Logs\flyleaf.log",                
#endif

                //PluginsPath       = @"C:\Flyleaf\Plugins",
                UIRefresh = false,    // Required for Activity, BufferedDuration, Stats in combination with Config.Player.Stats = true
                UIRefreshInterval = 250,      // How often (in ms) to notify the UI
                UICurTimePerSecond = true,     // Whether to notify UI for CurTime only when it's second changed or by UIRefreshInterval
            });
        }

        protected override void OnInitialized()
        {
            dialog = Container.Resolve<IDialogService>(); ;

            var service = Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();

            base.OnInitialized();
        }
        public static void MessageBox(IDialogParameters parameters)
        {
            dialog.ShowDialog("MessageBoxView", parameters, null);
        }
    }
}
