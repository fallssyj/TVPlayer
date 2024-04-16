using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TVPlayer.Common.Models;

namespace TVPlayer.Common.Utils
{
    public class ConfigUtils
    {
        public static string useragent = $"TVPlayer/{GetCompileVersion()}";
        public static string AppStartPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ConfigFile = $"{AppStartPath}/config.json";
        public static string Channelconfig = $"config";
        public static string ChannelConfiguration = $"{AppStartPath}/{Channelconfig}";

        public static string GetCompileVersion()
        {
            string OriginVersion = "" + File.GetLastWriteTime(App.Current.MainWindow.GetType().Assembly.Location);
            string formattedDate = "";

            foreach (char ch in OriginVersion)
            {
                if (char.IsDigit(ch))
                {
                    formattedDate += ch;
                }
            }

            return formattedDate.Length >= 11 ? formattedDate.Substring(0, 11) : "";

        }
        public static Config readConfig()
        {
            Config _config;
            try
            {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFile));
                return _config;
            }
            catch
            {
                _config = new Config();
                _config.Themes = false;
                writeConfig(_config);
                return _config;
            }
        }
        public static void writeConfig(Config _config)
        {
            string jsonStr = JsonConvert.SerializeObject(_config);
            File.WriteAllText(ConfigFile, jsonStr);
        }
        public static void ChangeThemes(bool IsThemeDark, Config _config)
        {
            var path = IsThemeDark ? new Uri("Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute) : new Uri("Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute);

            foreach (var res in Application.Current.Resources.MergedDictionaries)
            {
                if (res.Source != null && (res.Source.ToString() == "Themes/LightTheme.xaml" || res.Source.ToString() == "Themes/DarkTheme.xaml"))
                {
                    res.Source = path;
                }
            }
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(IsThemeDark ? BaseTheme.Dark : BaseTheme.Light);
            theme.SetPrimaryColor(Color.FromRgb(0x5c, 0xdd, 0x9c));
            theme.SetSecondaryColor(Colors.Lime);
            paletteHelper.SetTheme(theme);
            _config.Themes = IsThemeDark;
            writeConfig(_config);
        }
        public static async Task<int> getChannelAsync(string name, string path, string url)
        {
            string responseBody = null;
            int _Count = 0;
            try
            {
                if (!Directory.Exists(ChannelConfiguration)) Directory.CreateDirectory(ChannelConfiguration);
                if (!Directory.Exists($"{ChannelConfiguration}/{name}")) Directory.CreateDirectory($"{ChannelConfiguration}/{name}");
                using (var client = new HttpClient())
                {
                    var response = new HttpRequestMessage(HttpMethod.Get, url);
                    response.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    response.Headers.Add("User-Agent", useragent);
                    client.Timeout = TimeSpan.FromSeconds(8);
                    var result = await client.SendAsync(response);
                    responseBody = await result.Content.ReadAsStringAsync();

                    if (responseBody.IndexOf("#EXTINF") > -1)
                    {
                        ObservableCollection<Channel> channelLists = ParseFileModule.ParseM3uFile(responseBody);
                        responseBody = JsonConvert.SerializeObject(channelLists);
                        _Count = channelLists.Count;
                        File.WriteAllText($"{AppStartPath}/{path}", responseBody);
                        return _Count;
                    }
                    else
                    {
                        _Count = JsonConvert.DeserializeObject<ObservableCollection<Channel>>(responseBody).Count;
                        File.WriteAllText($"{AppStartPath}/{path}", responseBody);
                        return _Count;
                    }

                }
            }
            catch (Exception e)
            {
                App.MessageBox(new DialogParameters
                        {
                    { "Title", "错误" },
                    { "Msg", $"url:{url} \r\nresponseBody: {responseBody} \r\nerror: {e}" }
                });
                return _Count;
            }

        }
    }
}
