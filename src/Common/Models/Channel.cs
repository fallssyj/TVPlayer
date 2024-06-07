using Prism.Mvvm;
using System;
using System.Net.Http;
using System.Security.Policy;

namespace TVPlayer.Common.Models
{
    public class Channel : BindableBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string tvgname;
        public string Tvgname
        {
            get { return tvgname; }
            set { tvgname = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// ID
        /// </summary>
        private string tvgid;
        public string Tvgid
        {
            get { return tvgid; }
            set { tvgid = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// logo地址
        /// </summary>
        private string tvglogo;
        public string Tvglogo
        {
            get { return tvglogo; }
            set { tvglogo = value;  RaisePropertyChanged(); LoadImage(); }
        }

        private async void LoadImage()
        {
            if (string.IsNullOrEmpty(Tvglogo) || Tvglogo.Length == 0)
            {
                return;
            }
            using (var client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(8);
                    using (var request = new HttpRequestMessage(HttpMethod.Head, Tvglogo))
                    using (var response = await client.SendAsync(request))
                    {
                        if (!response.IsSuccessStatusCode) Tvglogo = "";
                    }
                }
                catch
                {
                    Tvglogo = "";
                    return;
                }
            }
            
        }

        private string grouptitle;
        /// <summary>
        /// 组名称
        /// </summary>
        public string Grouptitle
        {
            get { return grouptitle; }
            set { grouptitle = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// name2
        /// </summary>
        private string name2;
        public string Name2
        {
            get { return name2; }
            set { name2 = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Link
        /// </summary>
        private string link;
        public string Link
        {
            get { return link; }
            set { link = value; RaisePropertyChanged(); }
        }

        private bool favorite;

        public bool Favorite
        {
            get { return favorite; }
            set { favorite = value; RaisePropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Channel other = (Channel)obj;
            return Tvgname == other.Tvgname && Link == other.Link;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tvgname, Link);
        }

    }
}
