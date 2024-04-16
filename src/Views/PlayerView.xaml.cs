using Prism.Events;
using System.Windows;
using TVPlayer.Common.Events;

namespace TVPlayer.Views
{
    /// <summary>
    /// PlayerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerView : Window
    {
        public static PlayerView Instance { get; private set; }
        public PlayerView(IEventAggregator eventAggregator)
        {
            Instance = this;
            InitializeComponent();
            eventAggregator.GetEvent<MessageEvent>().Subscribe((m) =>
            {
                if (m.Arg == MessageArg.Close) Close();
            }, false);
        }

    }
}
