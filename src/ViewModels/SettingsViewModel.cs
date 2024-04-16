using Prism.Events;
using Prism.Mvvm;
using TVPlayer.Common.Events;
using TVPlayer.Common.Models;

namespace TVPlayer.ViewModels
{
    public class SettingsViewModel : BindableBase
    {

        private Config _configs;

        public Config Configs
        {
            get { return _configs; }
            set { _configs = value; RaisePropertyChanged(); }
        }
        private readonly IEventAggregator aggregator;
        public SettingsViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            InitializeComponents();
        }
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
                }
            });
        }
    }
}
