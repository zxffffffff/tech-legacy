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
        public float TimeSec { get; set; }
        public string[][] Lines { get; set; }
    }

    public class RhythmLyricsLine
    {
        public int Index { get; set; }
        public float BeginTime { get; set; }
        public float EndTime { get; set; }
        public string Text { get; set; }
        public string KeyText { get; set; }

        public static int FieldCount { get; } = 5;

        public void Deserialize(string[] line)
        {
            if (line.Count() < FieldCount)
            {
                GD.PrintErr("invalid line");
                return;
            }

            Index = line[0].ToInt();
            BeginTime = line[1].ToFloat();
            EndTime = line[2].ToFloat();
            Text = line[3];
            KeyText = line[4];
        }

        public string[] Serialize()
        {
            return [
                Index.ToString(),
                BeginTime.ToString("G9"),
                EndTime.ToString("G9"),
                Text,
                KeyText,
            ];
        }
    }

    public class RhythmLyricsRecord()
    {
        public RhythmLyrics Lyrics { get; private set; }

        public List<RhythmLyricsLine> LyricsLines { get; private set; }

        public bool Running { get; private set; }

        public void Start()
        {
            Running = true;

            Lyrics = new RhythmLyrics();
            LyricsLines = new List<RhythmLyricsLine>();
        }

        public void Stop()
        {
            Running = false;

            Lyrics.Lines = LyricsLines.Select(lyricsLine => lyricsLine.Serialize()).ToArray();
        }

        public void Tap(string keyText, float beginTime, float endTime)
        {
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
                KeyText = keyText,
            };
            LyricsLines.Add(lyricsLine);
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
