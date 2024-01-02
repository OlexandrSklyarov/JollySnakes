using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Input;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IInputService _inputService;
        private EcsWorld _world;
        private EcsFilter _attackStateFilter;
        private EcsFilter _filter;
        private EcsPool<InputComponent> _inputPool;
        private EcsPool<AttackStateComponent> _attackStatePool;

        public void Init(IEcsSystems systems)
        {
            _inputService = systems.GetShared<SharedData>().InputService;
            
            _world = systems.GetWorld();

            _attackStateFilter = _world.Filter<AttackStateComponent>().End();

            _filter = _world
                .Filter<LocalPlayerTag>()
                .Inc<InputComponent>()
                .End();

            _inputPool = _world.GetPool<InputComponent>();
            _attackStatePool = _world.GetPool<AttackStateComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _attackStateFilter)
            {
                _attackStatePool.Del(ent);
            }

            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);

                input.Movement = _inputService.Movement;
                input.LookDirection = _inputService.Look;
                
                //add attack state
                if(_inputService.Attack)
                {
                    _attackStatePool.Add(ent);
                }
            }
        }
    }
}