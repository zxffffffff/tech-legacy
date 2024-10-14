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

    // [0]--[show]--[tap]--[miss]-->
    [Export]
    double PlayAreaShowTime { get; set; } = 3.5f;
    [Export]
    double PlayAreaTapTime { get; set; } = 0.6f;
    [Export]
    double PlayAreaMissTime { get; set; } = 0.3f;

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

        _playAreaH = PlayArea.Texture.GetSize().Y * PlayArea.Scale.Y * 0.9f;
        _PlayAreaTop = PlayArea.Position.Y - _playAreaH / 2;
        _playAreaKeyX = new Dictionary<Key, float>();
        // GD.Print($"_PlayAreaTop={_PlayAreaTop}, _playAreaH={_playAreaH}");
        foreach (var row_keys in PlayKeyboard.Keys)
        {
            foreach (var key in row_keys)
            {
                _playAreaKeyX[key.KeyCode] = PlayKeyboard.Position.X + key.Position.X * PlayKeyboard.Scale.X;
                // GD.Print($"_playAreaKeyX[{key.KeyCode}]={_playAreaKeyX[key.KeyCode]}");
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
        int count = _lyricsWords != null ? _lyricsWords.Count : 0;
        PlayScore.Text = $"{_lyricsScore}/{count}";
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
        item.Modulate = new Color(item.Modulate.R, item.Modulate.G, item.Modulate.B, 0);
        item.Position = new Vector2(-99999, -99999);
        _playKeys.Remove(hash);
        _recycleKeys.Add(item);
    }

    private double CheckShowProgress(double lyrics_time, double now_time)
    {
        // [0]--[show]--[now]--[miss]-->
        var show_time = lyrics_time - PlayAreaShowTime;
        var miss_time = lyrics_time + PlayAreaMissTime / 2;
        // 0~100%
        var progress = (now_time - show_time) / (miss_time - show_time);
        return progress;
    }

    private double CheckTapProgress(double lyrics_time, double now_time)
    {
        // [0]--[tap]--[now]--[miss]-->
        var tap_time = lyrics_time - PlayAreaTapTime;
        var miss_time = lyrics_time + PlayAreaMissTime / 2;
        // 0~100%
        var progress = (now_time - tap_time) / (miss_time - tap_time);
        return progress;
    }

    private void OnKeyPressEvent(bool isPressed, Key keyCode)
    {
        if (keyCode == Key.None)
            return;

        var now_time = AudioTime();

        // record
        if (_lyricsRecord != null)
        {
            if (isPressed)
                _timeKeyDown[keyCode] = now_time;
            else
                _lyricsRecord.Tap(keyCode, _timeKeyDown[keyCode], now_time);
        }

        // play tap
        if (_fsm.State == RhythmPlayState.Playing && isPressed)
        {
            for (int i = 0; i < _lyricsWords.Count; ++i)
            {
                var word = _lyricsWords[i];
                var play_key = GetPlayKey(word.GetHashCode(), false);
                if (play_key != null && play_key.Visible)
                {
                    var tap_progress = CheckTapProgress(word.BeginTime, now_time);
                    if (0 <= tap_progress && tap_progress <= 1)
                    {
                        if (word.KeyCode == keyCode)
                        {
                            // tap
                            GD.Print($" tap {word.Text} {word.KeyCode}");
                            _lyricsScore += 1;
                            UpdatePlayScore();
                            play_key.Hide();
                            break;
                        }
                        else
                        {
                            // lose
                            GD.Print($"lose {word.Text} {word.KeyCode} != {keyCode}");
                            play_key.Hide();
                            continue;
                        }
                    }
                }
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_fsm.State == RhythmPlayState.Playing)
        {
            var now_time = AudioTime();

            for (int i = 0; i < _lyricsWords.Count; ++i)
            {
                var word = _lyricsWords[i];
                var show_progress = CheckShowProgress(word.BeginTime, now_time);
                if (0 <= show_progress && show_progress <= 1)
                {
                    var play_key = GetPlayKey(word.GetHashCode(), true);
                    if (play_key.Modulate.A == 0)
                    {
                        // show
                        GD.Print($"show {word.Text} {word.KeyCode}");
                        play_key.KeyCode = word.KeyCode;
                        play_key.IsPressed = false;
                        play_key.Show();
                    }
                    var x = _playAreaKeyX[play_key.KeyCode];
                    var y = _PlayAreaTop + _playAreaH * show_progress;
                    play_key.Modulate = new Color(play_key.Modulate.R, play_key.Modulate.G, play_key.Modulate.B, (float)show_progress);
                    play_key.Position = new Vector2((float)x, (float)y);

                    var tap_progress = CheckTapProgress(word.BeginTime, now_time);
                    if (0 <= tap_progress && tap_progress <= 1)
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
                        if (play_key.Visible)
                        {
                            // miss
                            GD.Print($"miss {word.Text} {word.KeyCode}");
                            _lyricsScore += 0;
                            UpdatePlayScore();
                            play_key.Hide();
                        }
                        RecyclePlayKey(word.GetHashCode());
                    }
                }
            }
        }
    }
}
