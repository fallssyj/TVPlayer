using Prism.Events;

namespace TVPlayer.Common.Events
{

    public class MessageModel
    {
        public int Arg { get; set; }
        public object Message { get; set; }
    }

    public class MessageArg
    {
        public static readonly int Config = 01;
        public static readonly int Player = 02;
        public static readonly int Close = 03;

    }

    public class MessageEvent : PubSubEvent<MessageModel>
    {
    }
}
