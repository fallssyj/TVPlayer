using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using TVPlayer.Common.Models;

namespace TVPlayer.Common.Utils
{
    //Link https://github.com/Isayso/PlaylistEditorTV
    public class ColList
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
    }
    internal static class ParseFileModule
    {
        public static List<ColList> columnList { get; private set; }
        public static ObservableCollection<Channel> channelLists { get; private set; }

        public static ObservableCollection<Channel> ParseM3uFile(string fullstr)
        {
            string[] regArray = {
                "tvg-name",
                "tvg-id",
                "tvg-logo",
                "group-title"};
            string[] linktypes = ["ht", "plugin", "rt", "ud", "mm"]; //Types of links in Column "Link"
            columnList = new List<ColList>();
            channelLists = new ObservableCollection<Channel>();

            for (int i = 0; i < regArray.Length; i++)
            {
                if (fullstr.ContainsElement(regArray[i] + "=\"([^\"]*)"))
                {

                    columnList.Add(new ColList
                    {
                        Name = regArray[i],
                        Visible = true,
                    });
                }
            }

            string[] fileRows = fullstr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Channel _channelListEntry = null;
            for (int i = 0; i < fileRows.Length; i++)
            {

                if (fileRows[i].StartsWith("#EXTINF"))
                {
                    _channelListEntry = new Channel();
                    foreach (var c in columnList)
                    {
                        string header = c.Name.ToString();

                        var match = Regex.Match(fileRows[i], header + "=\"([^\"]*)\"").Groups[1];

                        if (match.Success)
                        {
                            string _value = match.Captures[0].Value;
                            switch (header)
                            {
                                case "tvg-name":
                                    _channelListEntry.Tvgname = _value;
                                    break;
                                case "tvg-id":
                                    _channelListEntry.Tvgid = _value;
                                    break;
                                case "tvg-logo":
                                    _channelListEntry.Tvglogo = _value;
                                    break;
                                case "group-title":
                                    _channelListEntry.Grouptitle = _value;
                                    break;

                            }
                            continue;
                        }
                    }
                    _channelListEntry.Name2 = fileRows[i].Split(',').Last().Trim();
                    channelLists.Add(_channelListEntry);
                    continue;
                }
                else if ((linktypes.Any(fileRows[i].StartsWith))
                    && (fileRows[i].Contains("//") || fileRows[i].Contains(":\\")))//issue #32 issue #61
                {
                    try
                    {
                        _channelListEntry.Link = fileRows[i];

                    }
                    catch { continue; }
                }

            }

            return channelLists;

        }
        private static bool ContainsElement(this string input, string regString)
        {
            var match = Regex.Match(input, regString);

            if (match.Success) return true;

            return false;
        }
    }
}
