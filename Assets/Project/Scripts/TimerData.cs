namespace Project
{
    using System;

    public class TimerData
    {
        public int Index { get; private set; }

        //TODO: тут заменить состояниe на emum
        public bool IsStarted { get; private set; }
        public bool IsFinished { get; private set; }

        public event Action OnTimeSpanChanged;
        public event Action OnFinish;

        private TimeSpan _timeSpan;
        private DateTime _fireTime;

        public TimerData(int index)
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
            IsFinished = false;

            _fireTime = GetTimeNow().Add(_timeSpan);
        }

        public void UpdateRemainingTime()
        {
            if (!IsStarted || IsFinished) return;

            var time = _fireTime - GetTimeNow();
            if (time > TimeSpan.Zero) return;

            time = TimeSpan.Zero;
            _timeSpan = TimeSpan.Zero;

            IsStarted = false;
            IsFinished = true;

            OnFinish?.Invoke();
        }

        public TimeSpan GetRemainingTime()
        {
            if (IsStarted)
                return _fireTime - GetTimeNow();

            return _timeSpan;
        }


        private static DateTime GetTimeNow() => DateTime.UtcNow;
    }
}