using Leopotam.EcsLite;
using SA.Runtime.Core.Data.Constants;
using UnityEngine;
using SA.Runtime.Core.Data;
using VContainer;
using SA.Runtime.Core.Services.Time;
using SA.Runtime.Core.Services.Input;
using Cinemachine;
using SA.Runtime.Core.Systems;
using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Services.Factories;

namespace SA.Runtime.Core 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        private EcsWorld _world;        
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;
        private SharedData _sharedData;

        [Inject]
        private void Construct(
            GameConfig gameConfig,
            TimeService timeService,
            IInputService inputService,
            CinemachineFreeLook followCamera,
            IUnitFactory unitFactory
        )
        {
            _sharedData = new SharedData()
            {
                GameConfig = gameConfig,
                TimeService = timeService,
                InputService = inputService,
                FollowCamera = followCamera,
                UnitFactory = unitFactory
            };
        }

        private void Start() 
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world, _sharedData);
            _fixedUpdateSystems = new EcsSystems(_world, _sharedData);
            _lateUpdateSystems = new EcsSystems(_world, _sharedData);
            
            RegisterSystems();

            Debug.Log("Start game");
        }

        private void RegisterSystems()
        {
            var eventsWorld = new EcsWorld ();

            _updateSystems                
                .AddWorld (eventsWorld, GameConst.World.EVENTS)
            #if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem (GameConst.World.EVENTS))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
            #endif
                .Add(new CreatePlayerSystem())
                .Add(new PlayerInputSystem())         
                .Init();


            _fixedUpdateSystems                
                .AddWorld (eventsWorld, GameConst.World.EVENTS)
            #if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem (GameConst.World.EVENTS))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
            #endif
                .Add(new PlayerMovementSystem())
                .Init();


            _lateUpdateSystems                
                .AddWorld (eventsWorld, GameConst.World.EVENTS)
            #if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem (GameConst.World.EVENTS))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
            #endif
                .Init();
        }

        private void Update() 
        {
            _updateSystems?.Run ();
        }

        private void FixedUpdate() 
        {
            _fixedUpdateSystems?.Run ();
        }

        private void LateUpdate() 
        {
            _lateUpdateSystems?.Run ();
        }

        private void OnDestroy() 
        {            
            _updateSystems?.Destroy ();
            _updateSystems = null;  

            _fixedUpdateSystems?.Destroy ();
            _fixedUpdateSystems = null;  

            _lateUpdateSystems?.Destroy ();
            _lateUpdateSystems = null;        

            _world?.Destroy ();
            _world = null;            
        }
    }
}