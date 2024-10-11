using Godot;
using System;
using System.Collections.Generic;
using Utils;

public partial class KeyboardRhythm : Node2D
{
    [Export]
    public Button BtnBack;

    [Export]
    public Button BtnPlay;

    [Export]
    public Button BtnRecord;

    [Export]
    public KeyboardRhythmPlay RhythmPlay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        BtnBack.Pressed += OnBtnBack;
        BtnPlay.Pressed += () => Play();
        BtnRecord.Pressed += () => Record();

        RhythmPlay.Finished += OnPlayStop;
    }

    public void OnBtnBack()
    {
        if (BtnPlay.Visible)
        {
            Common.Instance.EmitCommonSignal("Home");
        }
        else
        {
            RhythmPlay.Action(RhythmPlayTrigger.GG);
        }
    }

    public void Play()
    {
        BtnRecord.Visible = false;
        BtnPlay.Visible = false;

        RhythmPlay.Visible = true;
        RhythmPlay.Action(RhythmPlayTrigger.Play);
    }

    public void Record()
    {
        Play();
        RhythmPlay.Action(RhythmPlayTrigger.Record);
    }

    private void OnPlayStop()
    {
        BtnRecord.Visible = true;
        BtnPlay.Visible = true;

        RhythmPlay.Visible = false;
        RhythmPlay.Action(RhythmPlayTrigger.Clear);
    }
}
