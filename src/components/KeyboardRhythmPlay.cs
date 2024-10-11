using Godot;
using Stateless;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Utils;

[Tool]
public partial class KeyboardRhythmPlay : Node2D
{
    [Export]
    public Label PlayScore;

    [Export]
    public Sprite2D PlayArea;

    [Export]
    public Keyboard PlayKeyboard;

    [Export]
    public AudioStreamPlayer Audio { get; set; }

    [Export]
    public string AudioJsonPath { get; set; }

    [Export]
    public PackedScene KeyboardKeyTscn { get; set; }

    private StateMachine<RhythmPlayState, RhythmPlayTrigger> _fsm = new StateMachine<RhythmPlayState, RhythmPlayTrigger>(RhythmPlayState.Init);

    // record
    private RhythmLyricsRecord _lyricsRecord;
    private Dictionary<Key, double> _timeKeyDown;

    // play
    private List<RhythmLyricsWord> _lyricsWords;
    private int _lyricsScore = 0;
    private Dictionary<int, KeyboardKey> _playKeys = new Dictionary<int, KeyboardKey>();
    private List<KeyboardKey> _recycleKeys = new List<KeyboardKey>();

    float _playAreaH = 0;
    float _PlayAreaTop = 0;
    Dictionary<Key, float> _playAreaKeyX;

    [Export]
    double PlayAreaShowTime { get; set; } = 2.5f;
    [Export]
    double PlayAreaPressTime { get; set; } = 0.5f;

    // cbk
    public delegate void FinishedCbk();
    public event FinishedCbk Finished;

    public override void _Ready()
    {
        base._Ready();

        Audio.Finished += OnAudioFinished;
        PlayKeyboard.KeyPressCbk += OnKeyPressEvent;

        _fsm.Configure(RhythmPlayState.Init)
            .Permit(RhythmPlayTrigger.Play, RhythmPlayState.Playing);

        _fsm.Configure(RhythmPlayState.Playing)
            .OnEntry(OnPlay)
            .InternalTransition(RhythmPlayTrigger.Record, OnRecord)
            .Permit(RhythmPlayTrigger.Pause, RhythmPlayState.Pausing)
            .Permit(RhythmPlayTrigger.GG, RhythmPlayState.Settlement);

        _fsm.Configure(RhythmPlayState.Pausing)
            .SubstateOf(RhythmPlayState.Playing)
            .OnEntry(OnEntryPausing)
            .OnExit(OnExitPausing)
            .Permit(RhythmPlayTrigger.PauseResume, RhythmPlayState.Playing);

        _fsm.Configure(RhythmPlayState.Settlement)
            .OnEntry(OnSettlement)
            .OnExit(OnSettlementClear)
            .Permit(RhythmPlayTrigger.Clear, RhythmPlayState.Init);
    }

    public void Action(RhythmPlayTrigger trigger)
    {
        _fsm.Fire(trigger);
    }

    private void OnPlay()
    {
        var file = FileAccess.Open(AudioJsonPath, FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        GD.Print("==== 读取 json ====");
        GD.Print(json.Trim());
        GD.Print("==== 读取 json ====");
        var lyrics = KeyboardRhythmMgr.Deserialize(json);
        _lyricsWords = KeyboardRhythmMgr.DeserializeWords(lyrics.Words);
        _lyricsScore = 0;

        _playAreaH = PlayArea.Texture.GetSize().Y * PlayArea.Scale.Y;
        _PlayAreaTop = PlayArea.Position.Y - _playAreaH / 2;
        _playAreaKeyX = new Dictionary<Key, float>();
        foreach (var row_keys in PlayKeyboard.Keys)
        {
            foreach (var key in row_keys)
            {
                _playAreaKeyX[key.KeyCode] = PlayKeyboard.Position.X + key.Position.X * PlayKeyboard.Scale.X;
            }
        }

        UpdatePlayScore();
        Audio.Play();
    }

    private void OnRecord()
    {
        _lyricsRecord = new RhythmLyricsRecord();
        _lyricsRecord.Start();
        _timeKeyDown = new Dictionary<Key, double>();
    }

    private void OnEntryPausing()
    {
        // Todo
    }

    private void OnExitPausing()
    {
        // Todo
    }

    private void OnSettlement()
    {
        Audio.Stop();

        // Todo

        if (Finished != null)
            Finished();
    }

    private void OnSettlementClear()
    {
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

        _lyricsWords = null;
        _lyricsScore = 0;

        Debug.Assert(_playKeys.Count == 0);

        _playAreaH = 0;
        _PlayAreaTop = 0;
        _playAreaKeyX = null;
    }

    private void OnAudioFinished()
    {
        Action(RhythmPlayTrigger.GG);
    }

    private void UpdatePlayScore()
    {
        int score = 1000000 * _lyricsScore / _lyricsWords.Count;
        PlayScore.Text = score.ToString();
    }

    private double AudioTime()
    {
        // 参考 https://docs.godotengine.org/de/4.x/tutorials/audio/sync_with_audio.html
        double time = Audio.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
        time -= AudioServer.GetOutputLatency();
        return time;
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
                item.Modulate = new Color(item.Modulate.R, item.Modulate.G, item.Modulate.B, 0);
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

    private bool CheckTapKey(RhythmLyricsWord word, double audio_time)
    {
        var start_time = audio_time - PlayAreaPressTime / 2;
        var end_time = audio_time + PlayAreaPressTime / 2;
        return start_time <= word.BeginTime && word.BeginTime <= end_time;
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

        if (_fsm.State == RhythmPlayState.Playing && isPressed)
        {
            var audio_time = AudioTime();
            for (int i = 0; i < _lyricsWords.Count; ++i)
            {
                var word = _lyricsWords[i];
                if (CheckTapKey(word, audio_time))
                {
                    var play_key = GetPlayKey(word.GetHashCode(), false);
                    if (play_key == null)
                    {
                        GD.PrintErr("GetPlayKey tap is null");
                        continue;
                    }

                    // tap
                    _lyricsScore += 1;
                    UpdatePlayScore();

                    play_key.Modulate = new Color(play_key.Modulate.R, play_key.Modulate.G, play_key.Modulate.B, 0);
                    play_key.Position = new Vector2(-99999, -99999);
                    RecyclePlayKey(word.GetHashCode());
                }
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_fsm.State == RhythmPlayState.Playing)
        {
            var audio_time = AudioTime();
            var long_time = audio_time + PlayAreaShowTime;
            for (int i = 0; i < _lyricsWords.Count; ++i)
            {
                var word = _lyricsWords[i];
                if (audio_time <= word.BeginTime && word.BeginTime <= long_time)
                {
                    // progress
                    var play_key = GetPlayKey(word.GetHashCode(), true);
                    if (play_key.Modulate.A == 0)
                    {
                        play_key.KeyCode = word.KeyCode;
                        play_key.IsPressed = false;
                    }
                    var progress = (word.BeginTime - audio_time) / PlayAreaShowTime;
                    var x = _playAreaKeyX[play_key.KeyCode];
                    var y = _PlayAreaTop + _playAreaH * (1 - progress);
                    play_key.Modulate = new Color(play_key.Modulate.R, play_key.Modulate.G, play_key.Modulate.B, (float)(1 - progress));
                    play_key.Position = new Vector2((float)x, (float)y);

                    if (CheckTapKey(word, audio_time))
                    {
                        // can tap
                        play_key.IsPressed = true;
                    }
                }
                else
                {
                    var play_key = GetPlayKey(word.GetHashCode(), false);
                    if (play_key != null)
                    {
                        // miss
                        _lyricsScore += 0;
                        UpdatePlayScore();

                        play_key.Modulate = new Color(play_key.Modulate.R, play_key.Modulate.G, play_key.Modulate.B, 0);
                        play_key.Position = new Vector2(-99999, -99999);
                        RecyclePlayKey(word.GetHashCode());
                    }
                }
            }
        }
    }
}
