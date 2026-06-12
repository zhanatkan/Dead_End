using System;
using System.Collections.Generic;

namespace Game.Scripts.Base.Services.Timer
{
    public class TimerService : ITimerService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Dictionary<string, Timer> _timers = new();

        public TimerService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public Timer CreateTimer(string name, float time = 0, Action onTimerEnd = null, Action<float> onTimerChange = null,
            bool repeat = false)
        {
            var timer = new Timer(_coroutineRunner, time, onTimerEnd, onTimerChange, repeat);
            _timers.Add(name, timer);

            return timer;
        }

        public void RemoveTimer(string name)
        {
            var timer = _timers[name];
            timer?.StopTimer();
            _timers.Remove(name);
        }

        public Timer GetTimer(string name) => _timers[name];
        
        public Dictionary<string, Timer> GetAllTimers()
        {
            return new Dictionary<string, Timer>(_timers);
        }
    }
}