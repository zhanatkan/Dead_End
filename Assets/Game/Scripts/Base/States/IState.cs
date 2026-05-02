namespace Game.Scripts.Base.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<T> : IExitableState
    {
        void Enter(T payload);
    }

    public interface IExitableState
    {
        void Exit();
    }
}