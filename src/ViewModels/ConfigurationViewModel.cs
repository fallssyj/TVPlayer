using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TVPlayer.Common.Events;
using TVPlayer.Common.Models;
using TVPlayer.Common.Utils;

namespace TVPlayer.ViewModels
{
    public class ConfigurationViewModel : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        private Visibility _isEditWindow = Visibility.Collapsed;

        public Visibility IsEditWindow
        {
            get { return _isEditWindow; }
            set { _isEditWindow = value; RaisePropertyChanged(); }
        }

        private Visibility _isAddWindow = Visibility.Collapsed;

        public Visibility IsAddWindow
        {
            get { return _isAddWindow; }
            set { _isAddWindow = value; RaisePropertyChanged(); }
        }


        private Visibility _isRefresh = Visibility.Collapsed;

        public Visibility IsRefresh
        {
            get { return _isRefresh; }
            set { _isRefresh = value; RaisePropertyChanged(); }
        }

        private Config _configs;

        public Config Configs
        {
            get { return _configs; }
            set { _configs = value; RaisePropertyChanged(); }
        }
        private ChannelConfiguration _chf;

        public ChannelConfiguration chf
        {
            get { return _chf; }
            set { _chf = value; RaisePropertyChanged(); }
        }

        private readonly IEventAggregator aggregator;
        public ConfigurationViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            InitializeComponents();
        }

        public DelegateCommand<ChannelConfiguration> MeConfigsCommand { get; private set; }
        public DelegateCommand<ChannelConfiguration> DeleteItemCommand { get; private set; }
        public DelegateCommand<ChannelConfiguration> ShowEditItemCommand { get; private set; }
        public DelegateCommand<ChannelConfiguration> RefreshChannelCommand { get; private set; }

        public DelegateCommand SaveEditCommand { get; private set; }

        public DelegateCommand CloseEditWindowCommand { get; private set; }
        public DelegateCommand CloseAddWindowCommand { get; private set; }

        public DelegateCommand RefreshAllChannelCommand { get; private set; }
        public DelegateCommand ShowAddChannelCommand { get; private set; }
        public DelegateCommand AddChannelCommand { get; private set; }
        /// <summary>
        /// 初始化一些操作
        /// </summary
        private void InitializeComponents()
        {
            InitCommand();

        }

        /// <summary>
        /// 注册一些事件
        /// </summary>
        private void InitCommand()
        {
            aggregator.GetEvent<MessageEvent>().Subscribe((m) =>
            {
                if (m.Arg == MessageArg.Config)
                {
                    Configs = m.Message as Config;
                    checkUpdateTime();
                }
            });

            CloseEditWindowCommand = new DelegateCommand(() =>
            {
                IsEditWindow = IsEditWindow != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;

            });

            CloseAddWindowCommand = new DelegateCommand(() =>
            {
                IsAddWindow = IsAddWindow != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;

            });
            MeConfigsCommand = new DelegateCommand<ChannelConfiguration>(MeConfigs);

            ShowEditItemCommand = new DelegateCommand<ChannelConfiguration>(ShowEditItem);
            DeleteItemCommand = new DelegateCommand<ChannelConfiguration>(DeleteItem);
            RefreshChannelCommand = new DelegateCommand<ChannelConfiguration>(RefreshChannelAsync);

            SaveEditCommand = new DelegateCommand(SaveEdit);

            RefreshAllChannelCommand = new DelegateCommand(RefreshAllChannel);
            ShowAddChannelCommand = new DelegateCommand(ShowAddChannel);
            AddChannelCommand = new DelegateCommand(AddChannelAsync);
        }



        /// <summary>
        /// 更新UI 配置上次更新事件
        /// </summary>
        private void checkUpdateTime()
        {
            if (Configs == null || Configs.ChannelConfigurations == null) return;
            foreach (var ch in Configs.ChannelConfigurations)
            {
                var timeSpan = DateTime.Now.Subtract(ch.UpdateTime);
                if (timeSpan.TotalDays >= 1)
                {
                    ch.UpdateTimes = $"{(int)timeSpan.TotalDays} 天前";
                }
                else if (timeSpan.TotalHours >= 1)
                {
                    ch.UpdateTimes = $"{(int)timeSpan.TotalHours} 小时前";
                }
                else if (timeSpan.TotalMinutes >= 1)
                {
                    ch.UpdateTimes = $"{(int)timeSpan.TotalMinutes} 分前";
                }
                else
                {
                    ch.UpdateTimes = $"{(int)timeSpan.TotalSeconds} 秒前";
                }
            }
        }

        private async void AddChannelAsync()
        {
            try
            {
                chf.path = $"{ConfigUtils.Channelconfig}/{chf.name}/Channel.json";
                chf.count = await ConfigUtils.getChannelAsync(chf.name, chf.path, chf.url);
                chf.UpdateTime = DateTime.Now;
                if (chf.count != 0)
                {
                    if (Configs.ChannelConfigurations == null) Configs.ChannelConfigurations = new ObservableCollection<ChannelConfiguration>();
                    Configs.ChannelConfigurations.Add(chf);
                    ConfigUtils.writeConfig(Configs);
                }
                CloseAddWindowCommand.Execute();
                checkUpdateTime();
            }
            catch (Exception e)
            {
                App.MessageBox(new DialogParameters
                {
                    { "Title", "错误" },
                    { "Msg", $"error: {e}"}
                });
            }

        }

        private void ShowAddChannel()
        {
            chf = new ChannelConfiguration();
            IsAddWindow = IsAddWindow != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
            IsEditWindow = Visibility.Collapsed;

        }
        private async void RefreshChannelAsync(ChannelConfiguration configuration)
        {
            try
            {
                if (configuration == null) return;

                IsRefresh = Visibility.Visible;

                configuration.count = await ConfigUtils.getChannelAsync(configuration.name, configuration.path, configuration.url);
                configuration.UpdateTime = DateTime.Now;
                Configs.Favorite = new ObservableCollection<Channel>();
                ConfigUtils.writeConfig(Configs);
                IsRefresh = Visibility.Collapsed;
                checkUpdateTime();
            }
            catch (Exception e)
            {

                App.MessageBox(new DialogParameters
                {
                    { "Title", "错误" },
                    { "Msg", $"error: {e}"}
                });
            }
        }
        private async void RefreshAllChannel()
        {
            try
            {
                IsRefresh = Visibility.Visible;
                foreach (var c in Configs.ChannelConfigurations)
                {
                    c.count = await ConfigUtils.getChannelAsync(c.name, c.path, c.url);
                    c.UpdateTime = DateTime.Now;
                }
                Configs.Favorite = new ObservableCollection<Channel>();
                ConfigUtils.writeConfig(Configs);
                IsRefresh = Visibility.Collapsed;
                checkUpdateTime();
            }
            catch (Exception e)
            {

                App.MessageBox(new DialogParameters
                {
                    { "Title", "错误" },
                    { "Msg", $"error: {e}"}
                });
            }

        }

        private void SaveEdit()
        {
            try
            {

                CloseEditWindowCommand.Execute();
                ConfigUtils.writeConfig(Configs);
            }
            catch (Exception e)
            {

                App.MessageBox(new DialogParameters
                {
                    { "Title", "错误" },
                    { "Msg", $"error: {e}"}
                });
            }

        }

        private void ShowEditItem(ChannelConfiguration configuration)
        {
            if (configuration == null) return;
            chf = configuration;
            Title = configuration.name;
            IsEditWindow = IsEditWindow != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
            IsAddWindow = Visibility.Collapsed;
        }

        private void DeleteItem(ChannelConfiguration configuration)
        {
            try
            {
                if (configuration == null) return;
                System.Diagnostics.Debug.WriteLine(configuration.name);
                Configs.ChannelConfigurations.Remove(configuration);
                Directory.Delete($"{ConfigUtils.ChannelConfiguration}/{configuration.name}", true);
                ConfigUtils.writeConfig(Configs);
            }
            catch (Exception e)
            {

                App.MessageBox(new DialogParameters
                {
                    { "Title", "错误" },
                    { "Msg", $"error: {e}"}
                });
            }

        }

        private void MeConfigs(ChannelConfiguration configuration)
        {
            try
            {
                if (configuration == null) return;
                foreach (var c in Configs.ChannelConfigurations) c.isSelect = false;
                configuration.isSelect = true;
                ConfigUtils.writeConfig(Configs);
            }
            catch (Exception e)
            {
                App.MessageBox(new DialogParameters
                {
                    { "Title", "错误" },
                    { "Msg", $"error: {e}"}
                });
            }

        }
    }
}
