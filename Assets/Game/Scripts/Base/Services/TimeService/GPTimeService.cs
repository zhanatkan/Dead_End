#if UNITY_WEBGL && GAME_PUSH
using System;
using GamePush;

namespace Game.Scripts.Base.Services.TimeService
{
    public class GPTimeService : ITimeService
    {
        public DateTime GetTime() => GP_Server.Time();
    }
}
#endif