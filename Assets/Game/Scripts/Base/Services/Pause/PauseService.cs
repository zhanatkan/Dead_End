using System.Collections.Generic;
using VContainer;

namespace Game.Scripts.Base.Services.Pause
{
    public sealed class PauseService : IPauseService
    {
        private readonly List<IPauseHandler> _handlers = new();

        [Inject]
        public PauseService()
        {
            
        }
        
        public void Register(IPauseHandler pauseHandler)
        {
            _handlers.Add(pauseHandler);
        }

        public void Unregister(IPauseHandler pauseHandler)
        {
            _handlers.Remove(pauseHandler);
        }

        public void CleanUp()
        {
            _handlers.Clear();
        }

        public void SetPause(bool isPaused)
        {
            foreach (var handler in _handlers)
            {
                handler.OnPauseChanged(isPaused);
            }
        }
    }
}