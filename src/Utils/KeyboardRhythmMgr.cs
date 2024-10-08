using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Utils
{
    public class RhythmLyricsLine
    {
        public int Index { get; set; }
        public float BeginTime { get; set; }
        public float EndTime { get; set; }
        public string Text { get; set; }
        public string KeyText { get; set; }
    }

    public class RhythmLyrics
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float TimeSec { get; set; }
        public List<RhythmLyricsLine> Lines { get; set; }
    }

    public class KeyboardRhythmMgr
    {
        static RhythmLyrics Deserialize(string json)
        {
            var lyrics = JsonConvert.DeserializeObject<RhythmLyrics>(json);
            return lyrics;
        }

        static string Serialize(RhythmLyrics lyrics)
        {
            string json = JsonConvert.SerializeObject(lyrics);
            return json;
        }
    }
}
