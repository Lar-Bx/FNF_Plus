using System;

namespace FNF_plus.Tool.Time;

public class TimerCustom
{
    public float TimeScale { get; set; } = 1f;
    private long _waitTime = -1;
    private long _currentTime = 0;
    private int _loopCount = 0;

    public delegate void OnCompletedLoopEventHandler();
    public event OnCompletedLoopEventHandler OnCompletedLoop;
    
    public void Update(float deltaTime)
    {
        _currentTime += (long)Math.Round(deltaTime * TimeScale * 1000);
        if (_currentTime >= _waitTime && _waitTime > 0)
        {
            _loopCount++;
            Math.DivRem(_currentTime, _waitTime, out _currentTime);
            OnCompletedLoop?.Invoke();
        }
    }

    public void Reset()
    {
        _currentTime = 0;
        _loopCount = 0;
    }

    public long GetCurrentTime() => _currentTime;
    
    public long GetWaitTime() => _waitTime;
    
    public int GetLoopCount() => _loopCount;
    
    public void SetWaitTime(long waitTime) => _waitTime = waitTime;
    
    public float GetRealTime() => _currentTime / 1000f;
     
    
}