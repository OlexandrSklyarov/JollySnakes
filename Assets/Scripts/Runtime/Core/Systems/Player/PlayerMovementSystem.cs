using System;
using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Time;
using Unity.VisualScripting;
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
                view.RB.transform.rotation,
                Quaternion.LookRotation(lookAt),
                view.Config.Movement.RotationSpeed * _time.FixedDeltaTime
            );

            view.RB.MoveRotation(newRot);
        }

        private void Movement(ref InputComponent input, ref PlayerViewComponent view, ref MovementComponent movement)
        {
            view.RB.drag = (movement.IsGrounded) ? 
                view.Config.Movement.GroundDrag : 
                view.Config.Movement.AirDrag;

            movement.CameraRelativeMovement = GetRelativeCameraDirection(ref input);
            
            view.RB.AddForce
            (
                view.Config.Movement.Acceleration * movement.CameraRelativeMovement, 
                ForceMode.Acceleration
            );

            //apply additional gravity
            view.RB.AddForce
            (
                new Vector3(0f, -view.Config.Movement.AdditionalGravity, 0f), 
                ForceMode.Acceleration
            );

            if (input.Movement == Vector2.zero)
            {
                DampingVelocity(ref view);
            }
        }

        private void DampingVelocity(ref PlayerViewComponent view)
        {
            var curVel = view.RB.velocity;
            var newVel = new Vector3(curVel.x, 0f, curVel.z);

            if (newVel.magnitude < view.Config.Movement.MinSpeed) return;

            newVel *= 0.95f;
            newVel.y = curVel.y;
            view.RB.velocity = newVel;
        }

        private Vector3 GetRelativeCameraDirection(ref InputComponent input)
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