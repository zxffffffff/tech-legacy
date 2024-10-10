using Godot;
using System;
using System.Collections.Generic;
using Utils;

[Tool]
public partial class KeyboardRhythmPlay : Area2D
{
    [Export]
    public TextEdit PlayArea;

    [Export]
    public AudioStreamPlayer Audio { get; set; }

    [Export]
    public string AudioJsonPath { get; set; }

    [Export]
    public PackedScene KeyboardRhythmKeyTscn { get; set; }

    // record
    private RhythmLyricsRecord _lyricsRecord;
    private Dictionary<Key, double> _timeKeyDown;

    // play
    private List<RhythmLyricsLine> _lyricsLines;
    private Dictionary<int, KeyboardRhythmKey> _playKeys = new Dictionary<int, KeyboardRhythmKey>();
    private List<KeyboardRhythmKey> _recycleKeys = new List<KeyboardRhythmKey>();

    // cbk
    public delegate void PlayStopCbk(bool isFinished);
    public PlayStopCbk PlayStop { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Audio.Finished += OnAudioFinished;
        EventBus.Instance.KeyPress += OnKeyPressEvent;
    }

    public void Record()
    {
        _timeKeyDown = new Dictionary<Key, double>();
        _lyricsRecord = new RhythmLyricsRecord();
        _lyricsRecord.Start();

        Play();
    }

    public void Play()
    {
        var file = FileAccess.Open(AudioJsonPath, FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        GD.Print("==== 读取 json ====");
        GD.Print(json.Trim());
        GD.Print("==== 读取 json ====");
        var lyrics = KeyboardRhythmMgr.Deserialize(json);
        _lyricsLines = KeyboardRhythmMgr.DeserializeLines(lyrics.Lines);

        Audio.Play();
    }

    public void Stop(bool isFinished)
    {
        Audio.Stop();

        if (_lyricsRecord != null)
        {
            _lyricsRecord.Stop();
            var json = KeyboardRhythmMgr.Serialize(_lyricsRecord.Lyrics);
            json = json.Replace("[[", "[\n[");
            json = json.Replace("],[", "],\n[");
            json = json.Replace("]]", "]\n]");
            GD.Print("==== 需要手动拷贝到 json ====");
            GD.Print(json);
            GD.Print("==== 需要手动拷贝到 json ====");

            _lyricsRecord = null;
            _timeKeyDown = null;
        }

        _lyricsLines = null;

        PlayStop(isFinished);
    }

    private void OnAudioFinished()
    {
        Stop(true);
    }

    private double AudioTime()
    {
        // 参考 https://docs.godotengine.org/de/4.x/tutorials/audio/sync_with_audio.html
        double time = Audio.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
        time -= AudioServer.GetOutputLatency();
        return time;
    }

    private void OnKeyPressEvent(bool isPressed, Key keyCode)
    {
        if (keyCode == Key.None)
            return;

        if (_lyricsRecord != null)
        {
            var audio_time = AudioTime();
            if (isPressed)
                _timeKeyDown[keyCode] = audio_time;
            else
                _lyricsRecord.Tap(keyCode, _timeKeyDown[keyCode], audio_time);
        }
    }

    private KeyboardRhythmKey GetPlayKey(int hash, bool create)
    {
        var item = _playKeys[hash];
        if (create && item == null)
        {
            if (_recycleKeys.Count > 0)
            {
                item = _recycleKeys[0];
                _recycleKeys.RemoveAt(0);
            }
            else
            {
                item = KeyboardRhythmKeyTscn.Instantiate() as KeyboardRhythmKey;
                PlayArea.AddChild(item);
            }
            _playKeys[hash] = item;
        }
        return item;
    }

    private void RecyclePlayKey(int hash)
    {
        var item = _playKeys[hash];
        if (item == null)
        {
            GD.PrintErr("RecyclePlayKey is null");
            return;
        }
        _playKeys.Remove(hash);
        _recycleKeys.Add(item);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Audio.Playing)
        {
            var audio_time = AudioTime();
            const double show_time = 1.5f; // Todo
            var long_time = audio_time + show_time;
            for (int i = 0; i < _lyricsLines.Count; ++i)
            {
                var line = _lyricsLines[i];
                if (audio_time <= line.BeginTime && line.EndTime <= long_time)
                {
                    // show
                    var play_key = GetPlayKey(line.GetHashCode(), true);
                    play_key.Progress = (line.BeginTime - audio_time) / show_time;
                    var x = PlayArea.Position.X + PlayArea.Size.X / 2;
                    var y = PlayArea.Position.Y + PlayArea.Size.Y * play_key.Progress;
                    play_key.Position = new Vector2((float)x, (float)y);
                }
                else
                {
                    var play_key = GetPlayKey(line.GetHashCode(), false);
                    if (play_key != null)
                    {
                        // recycle
                        play_key.Progress = 0;
                        play_key.Position = new Vector2(-99999, -99999);
                        RecyclePlayKey(line.GetHashCode());
                    }
                }
            }
        }
    }
}
