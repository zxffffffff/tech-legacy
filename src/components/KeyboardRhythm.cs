using Godot;
using System;
using System.Collections.Generic;
using Utils;

public partial class KeyboardRhythm : Node2D
{
    [Export]
    public Button BtnBack;

    [Export]
    public Button BtnCreate;

    [Export]
    public Button BtnPlay;

    [Export]
    public TextEdit TextLyrics;

    [Export]
    public AudioStreamPlayer Audio { get; set; }

    [Export]
    public string AudioJsonPath { get; set; }

    private RhythmLyricsPlay _lyricsPlay;

    private RhythmLyricsRecord _lyricsRecord;
    private Dictionary<Key, double> _timeKeyDown;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        BtnBack.Pressed += OnBtnBack;
        BtnCreate.Pressed += OnBtnCreate;
        BtnPlay.Pressed += OnBtnPlay;
        Audio.Finished += OnAudioFinished;

        EventBus.Instance.KeyPress += OnKeyPressEvent;
    }

    public void OnBtnBack()
    {
        if (BtnPlay.Visible)
        {
            Common.Instance.EmitCommonSignal("Home");
        }
        else
        {
            Stop();
        }
    }

    public void OnBtnCreate()
    {
        _timeKeyDown = new Dictionary<Key, double>();
        _lyricsRecord = new RhythmLyricsRecord();
        _lyricsRecord.Start();

        Play();
    }

    public void OnBtnPlay()
    {
        Play();
    }

    private void Play()
    {
        BtnCreate.Visible = false;
        BtnPlay.Visible = false;

        var file = FileAccess.Open(AudioJsonPath, FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        GD.Print("==== 歌词 ====");
        GD.Print(json.Trim());
        GD.Print("==== 歌词 ====");
        var lyrics = KeyboardRhythmMgr.Deserialize(json);
        _lyricsPlay = new RhythmLyricsPlay();
        _lyricsPlay.Start(lyrics);

        Audio.Play();
    }

    public void Stop()
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

        _lyricsPlay = null;

        BtnCreate.Visible = true;
        BtnPlay.Visible = true;
    }

    private double AudioTime()
    {
        // 参考 https://docs.godotengine.org/de/4.x/tutorials/audio/sync_with_audio.html
        double time = Audio.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
        time -= AudioServer.GetOutputLatency();
        return time;
    }

    public void OnKeyPressEvent(bool isPressed, Key keyCode)
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

    public void OnAudioFinished()
    {
        Stop();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Audio.Playing)
        {
            var audio_time = AudioTime();
            var text = _lyricsPlay.Check(audio_time);
            if (!string.IsNullOrEmpty(text))
            {
                if (!TextLyrics.Text.EndsWith(text))
                {
                    TextLyrics.Text += "\n" + text;
                    TextLyrics.ScrollVertical = int.MaxValue;
                }
            }
        }
    }
}
