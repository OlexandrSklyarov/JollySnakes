using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Input;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IInputService _inputService;
        private EcsFilter _filter;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _inputService = systems.GetShared<SharedData>().InputService;
            
            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<InputComponent>()
                .End();

            _inputPool = world.GetPool<InputComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);

                input.Movement = _inputService.Movement;
                input.LookDirection = _inputService.Look;
                input.IsAttack = _inputService.Attack;
            }
        }
    }
}