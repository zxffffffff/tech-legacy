using Godot;
using System;
using System.Collections.Generic;
using Utils;

[Tool]
public partial class KeyboardRhythmPlay : Node2D
{
    [Export]
    public Sprite2D PlayArea;

    [Export]
    public AudioStreamPlayer Audio { get; set; }

    [Export]
    public string AudioJsonPath { get; set; }

    [Export]
    public PackedScene KeyboardKeyTscn { get; set; }

    // record
    private RhythmLyricsRecord _lyricsRecord;
    private Dictionary<Key, double> _timeKeyDown;

    // play
    private List<RhythmLyricsLine> _lyricsLines;
    private Dictionary<int, KeyboardKey> _playKeys = new Dictionary<int, KeyboardKey>();
    private List<KeyboardKey> _recycleKeys = new List<KeyboardKey>();

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

    private KeyboardKey GetPlayKey(int hash, bool create)
    {
        KeyboardKey item;
        if (_playKeys.TryGetValue(hash, out item))
        {
            return item;
        }

        if (create)
        {
            if (_recycleKeys.Count > 0)
            {
                item = _recycleKeys[0];
                _recycleKeys.RemoveAt(0);
            }
            else
            {
                item = KeyboardKeyTscn.Instantiate() as KeyboardKey;
                item.Scale = new Vector2(2, 2);
                item.EnableInput = false;
                AddChild(item);
            }
            _playKeys.Add(hash, item);
        }
        return item;
    }

    private void RecyclePlayKey(int hash)
    {
        var item = GetPlayKey(hash, false);
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
            var area_w = PlayArea.Texture.GetSize().X * PlayArea.Scale.X;
            var area_h = PlayArea.Texture.GetSize().Y * PlayArea.Scale.Y;
            var area_x = PlayArea.Position.X - area_w / 2;
            var area_y = PlayArea.Position.Y - area_h / 2;

            var audio_time = AudioTime();
            const double show_time = 2f; // Todo
            var long_time = audio_time + show_time;
            for (int i = 0; i < _lyricsLines.Count; ++i)
            {
                var line = _lyricsLines[i];
                if (audio_time <= line.BeginTime && line.BeginTime <= long_time)
                {
                    // show
                    var play_key = GetPlayKey(line.GetHashCode(), true);
                    if (play_key.Progress == 0)
                    {
                        play_key.KeyCode = line.KeyCode;
                    }
                    play_key.Progress = (line.BeginTime - audio_time) / show_time;
                    var x = area_x + area_w / 2;
                    var y = area_y + area_h * (1 - play_key.Progress);
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
