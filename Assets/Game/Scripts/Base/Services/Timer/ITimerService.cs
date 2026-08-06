using System;
using System.Collections.Generic;

namespace Game.Scripts.Base.Services.Timer
{
    public interface ITimerService
    {
        Timer CreateTimer(string name, float time = 0, Action onTimerEnd = null, Action<float> onTimerChange = null,
            bool repeat = false);
        void RemoveTimer(string name);
        Timer GetTimer(string name);
        Dictionary<string, Timer> GetAllTimers();
    }
}