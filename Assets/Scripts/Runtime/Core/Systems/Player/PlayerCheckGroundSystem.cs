using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PlayerViewComponent> _viewPool;
        private EcsPool<MovementComponent> _movementPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<PlayerViewComponent>()
                .Inc<MovementComponent>()
                .End();

            _viewPool = world.GetPool<PlayerViewComponent>();
            _movementPool = world.GetPool<MovementComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {           
            foreach (var ent in _filter)
            {
                ref var view = ref _viewPool.Get(ent);
                ref var movement = ref _movementPool.Get(ent);
                
                movement.IsGrounded = IsGroundChecked(ref view);                
            }
        }        


        private bool IsGroundChecked(ref PlayerViewComponent view)
        {
            var origin = view.RB.transform.position + Vector3.up;
            var dist = 1f + view.Config.Movement.CheckGroundBounds.y;
            
            return Physics.BoxCast
            (
                origin, 
                view.Config.Movement.CheckGroundBounds, 
                Vector3.down, 
                Quaternion.LookRotation(Vector3.back), 
                dist, 
                view.Config.Movement.GroundLayerMask
            );
        }
    }
}