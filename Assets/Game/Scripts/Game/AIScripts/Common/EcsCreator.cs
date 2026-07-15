using System;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Game.AIScripts.Behaviour;
using Game.Scripts.Settings;
using Game.Scripts.Game.AIScripts.Spawn;
using UnityEngine;
using Leopotam.Ecs;
using VContainer;

namespace Game.Scripts.Game.AIScripts.Common
{
    public class EcsCreator : MonoBehaviour
    {
        private BotsSetting _botsSetting;
        
        public EcsWorld EcsWorld {get; private set;}
        
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        [Inject]
        public void Construct(ISettingsProvider settingsProvider)
        {
            _botsSetting = settingsProvider.BotsSetting;
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
                .Add(new BotVisionSystem())
                .Add(new BotBehaviorSystem())
                .OneFrame<MapLoadedEvent>()    
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