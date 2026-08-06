using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Game.AIScripts.Behaviour;
using Game.Scripts.Game.AIScripts.Player;
using Game.Scripts.Game.AIScripts.Health;
using Game.Scripts.Settings;
using Game.Scripts.Game.AIScripts.Spawn;
using Game.Scripts.Game.Character.Player;
using UnityEngine;
using Leopotam.Ecs;
using VContainer;

namespace Game.Scripts.Game.AIScripts.Common
{
    public class EcsCreator : MonoBehaviour
    {
        private BotsSetting _botsSetting;
        private PlayerController _playerController;
        
        public EcsWorld EcsWorld {get; private set;}
        
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        [Inject]
        public void Construct(ISettingsProvider settingsProvider, 
            PlayerController playerController)
        {
            _botsSetting = settingsProvider.BotsSetting;
            _playerController = playerController;
        }

        public void Init()
        {
            EcsWorld = new EcsWorld();
            _updateSystems = new EcsSystems(EcsWorld);
            _fixedUpdateSystems = new EcsSystems(EcsWorld);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(EcsWorld);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
            _updateSystems
                .Add(new SpawnMonsterSystem()) 
                .Add(new PlayerNoiseSystem(_playerController))
                .Add(new BotVisionSystem())
                .Add(new BotHearingSystem())
                .Add(new BotBehaviorSystem())
                .Add(new BotAttackSystem())
                .Add(new HealthSystem())
                .OneFrame<MapLoadedEvent>()  
                .OneFrame<TakeDamageEvent>()
                .Inject(_botsSetting);
            
            _updateSystems.Init();
            _fixedUpdateSystems.Init();
        }

        public void DeInit()
        {
            EcsWorld?.Destroy();
            EcsWorld = null;
            _updateSystems?.Destroy();
            _updateSystems = null;
            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems = null;
        }

        private void Update()
        {
            _updateSystems?.Run();
        }
        
        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }
    }
}