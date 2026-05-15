namespace Game.Scripts.Base.Services.Pause
{
    public interface IPauseService
    {
        void Register(IPauseHandler handler);
        void Unregister(IPauseHandler handler);
        void CleanUp();
        void SetPause(bool isPaused);
    }
}