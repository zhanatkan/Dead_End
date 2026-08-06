using System;

namespace Game.Scripts.Base.Services.TimeService
{
    public class MockTimeService : ITimeService
    {
        public DateTime GetTime() => DateTime.Now;
    }
}