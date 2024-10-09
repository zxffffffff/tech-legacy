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
    public AudioStreamPlayer Audio { get; set; }

    private double _timeBegin = 0;
    private double _timeDelay = 0;
    private double _timeNow = 0;

    private RhythmLyricsRecord _lyricsRecord;

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
        _lyricsRecord = new RhythmLyricsRecord();
        _lyricsRecord.Start();

        Play();
    }

    public void OnBtnPlay()
    {
        Play();
    }

    public void OnKeyPressEvent(bool isPressed, Key keyCode)
    {
        if (keyCode == Key.None)
            return;

        if (_lyricsRecord != null)
        {
            // Todo
        }
    }

    public void OnAudioFinished()
    {
        if (_lyricsRecord != null)
        {
            _lyricsRecord.Stop();
            // Todo
            _lyricsRecord = null;
        }

        // Todo

        Stop();
    }

    private void Play()
    {
        BtnCreate.Visible = false;
        BtnPlay.Visible = false;

        _timeBegin = Time.GetTicksUsec();
        _timeDelay = AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency();
        _timeNow = 0;

        Audio.Play();
    }

    public void Stop()
    {
        Audio.Stop();

        _timeBegin = 0;
        _timeDelay = 0;
        _timeNow = 0;

        BtnCreate.Visible = true;
        BtnPlay.Visible = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Audio.Playing)
        {
            // 同步游戏音频及音乐 https://docs.godotengine.org/zh-cn/4.x/tutorials/audio/sync_with_audio.html
            // 使用系统时钟同步，适合节奏游戏
            double time = (Time.GetTicksUsec() - _timeBegin) / 1000000.0d;
            _timeNow = Math.Max(0.0d, time - _timeDelay);
        }
    }
}
