using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Components.Player;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Time;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<InputComponent> _inputPool;
        private EcsPool<SnakeViewComponent> _viewPool;
        private TimeService _time;

        public void Init(IEcsSystems systems)
        {            
            _time = systems.GetShared<SharedData>().TimeService;

            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<InputComponent>()
                .Inc<SnakeViewComponent>()
                .End();

            _inputPool = world.GetPool<InputComponent>();
            _viewPool = world.GetPool<SnakeViewComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);
               
                var tr = view.ViewRef.transform;
                var newPos = tr.position + tr.forward * 
                    (input.Vert * view.ViewRef.Config.Speed * _time.FixedDeltaTime);

                view.ViewRef.RB.MovePosition(newPos);
            }
        }
    }
}