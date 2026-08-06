using CharacterController = Game.Scripts.Game.Character.Base.CharacterController;

namespace Game.Scripts.EventBus 
{
    public struct OnPlayerDeath : IEvent
    {
        public CharacterController Character;

        public OnPlayerDeath(CharacterController character)
        {
            Character = character;
        }
    }

    public struct OnAdWatched : IEvent
    {
        public bool Success;

        public OnAdWatched(bool success)
        {
            Success = success;
        }
    }

    public struct OnQuitGame : IEvent {}
    public struct OnContinueGame : IEvent {}
}