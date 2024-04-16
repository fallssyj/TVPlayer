using FlyleafLib;
using FlyleafLib.MediaFramework.MediaRenderer;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using static FlyleafLib.Utils;

namespace TVPlayer.Common.Utils
{
    public class FlyleafLibConfigs
    {
        public static Config GetConfig()
        {
            var config = new Config();
            config.Player.AutoPlay = true;
            config.Player.MinBufferDuration = 5000000;
            config.Player.IdleFps = 60;
            config.Player.MaxLatency = 0;
            config.Player.MinLatency = 0;
            config.Player.LatencySpeedChangeInterval = 7000000;
            config.Player.FolderRecordings = $"{ConfigUtils.AppStartPath}/Recordings";
            config.Player.FolderSnapshots = $"{ConfigUtils.AppStartPath}/Snapshots";
            config.Player.SeekAccurate = false;
            config.Player.SnapshotFormat = "bmp";
            config.Player.Stats = false;
            config.Player.ThreadPriority = ThreadPriority.AboveNormal;
            config.Player.UICurTimePerFrame = false;
            config.Player.VolumeMax = 150;
            config.Player.Usage = 0;
            config.Player.AudioDelayOffset = 1000000;
            config.Player.AudioDelayOffset2 = 10000000;
            config.Player.SubtitlesDelayOffset = 1000000;
            config.Player.SubtitlesDelayOffset2 = 10000000;
            config.Player.SeekOffset = 50000000;
            config.Player.SeekOffset2 = 150000000;
            config.Player.SeekOffset3 = 300000000;
            config.Player.SpeedOffset = 0.1;
            config.Player.SpeedOffset2 = 0.25;
            config.Player.ZoomOffset = 10;
            config.Player.VolumeOffset = 5;

            config.Demuxer.AllowFindStreamInfo = true;
            config.Demuxer.AllowInterrupts = true;
            config.Demuxer.AllowReadInterrupts = true;
            config.Demuxer.AllowTimeouts = true;
            config.Demuxer.ExcludeInterruptFmts = new List<string> { "rtsp" };
            config.Demuxer.BufferDuration = 300000000;
            config.Demuxer.BufferPackets = 0;
            config.Demuxer.MaxAudioPackets = 0;
            config.Demuxer.MaxErrors = 30;
            config.Demuxer.IOStreamBufferSize = 2097152;
            config.Demuxer.CloseTimeout = 10000000;
            config.Demuxer.OpenTimeout = 3000000000;
            config.Demuxer.ReadTimeout = 100000000;
            config.Demuxer.ReadLiveTimeout = 200000000;
            config.Demuxer.SeekTimeout = 80000000;
            config.Demuxer.ForceFormat = null;
            config.Demuxer.ForceFPS = 0;
            config.Demuxer.FormatFlags = 256;
            config.Demuxer.FormatOptToUnderlying = false;

            config.Demuxer.FormatOpt = new SerializableDictionary<string, string>
            {
                { "probesize", "52428800" },
                { "analyzeduration", "10000000" },
                { "reconnect", "1" },
                { "reconnect_streamed", "1" },
                { "reconnect_delay_max", "7" },
                { "user_agent", ConfigUtils.useragent },
                { "rtsp_transport", "tcp" }
            };
            config.Decoder.AudioCodecOpt = new SerializableDictionary<string, string>
            {
                { "probesize", "52428800" },
                { "analyzeduration", "10000000" },
                { "reconnect", "1" },
                { "reconnect_streamed", "1" },
                { "reconnect_delay_max", "7" },
                { "user_agent", ConfigUtils.useragent },
                { "rtsp_transport", "tcp" }
            };
            config.Decoder.SubtitlesCodecOpt = new SerializableDictionary<string, string>
            {
                { "probesize", "52428800" },
                { "analyzeduration", "10000000" },
                { "reconnect", "1" },
                { "reconnect_streamed", "1" },
                { "reconnect_delay_max", "7" },
                { "user_agent", ConfigUtils.useragent },
                { "rtsp_transport", "tcp" }
            };

            config.Decoder.VideoThreads = 12;
            config.Decoder.MaxVideoFrames = 4;
            config.Decoder.MaxAudioFrames = 10;
            config.Decoder.MaxSubsFrames = 1;
            config.Decoder.MaxErrors = 200;
            config.Decoder.ZeroCopy = 0;
            config.Decoder.AllowProfileMismatch = false;
            config.Decoder.ShowCorrupted = false;
            config.Decoder.LowDelay = false;

            config.Video.GPUAdapter = "";
            config.Video.AspectRatio = new AspectRatio { Num = 1, Den = 1, Value = -1, ValueStr = "Keep" };
            config.Video.CustomAspectRatio = new AspectRatio { Num = 16, Den = 9, Value = (float)1.7777778, ValueStr = "16:9" };
            config.Video.BackgroundColor = Colors.Black;
            config.Video.ClearScreenOnOpen = false;
            config.Video.Enabled = true;
            config.Video.MaxVerticalResolutionCustom = 0;
            config.Video.SwsHighQuality = false;
            config.Video.SwsForce = false;
            config.Video.VideoAcceleration = true;
            config.Video.VideoProcessor = 0;
            config.Video.VSync = 0;
            config.Video.Deinterlace = false;
            config.Video.DeinterlaceBottomFirst = false;
            config.Video.HDRtoSDRMethod = HDRtoSDRMethod.Hable;
            config.Video.HDRtoSDRTone = (float)1.4;
            config.Video.Swap10Bit = false;
            config.Video.SwapBuffers = 2;
            config.Video.SwapForceR8G8B8A8 = false;
            config.Video.Filters = new SerializableDictionary<VideoFilters, VideoFilter>
            {
                {
                    VideoFilters.Brightness,new VideoFilter
                    {
                        Filter = VideoFilters.Brightness,
                        Minimum = -1000,
                        Maximum = 1000,
                        Step = (float)0.1,
                        DefaultValue = 0,
                        Value = 0
                    }
                },
                {
                    VideoFilters.Contrast,new VideoFilter
                    {
                        Filter = VideoFilters.Contrast,
                        Minimum = 0,
                        Maximum = 200,
                        Step = (float)0.01,
                        DefaultValue = 100,
                        Value = 100
                    }
                }
                ,
                {
                    VideoFilters.Hue,new VideoFilter
                    {
                        Filter = VideoFilters.Hue,
                        Minimum = -180,
                        Maximum = 180,
                        Step = 1,
                        DefaultValue = 0,
                        Value = 0
                    }
                }
                ,
                {
                    VideoFilters.Saturation,new VideoFilter
                    {
                        Filter = VideoFilters.Saturation,
                        Minimum = 0,
                        Maximum = 300,
                        Step = (float)0.01,
                        DefaultValue = 100,
                        Value = 100
                    }
                }
                ,
                {
                    VideoFilters.NoiseReduction,new VideoFilter
                    {
                        Filter = VideoFilters.NoiseReduction,
                        Minimum = 0,
                        Maximum = 100,
                        Step = 1,
                        DefaultValue = 50,
                        Value = 50
                    }
                }
                ,
                {
                    VideoFilters.EdgeEnhancement,new VideoFilter
                    {
                        Filter = VideoFilters.EdgeEnhancement,
                        Minimum = 0,
                        Maximum = 100,
                        Step = 1,
                        DefaultValue = 50,
                        Value = 50
                    }
                }
                ,
                {
                    VideoFilters.AnamorphicScaling,new VideoFilter
                    {
                        Filter = VideoFilters.AnamorphicScaling,
                        Minimum = 0,
                        Maximum = 100,
                        Step = 1,
                        DefaultValue = 50,
                        Value = 50
                    }
                }
                ,
                {
                    VideoFilters.StereoAdjustment,new VideoFilter
                    {
                        Filter = VideoFilters.StereoAdjustment,
                        Minimum = 0,
                        Maximum = 100,
                        Step = 1,
                        DefaultValue = 100,
                        Value = 100
                    }
                }


            };

            config.Audio.Delay = 0;
            config.Audio.Enabled = true;
            config.Audio.FiltersEnabled = true;
            config.Audio.Filters = null;
            config.Plugins = new SerializableDictionary<string, SerializableDictionary<string, string>>
            {
                {
                    "YoutubeDL", new SerializableDictionary<string, string>
                    {
                        { "ExtraArguments","--cookies-from-browser chrome" }
                    }
                }
            };

            return config;
        }
    }
}
