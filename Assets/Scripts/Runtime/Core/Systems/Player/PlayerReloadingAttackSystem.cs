using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Time;

namespace SA.Runtime.Core.Systems
{
    public class PlayerReloadingAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private TimeService _time;
        private EcsFilter _filter;
        private EcsPool<TongueComponent> _tonguePool;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService;

            var world = systems.GetWorld();           

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<TongueComponent>()
                .End();

            _tonguePool = world.GetPool<TongueComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var tongue = ref _tonguePool.Get(ent);

                if (tongue.AttackReloadingTimer > 0f)
                {
                    tongue.AttackReloadingTimer -= _time.DeltaTime;
                }
            }
        }
    }
}