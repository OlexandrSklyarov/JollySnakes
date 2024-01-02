using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerSpeedLimitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PlayerViewComponent> _viewPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<PlayerViewComponent>()
                .End();
            
            _viewPool = world.GetPool<PlayerViewComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var view = ref _viewPool.Get(ent);

                var curVelocity = view.ViewRef.RB.velocity;
                var horVelocity = new Vector3(curVelocity.x, 0f, curVelocity.z);
                var speed = view.ViewRef.Config.Movement.Speed;

                if (horVelocity.sqrMagnitude <= speed * speed) continue;

                var limitedVelocity = horVelocity.normalized * speed;
                view.ViewRef.RB.velocity = new Vector3(limitedVelocity.x, curVelocity.y, limitedVelocity.z);            
            }
        }
    }
}