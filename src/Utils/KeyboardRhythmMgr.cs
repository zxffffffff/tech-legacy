using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Utils
{
    public class RhythmLyrics
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double TimeSec { get; set; }
        public string[][] Lines { get; set; }
    }

    public class RhythmLyricsLine
    {
        public int Index { get; set; }
        public double BeginTime { get; set; }
        public double EndTime { get; set; }
        public string Text { get; set; }
        public string KeyText { get; set; }

        public static int FieldCount { get; } = 5;

        public static RhythmLyricsLine Deserialize(string[] line)
        {
            if (line.Count() < FieldCount)
            {
                GD.PrintErr("invalid line");
                return null;
            }

            var lyricsLine = new RhythmLyricsLine()
            {
                Index = line[0].ToInt(),
                BeginTime = double.Parse(line[1]),
                EndTime = double.Parse(line[2]),
                Text = line[3],
                KeyText = line[4],
            };
            return lyricsLine;
        }

        public string[] Serialize()
        {
            return [
                Index.ToString(),
                BeginTime.ToString("F3"),
                EndTime.ToString("F3"),
                Text,
                KeyText,
            ];
        }
    }

    public class RhythmLyricsPlay()
    {
        private RhythmLyrics _lyrics;
        private List<RhythmLyricsLine> _lyricsLines;

        private int _lastIndex = 0;

        public void Start(RhythmLyrics lyrics)
        {
            if (lyrics == null || lyrics.Lines == null)
            {
                GD.PrintErr("lyrics is null");
                return;
            }

            _lyrics = lyrics;
            _lyricsLines = _lyrics.Lines.Select(line => RhythmLyricsLine.Deserialize(line)).ToList();
            _lastIndex = 0;
        }

        public string Check(double nowTime)
        {
            if (_lyrics == null || _lyrics.Lines == null)
                return null;

            for (int i = _lastIndex; i < _lyricsLines.Count; ++i)
            {
                var line = _lyricsLines[i];
                if (line.BeginTime <= nowTime && nowTime <= line.EndTime)
                {
                    _lastIndex = i;
                    return line.KeyText;
                }
                else if (nowTime < line.BeginTime)
                {
                    // gap
                    _lastIndex = i;
                    return line.KeyText;
                }
            }
            return null;
        }
    }

    public class RhythmLyricsRecord()
    {
        public RhythmLyrics Lyrics { get; private set; }
        private List<RhythmLyricsLine> _lyricsLines;

        public bool Running { get; private set; }

        public void Start()
        {
            Running = true;

            Lyrics = new RhythmLyrics();
            _lyricsLines = new List<RhythmLyricsLine>();
        }

        public void Stop()
        {
            Running = false;

            Lyrics.Lines = _lyricsLines.Select(lyricsLine => lyricsLine.Serialize()).ToArray();
            _lyricsLines = null;
        }

        public void Tap(Key keyCode, double beginTime, double endTime)
        {
            GD.Print($"Record Tap {keyCode} {beginTime} {endTime}");
            if (!Running)
            {
                GD.PrintErr("is not running");
                return;
            }

            var lyricsLine = new RhythmLyricsLine()
            {
                // Todo 需要手动修改 Index 和 Text
                BeginTime = beginTime,
                EndTime = endTime,
                KeyText = keyCode.ToString(),
            };
            _lyricsLines.Add(lyricsLine);
        }
    }

    public class KeyboardRhythmMgr
    {
        public static RhythmLyrics Deserialize(string json)
        {
            return JsonSerializer.Deserialize<RhythmLyrics>(json);
        }

        public static string Serialize(RhythmLyrics lyrics)
        {
            return JsonSerializer.Serialize(lyrics);
        }
    }
}
