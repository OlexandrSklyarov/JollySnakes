using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerSpeedLimitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PlayerViewComponent> _viewPool;
        private EcsPool<AttackStateComponent> _attackStatePool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<PlayerViewComponent>()
                .End();
            
            _viewPool = world.GetPool<PlayerViewComponent>();
            _attackStatePool = world.GetPool<AttackStateComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var view = ref _viewPool.Get(ent);

                var curVelocity = view.RB.linearVelocity;
                var horVelocity = new Vector3(curVelocity.x, 0f, curVelocity.z);
                var speed = view.Config.Movement.MaxSpeed;

                if (horVelocity.sqrMagnitude <= speed * speed) continue;

                var limitedVelocity = horVelocity.normalized * speed;

                if (_attackStatePool.Has(ent))
                {
                    limitedVelocity = horVelocity.normalized * view.Config.Movement.MinSpeed;
                }    
                
                view.RB.linearVelocity = new Vector3(limitedVelocity.x, curVelocity.y, limitedVelocity.z);       
            }
        }
    }
}