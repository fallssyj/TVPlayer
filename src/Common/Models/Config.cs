using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace TVPlayer.Common.Models
{
    public class Config : BindableBase
    {
        private Boolean _Themes;

        public Boolean Themes
        {
            get { return _Themes; }
            set { _Themes = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<ChannelConfiguration> _ChannelConfigurations;

        public ObservableCollection<ChannelConfiguration> ChannelConfigurations
        {
            get { return _ChannelConfigurations; }
            set { _ChannelConfigurations = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Channel> _favorite;

        public ObservableCollection<Channel> Favorite
        {
            get { return _favorite; }
            set { _favorite = value; RaisePropertyChanged(); }
        }


    }
}
