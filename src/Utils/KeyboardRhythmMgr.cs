using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Utils
{
    public class RhythmLyrics
    {
        public string Title { get; set; } // ti 标题
        public string Artist { get; set; } // ar 歌手
        public string Author { get; set; } // au 作曲
        public string Lyricists { get; set; } // lr 作词
        public string Album { get; set; } // al 专辑
        public string Length { get; set; } // length (mm:ss)

        public string[][] Words { get; set; } // <mm:ss.xx> 1 <mm:ss.xx> 2 <mm:ss.xx>
    }

    public class RhythmLyricsWord
    {
        public int Index { get; set; }
        public double BeginTime { get; set; }
        public double EndTime { get; set; }
        public string Text { get; set; }
        public Key KeyCode { get; set; }

        public static int FieldCount { get; } = 5;

        public string[] Serialize()
        {
            return [
                Index.ToString(),
                BeginTime.ToString("0.00"),
                EndTime.ToString("0.00"),
                Text,
                KeyboardKeyMgr.KeyCodeToString(KeyCode),
            ];
        }

        public static RhythmLyricsWord Deserialize(string[] words)
        {
            if (words.Count() < FieldCount)
            {
                GD.PrintErr("invalid line");
                return null;
            }

            var lyricsLine = new RhythmLyricsWord()
            {
                Index = words[0].ToInt(),
                BeginTime = double.Parse(words[1]),
                EndTime = double.Parse(words[2]),
                Text = words[3],
                KeyCode = KeyboardKeyMgr.StringToKeyCode(words[4]),
            };
            return lyricsLine;
        }
    }

    public class RhythmLyricsRecord()
    {
        public RhythmLyrics Lyrics { get; private set; }
        private List<RhythmLyricsWord> _lyricsWords;

        public bool Running { get; private set; }

        public void Start()
        {
            Running = true;

            Lyrics = new RhythmLyrics();
            _lyricsWords = new List<RhythmLyricsWord>();
        }

        public void Stop()
        {
            Running = false;

            Lyrics.Words = _lyricsWords.Select(lyricsLine => lyricsLine.Serialize()).ToArray();
            _lyricsWords = null;
        }

        public void Tap(Key keyCode, double beginTime, double endTime)
        {
            GD.Print($"Record Tap {keyCode} {beginTime} {endTime}");
            if (!Running)
            {
                GD.PrintErr("is not running");
                return;
            }

            var lyricsWord = new RhythmLyricsWord()
            {
                // Todo 需要手动修改 Index 和 Text
                BeginTime = beginTime,
                EndTime = endTime,
                KeyCode = keyCode,
            };
            _lyricsWords.Add(lyricsWord);
        }
    }

    public enum RhythmPlayState
    {
        Init,
        Playing,
        Pausing,
        Settlement,
    }

    public enum RhythmPlayTrigger
    {
        Play,
        Record,
        Pause,
        PauseResume,
        GG,
        Clear,
    }

    public class KeyboardRhythmMgr
    {
        public static string Serialize(RhythmLyrics lyrics)
        {
            return JsonSerializer.Serialize(lyrics);
        }

        public static RhythmLyrics Deserialize(string json)
        {
            return JsonSerializer.Deserialize<RhythmLyrics>(json);
        }

        public static List<RhythmLyricsWord> DeserializeWords(string[][] words)
        {
            if (words == null)
            {
                GD.PrintErr("DeserializeWords is null");
                return null;
            }
            return words.Select(word => RhythmLyricsWord.Deserialize(word)).ToList();
        }
    }
}
