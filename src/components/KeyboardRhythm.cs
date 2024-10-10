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
    public KeyboardRhythmPlay RhythmPlay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        BtnBack.Pressed += OnBtnBack;
        BtnCreate.Pressed += Record;
        BtnPlay.Pressed += Play;
        RhythmPlay.PlayStop += OnPlayStop;
    }

    public void OnBtnBack()
    {
        if (BtnPlay.Visible)
        {
            Common.Instance.EmitCommonSignal("Home");
        }
        else
        {
            RhythmPlay.Stop(false);
        }
    }

    public void Record()
    {
        BtnCreate.Visible = false;
        BtnPlay.Visible = false;

        RhythmPlay.Record();
    }

    public void Play()
    {
        BtnCreate.Visible = false;
        BtnPlay.Visible = false;

        RhythmPlay.Play();
    }

    private void OnPlayStop(bool isFinished)
    {
        BtnCreate.Visible = true;
        BtnPlay.Visible = true;
    }
}
