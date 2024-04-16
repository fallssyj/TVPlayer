using Prism.Mvvm;
using System;

namespace TVPlayer.Common.Models
{
    public class ChannelConfiguration : BindableBase
    {
        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private int _count;

        public int count
        {
            get { return _count; }
            set { _count = value; RaisePropertyChanged(); }
        }

        private string _url;

        public string url
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(); }
        }

        private string _path;

        public string path
        {
            get { return _path; }
            set { _path = value; RaisePropertyChanged(); }
        }

        private Boolean _isSelect;

        public Boolean isSelect
        {
            get { return _isSelect; }
            set { _isSelect = value; RaisePropertyChanged(); }
        }

        private DateTime _updateTime;

        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; RaisePropertyChanged(); }
        }

        private string _updateTimes;

        public string UpdateTimes
        {
            get { return _updateTimes; }
            set { _updateTimes = value; RaisePropertyChanged(); }
        }

    }
}
