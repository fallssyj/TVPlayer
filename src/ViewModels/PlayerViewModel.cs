using FlyleafLib.Controls.WPF;
using FlyleafLib.MediaPlayer;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;
using TVPlayer.Common.Events;
using TVPlayer.Common.Models;
using TVPlayer.Common.Utils;
using TVPlayer.Views;

namespace TVPlayer.ViewModels
{
    public class PlayerViewModel : BindableBase
    {

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Visibility isDebug = Visibility.Collapsed;

        public Visibility IsDebug
        {
            get { return isDebug; }
            set { isDebug = value; RaisePropertyChanged(); }
        }


        private readonly IEventAggregator aggregator;
        public PlayerViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            InitializeComponents();
        }


        public FlyleafME FlyleafME { get; set; }
        public Player Player { get; set; }



        public DelegateCommand CloseWindowCommand { get; set; }
        public DelegateCommand MinWindowCommand { get; set; }
        public DelegateCommand ShowDebugCommand { get; private set; }


        /// <summary>
        /// 初始化一些操作
        /// </summary
        private void InitializeComponents()
        {
            InitFlyleaf();
            InitCommand();
        }
        /// <summary>
        /// 初始化Flyleaf
        /// </summary>
        private void InitFlyleaf()
        {
            FlyleafME = new FlyleafME(PlayerView.Instance)
            {
                Tag = this,
                ActivityTimeout = 3500,
                KeyBindings = AvailableWindows.Both,
                DetachedResize = AvailableWindows.Overlay,
                DetachedDragMove = AvailableWindows.Both,
                ToggleFullScreenOnDoubleClick
                                   = AvailableWindows.Both,
                KeepRatioOnResize = true,
                OpenOnDrop = AvailableWindows.Both,

                PreferredLandscapeWidth = 1000,
                PreferredPortraitHeight = 700
            };

            Player = new Player(FlyleafLibConfigs.GetConfig());
            Player.Audio.Volume = 100;
            FlyleafME.Player = Player;
        }




        // <summary>
        /// 注册一些事件
        /// </summary>
        private void InitCommand()
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(UpdateUI, false);

            MinWindowCommand = new DelegateCommand(() => { FlyleafME.IsMinimized = true; });

            CloseWindowCommand = new DelegateCommand(() =>
            {
                Player.Dispose();
                aggregator.GetEvent<MessageEvent>().Publish(new MessageModel { Arg = MessageArg.Close });
            });

            ShowDebugCommand = new DelegateCommand(() =>
            {
                IsDebug = IsDebug != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
            });


        }

        private void UpdateUI(MessageModel m)
        {
            if (m.Arg == MessageArg.Player)
            {
                Title = (m.Message as Channel).Tvgname;
                Player.Commands.Open.Execute((m.Message as Channel).Link);
            }
        }
    }
}
