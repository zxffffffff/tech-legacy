using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;

namespace Utils
{
    // csv
    public class RhythmLyricsLine
    {
        public int Index { get; set; }
        public float BeginTime { get; set; }
        public float EndTime { get; set; }
        public string Text { get; set; }
        public string KeyText { get; set; }
    }

    // json
    public class RhythmLyrics
    {
        public string Lang { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public float TimeSec { get; set; }

        [JsonIgnore]
        public List<RhythmLyricsLine> Lines { get; set; }
    }

    public class KeyboardRhythmUtils
    {
        static RhythmLyrics Deserialize(string json_str, string csv_str)
        {
            var lyrics = JsonConvert.DeserializeObject<RhythmLyrics>(json_str);

            using (var reader = new StringReader(csv_str))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<RhythmLyricsLine>();
                lyrics.Lines = records.ToList();
            }

            return lyrics;
        }

        static (string json_str, string csv_str) Serialize(RhythmLyrics lyrics)
        {
            string json_str = JsonConvert.SerializeObject(lyrics);

            string csv_str = "";
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(lyrics.Lines);
                writer.Flush();
                stream.Position = 0;
                csv_str = reader.ReadToEnd();
            }

            return (json_str, csv_str);
        }
    }
}
