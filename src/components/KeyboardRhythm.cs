using Godot;
using System;
using System.Collections.Generic;
using Utils;

public partial class KeyboardRhythm : Node2D
{
    private Button btnCreate;
    private Button btnPlay;

    private AudioStreamPlayer2D audioStreamPlayer2D;
    private double _timeBegin = 0;
    private double _timeDelay = 0;
    private delegate void AudioStreamPlayCbk(double time);
    private AudioStreamPlayCbk audioStreamPlayCbk;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        var btnBack = GetNode<Button>("Control/ButtonBack");
        btnBack.Pressed += OnBtnBack;

        btnCreate = GetNode<Button>("Control/ButtonCreate");
        btnCreate.Pressed += OnBtnCreate;

        btnPlay = GetNode<Button>("Control/ButtonPlay");
        btnPlay.Pressed += OnBtnPlay;

        audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    }

    public void OnBtnBack()
    {
        if (btnPlay.Visible)
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
        Play((double time) => { });
    }

    public void OnBtnPlay()
    {
        Play((double time) => { });
    }

    private void Play(AudioStreamPlayCbk cbk)
    {
        btnCreate.Visible = false;
        btnPlay.Visible = false;

        _timeBegin = Time.GetTicksUsec();
        _timeDelay = AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency();
        audioStreamPlayer2D.Play();
        audioStreamPlayCbk = cbk;
    }

    public void Stop()
    {
        btnCreate.Visible = true;
        btnPlay.Visible = true;

        _timeBegin = 0;
        _timeDelay = 0;
        audioStreamPlayer2D.Stop();
        audioStreamPlayCbk = null;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (audioStreamPlayCbk != null)
        {
            double time = (Time.GetTicksUsec() - _timeBegin) / 1000000.0d;
            time = Math.Max(0.0d, time - _timeDelay);
            audioStreamPlayCbk(time);
        }
    }
}
