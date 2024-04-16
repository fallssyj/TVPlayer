using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TVPlayer.Common.Utils;

namespace TVPlayer.ViewModels
{
    public class AboutViewModel : BindableBase
    {

        private string author = "fallssyj";
        public string Author
        {
            get { return author; }
            set { SetProperty(ref author, value); }
        }

        private string compileVersion;

        public string CompileVersion
        {
            get { return compileVersion; }
            set { compileVersion = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<AboutModel> _aboutModels;

        public ObservableCollection<AboutModel> AboutModels
        {
            get { return _aboutModels; }
            set { _aboutModels = value; RaisePropertyChanged(); }
        }


        public DelegateCommand<AboutModel> MouseDoubleCommand { get; set; }
        public DelegateCommand BlogLinkCommand { get; set; }
        public DelegateCommand EmailLinkCommand { get; set; }
        public DelegateCommand GithublLinkCommand { get; set; }
        public AboutViewModel()
        {

            CompileVersion = ConfigUtils.GetCompileVersion();

            AboutModels =
            [
                new AboutModel() { Name = "fallssyj/TVPlayer", Link = "https://github.com/fallssyj/TVPlayer" },
                new AboutModel() { Name = "FFmpeg/FFmpeg", Link = "https://github.com/FFmpeg/FFmpeg" },
                new AboutModel() { Name = "Ruslan-B/FFmpeg.AutoGen", Link = "https://github.com/Ruslan-B/FFmpeg.AutoGen" },
                new AboutModel() { Name = "amerkoleci/Vortice.Windows", Link = "https://github.com/amerkoleci/Vortice.Windows" },
                new AboutModel() { Name = "SuRGeoNix/Flyleaf", Link = "https://github.com/SuRGeoNix/Flyleaf" },
                new AboutModel() { Name = "yt-dlp/yt-dlp", Link = "https://github.com/yt-dlp/yt-dlp" },
                new AboutModel() { Name = "Isayso/PlaylistEditorTV", Link = "https://github.com/Isayso/PlaylistEditorTV" },
                new AboutModel() { Name = "MaterialDesignInXAML/MaterialDesignInXamlToolkit", Link = "https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit" },
                new AboutModel() { Name = "JamesNK/Newtonsoft.Json", Link = "https://github.com/JamesNK/Newtonsoft.Json" },
                new AboutModel() { Name = "PrismLibrary/Prism", Link = "https://github.com/PrismLibrary/Prism" },

            ];

            MouseDoubleCommand = new DelegateCommand<AboutModel>((m) =>
            {
                if (m == null) return;
                Process.Start(new ProcessStartInfo
                {
                    FileName = m.Link,
                    UseShellExecute = true
                });
            });

            BlogLinkCommand = new DelegateCommand(() =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://51syj.club",
                    UseShellExecute = true
                });
            });
            EmailLinkCommand = new DelegateCommand(() =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "mailto:falls_syj@qq.com",
                    UseShellExecute = true
                });
            });
            GithublLinkCommand = new DelegateCommand(() =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/fallssyj",
                    UseShellExecute = true
                });
            });


        }


    }

    public class AboutModel
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _link;

        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

    }
}
