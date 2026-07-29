using FNF_plus.Resource.AnimatedCustom;
using FNF_plus.Tool.Time;

namespace FNF_plus.Base.Visual.AnimatedCustom;

using Godot;

[GlobalClass]
public partial class AnimatedCustomPlayer : Node2D
{
    [Export]
    public AnimatedSet AnimatedSet { get; set; }
    [Export]
    public float TimeScale { get; set; } = 1f;
    [Export]
    public bool Loop { get; set; } = true;
    
    private AnimatedCollection _currentCollection;
    private int _currentFrame = 0;
    private readonly TimerCustom _timer = new TimerCustom();
    private bool _playing = false;
    
    [Signal]
    public delegate void AnimationLoopFinishedEventHandler();
    
    
    public AnimatedCustomPlayer() : base()
    {
        _timer.OnCompletedLoop += FrameFinishHandle;
    }
    
    
    public Error Play(StringName name, int from = 0)
    {
        if (AnimatedSet == null) return Error.InvalidData;
        
        _timer.Reset();
        _timer.SetWaitTime((long)(1000f / AnimatedSet.GetCollection(name).Fps));
        _currentCollection = AnimatedSet.GetCollection(name);
        _currentFrame = from;
        _playing = true;
        return Error.Ok;
    }

    public Error Pause()
    {
        if (AnimatedSet == null) return Error.InvalidData;
        _playing = false;
        return Error.Ok;
    }

    public Error Resume()
    {
        if (AnimatedSet == null || _currentCollection == null) return Error.InvalidData;
        Play(_currentCollection.Name);
        _playing = true;
        return Error.Ok;
    }

    public Error Stop()
    {
        if (AnimatedSet == null) return Error.InvalidData;
        _currentCollection = null;
        _timer.Reset();
        _currentFrame = 0;
        _playing = false;
        return Error.Ok;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (AnimatedSet == null || _currentCollection == null) return;
        _timer.TimeScale = TimeScale;
        _timer.Update((float)delta);
    }


    public void FrameFinishHandle()
    {
        _currentFrame++;
        if (_currentFrame >= _currentCollection.Frames.Length)
        {
            _currentFrame = 0;
            QueueRedraw();
            EmitSignal(SignalName.AnimationLoopFinished);
        }
    }

    public override void _Draw()
    {
        base._Draw();
        
        DrawTexture(_currentCollection.Frames[_currentFrame], Vector2.Zero);
        
    }
}