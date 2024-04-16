using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;
using TVPlayer.Common;
using TVPlayer.Common.Events;
using TVPlayer.Common.Extensions;
using TVPlayer.Common.Models;
using TVPlayer.Common.Utils;

namespace TVPlayer.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        private string _title = "TV Player";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }




        private ObservableCollection<MenuBar> _menuBars;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _menuBars; }
            set { _menuBars = value; RaisePropertyChanged(); }
        }

        private Config _configs;

        public Config Configs
        {
            get { return _configs; }
            set { _configs = value; RaisePropertyChanged(); }
        }


        private string _header;

        public string Header
        {
            get { return _header; }
            set { _header = value; RaisePropertyChanged(); }
        }


        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;
        public MainViewModel(IRegionManager regionManager, IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            this.regionManager = regionManager;

            InitializeComponents();
        }
        /// <summary>
        /// 初始化一些操作
        /// </summary
        private void InitializeComponents()
        {
            InitCommand();
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBar();
            Configs = ConfigUtils.readConfig();
            ConfigUtils.ChangeThemes(Configs.Themes, Configs);
        }
        public DelegateCommand ThemeCommand { get; private set; }
        public DelegateCommand MinWindowCommand { get; private set; }
        public DelegateCommand MaxWindowCommand { get; private set; }
        public DelegateCommand CloseWindowCommand { get; private set; }
        public DelegateCommand<ChannelConfiguration> MeConfigsCommand { get; private set; }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        /// <summary>
        /// 注册一些事件
        /// </summary>
        private void InitCommand()
        {

            MinWindowCommand = new DelegateCommand(() => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            MaxWindowCommand = new DelegateCommand(() => { Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; });
            CloseWindowCommand = new DelegateCommand(Application.Current.Shutdown);
            ThemeCommand = new DelegateCommand(() => { ConfigUtils.ChangeThemes(!Configs.Themes, Configs); });

            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            MeConfigsCommand = new DelegateCommand<ChannelConfiguration>(MeConfigs);
        }

        private void Navigate(MenuBar bar)
        {
            if (bar == null || string.IsNullOrWhiteSpace(bar.NameSpace))
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(bar.NameSpace);
            Header = bar.Header;
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel() { Arg = MessageArg.Config, Message = Configs });

        }

        private void MeConfigs(ChannelConfiguration configuration)
        {
            if (configuration == null) return;
            foreach (var c in Configs.ChannelConfigurations) c.isSelect = false;
            configuration.isSelect = true;
            ConfigUtils.writeConfig(Configs);
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel() { Arg = MessageArg.Config, Message = Configs });
        }

        private void CreateMenuBar()
        {

            MenuBars.Add(new MenuBar() { Icon = "channel", Title = "频道", Header = "频道列表", NameSpace = "ChannelView" });
            MenuBars.Add(new MenuBar() { Icon = "favorite", Title = "收藏", Header = "我的收藏", NameSpace = "FavoriteView" });
            MenuBars.Add(new MenuBar() { Icon = "configuration", Title = "配置", Header = "配置中心", NameSpace = "ConfigurationView" });
            //MenuBars.Add(new MenuBar() { Icon = "settings", Title = "设置", Header = "设置", NameSpace = "SettingsView" });
            MenuBars.Add(new MenuBar() { Icon = "about", Title = "关于", Header = "关于", NameSpace = "AboutView" });
        }

        public void Configure()
        {
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("ChannelView");
            Header = "频道列表";
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel() { Arg = MessageArg.Config, Message = Configs });

        }
    }
}
