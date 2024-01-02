using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Time;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<InputComponent> _inputPool;
        private EcsPool<PlayerViewComponent> _viewPool;
        private EcsPool<MovementComponent> _movementPool;
        private TimeService _time;
        private Camera _camMain;

        public void Init(IEcsSystems systems)
        {            
            _time = systems.GetShared<SharedData>().TimeService;
            _camMain = Camera.main;

            var world = systems.GetWorld();

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<InputComponent>()
                .Inc<PlayerViewComponent>()
                .Inc<MovementComponent>()
                .End();

            _inputPool = world.GetPool<InputComponent>();
            _viewPool = world.GetPool<PlayerViewComponent>();
            _movementPool = world.GetPool<MovementComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);
                ref var movement = ref _movementPool.Get(ent);

                Movement(ref input, ref view, ref movement);
                Rotation(ref input, ref view, ref movement);
            }
        }

        private void Rotation(ref InputComponent input, ref PlayerViewComponent view, ref MovementComponent movement)
        {
            if (input.Movement == Vector2.zero) return;

            var lookAt = new Vector3
            (
                movement.CameraRelativeMovement.x,
                0f,
                movement.CameraRelativeMovement.z
            );            

            var newRot = Quaternion.Slerp
            (
                view.ViewRef.RB.transform.rotation,
                Quaternion.LookRotation(lookAt),
                view.ViewRef.Config.Movement.RotationSpeed * _time.FixedDeltaTime
            );

            view.ViewRef.RB.MoveRotation(newRot);
        }

        private void Movement(ref InputComponent input, ref PlayerViewComponent view, ref MovementComponent movement)
        {
            movement.CameraRelativeMovement = GetRelativeCameraDirection(ref view, ref input);

            var newPos = view.ViewRef.RB.transform.position +
                movement.CameraRelativeMovement * (view.ViewRef.Config.Movement.Speed * _time.FixedDeltaTime);

            view.ViewRef.RB.MovePosition(newPos);
        }

        private Vector3 GetRelativeCameraDirection(ref PlayerViewComponent view, ref InputComponent input)
        {         
            var camForward = _camMain.transform.forward;           
            var camRight = _camMain.transform.right;           
            camForward.y = camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            var vectorRotateToCameraSpace = input.Movement.y * camForward + input.Movement.x * camRight;       

            return vectorRotateToCameraSpace;
        }
    }
}