using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Time;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private TimeService _time;
        private EcsFilter _filter;
        private EcsPool<JumpComponent> _jumpPool;
        private EcsPool<PlayerViewComponent> _viewPool;
        private EcsPool<MovementComponent> _movementPool;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService;
            
            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<JumpComponent>()
                .Inc<PlayerViewComponent>()
                .Inc<MovementComponent>()
                .Inc<InputComponent>()
                .End();

            _jumpPool = world.GetPool<JumpComponent>();
            _viewPool = world.GetPool<PlayerViewComponent>();
            _movementPool = world.GetPool<MovementComponent>();
            _inputPool = world.GetPool<InputComponent>();
        }

        public void Run(IEcsSystems systems)
        {            
            foreach (var ent   in _filter)
            {
                ref var jump = ref _jumpPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);
                ref var movement = ref _movementPool.Get(ent);

                if (jump.JumpUnlockTimer > 0f)
                {
                    jump.JumpUnlockTimer -= Time.deltaTime;
                    continue;
                }
                
                if (!input.IsJumpPressed) continue;
                if (!movement.IsGrounded) continue;

                Jump(ref view, ref jump);
            }
        }        

        private void Jump(ref PlayerViewComponent view, ref JumpComponent jump)
        {
            var rb = view.RB;
            var vel = view.RB.velocity;
            var config = view.Config;

            rb.velocity = new Vector3(vel.x, 0f, vel.z);
            
            rb.AddForce
            (
                rb.transform.up * config.Jump.Force, 
                ForceMode.Acceleration
            );

            jump.JumpUnlockTimer = config.Jump.Cooldown;
        }
    }
}