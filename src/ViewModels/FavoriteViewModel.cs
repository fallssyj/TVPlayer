using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using TVPlayer.Common.Events;
using TVPlayer.Common.Models;
using TVPlayer.Common.Utils;
using TVPlayer.Views;

namespace TVPlayer.ViewModels
{
    public class FavoriteViewModel : BindableBase
    {
        private int _columnCount = 4;

        public int ColumnCount
        {
            get { return _columnCount; }
            set { _columnCount = value; RaisePropertyChanged(); }
        }

        private Config _configs;

        public Config Configs
        {
            get { return _configs; }
            set { _configs = value; RaisePropertyChanged(); }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<string> _groupTitles;

        public ObservableCollection<string> GroupTitles
        {
            get { return _groupTitles; }
            set { _groupTitles = value; RaisePropertyChanged(); SelectedIndex = 0; }
        }

        private ObservableCollection<Channel> _favorites;

        public ObservableCollection<Channel> Favorites
        {
            get { return _favorites; }
            set { _favorites = value; RaisePropertyChanged(); }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged(); }
        }

        private string GroupTitle { get; set; } = null;

        private readonly IEventAggregator aggregator;

        public FavoriteViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            InitializeComponents();
        }




        public DelegateCommand<UserControl> WindowSizeChangedCommand { get; set; }

        public DelegateCommand<Channel> PlayerargCommand { get; private set; }

        public DelegateCommand<Channel> FavoriteCommand { get; set; }

        public DelegateCommand<string> GroupTitlesChangedCommand { get; set; }

        public DelegateCommand SearchInputCommand { get; set; }


        private PlayerView _playerWindow { get; set; }

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
                    SelectedIndex = 0;
                    if (Configs.Favorite == null) return;
                    Favorites = Configs.Favorite;
                    GroupTitles = new ObservableCollection<string>() { "全部频道" };

                    foreach (var ch in Configs.Favorite)
                    {
                        if (string.IsNullOrEmpty(ch.Grouptitle) || ch.Grouptitle.Contains("未分类") || ch.Grouptitle.Length == 0)
                        {
                            if (GroupTitles.Where(item => item == "未分类").Count() == 0)
                                GroupTitles.Add("未分类");
                        }
                        else if (GroupTitles.Where(item => item == ch.Grouptitle).Count() == 0)
                            GroupTitles.Add(ch.Grouptitle);
                    }
                }
            });

            WindowSizeChangedCommand = new DelegateCommand<UserControl>((u) =>
            {
                ColumnCount = Math.Max(1, (int)Math.Floor(u.ActualWidth / 200));
            });

            PlayerargCommand = new DelegateCommand<Channel>(Playerarg);
            FavoriteCommand = new DelegateCommand<Channel>(Favorite);
            GroupTitlesChangedCommand = new DelegateCommand<string>(GroupTitlesChangedAsync);
            SearchInputCommand = new DelegateCommand(SearchInputAsync);
        }
        private async void SearchInputAsync()
        {
            await Task.Run(() =>
            {
                if (Configs.Favorite == null) return;
                if (string.IsNullOrEmpty(SearchText) || SearchText.Length == 0)
                {
                    if (string.IsNullOrEmpty(GroupTitle)) return;

                    if (GroupTitle.Contains("全部频道"))
                        Favorites = Configs.Favorite;
                    else if (GroupTitle.Contains("未分类"))
                    {
                        Favorites = new ObservableCollection<Channel>(Configs.Favorite.Where(item => item.Grouptitle == "未分类" || string.IsNullOrEmpty(item.Grouptitle) || item.Grouptitle.Length == 0));
                    }

                    else
                        Favorites = new ObservableCollection<Channel>(Configs.Favorite.Where(item => item.Grouptitle == GroupTitle));
                }
                else
                    Favorites = new ObservableCollection<Channel>(Configs.Favorite.Where(item => item.Tvgname.ToLower().IndexOf(SearchText.ToLower()) > -1));

            });

        }
        private async void GroupTitlesChangedAsync(string s)
        {
            await Task.Run(() =>
            {
                GroupTitle = s;
                if (string.IsNullOrEmpty(GroupTitle)) return;

                if (GroupTitle.Contains("全部频道"))
                    Favorites = Configs.Favorite;
                else if (GroupTitle.Contains("未分类"))
                {
                    Favorites = new ObservableCollection<Channel>(Configs.Favorite.Where(item => item.Grouptitle == "未分类" || string.IsNullOrEmpty(item.Grouptitle) || item.Grouptitle.Length == 0));
                }

                else
                    Favorites = new ObservableCollection<Channel>(Configs.Favorite.Where(item => item.Grouptitle == GroupTitle));
            });
        }

        private void Favorite(Channel _channel)
        {
            if (_channel == null) return;
            Configs.Favorite.Remove(_channel);
            Favorites.Remove(_channel);
            if (Configs.Favorite.Where(item => item.Grouptitle == _channel.Grouptitle).Count() == 0)
            {
                GroupTitles.Remove(string.IsNullOrEmpty(_channel.Grouptitle) || _channel.Grouptitle.Length == 0 ? "未分类" : _channel.Grouptitle);
                SelectedIndex = 0;
            }
            ConfigUtils.writeConfig(Configs);
        }

        private void Playerarg(Channel _channel)
        {
            if (_channel != null)
            {
                if (_playerWindow == null)
                {
                    _playerWindow = new PlayerView(aggregator);
                    _playerWindow.Show();
                    _playerWindow.Closing += ((s, e) => _playerWindow = null);
                }
                aggregator.GetEvent<MessageEvent>().Publish(new MessageModel { Arg = MessageArg.Player, Message = _channel });
            }

        }
    }
}
