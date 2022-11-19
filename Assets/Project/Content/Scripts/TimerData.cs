using System;

public class TimerData
{
    public int Index { get; private set; }   
    public bool IsStarted { get; private set; }   
    public bool IsFinished { get; private set; }
   
    public event Action OnOver;
    public event Action OnTimeSpanChanged;
    public event Action OnFinish;

    private TimeSpan _timeSpan;
    private DateTime _fireTime;

    public TimerData (int index)
    {
        Index = index;
    }


    public void Decrease(TimeSpan ts)
    {
        _timeSpan = _timeSpan.Subtract(ts);
        if (_timeSpan < TimeSpan.Zero)
            _timeSpan = TimeSpan.Zero;

        OnTimeSpanChanged?.Invoke();
    }

    public void Increase(TimeSpan ts)
    {
        _timeSpan = _timeSpan.Add(ts);
        OnTimeSpanChanged?.Invoke();
    }

    public void Start()
    {
        IsStarted = true;

        _fireTime = GetTimeNow().Add(_timeSpan);
    }

    public TimeSpan GetRemainingTime()
    {
        if (IsStarted)
        {
            var time = _fireTime - GetTimeNow();
            if (time <= TimeSpan.Zero)
            {
                time = TimeSpan.Zero;
                _timeSpan = TimeSpan.Zero;

                IsStarted = false;
                IsFinished = true;
                OnFinish?.Invoke();
            }

            return time;
        }

        return _timeSpan;
    }

    private static DateTime GetTimeNow() => DateTime.UtcNow;

}
