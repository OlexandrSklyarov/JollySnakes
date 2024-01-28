using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class CreatePlayerSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var factory = systems.GetShared<SharedData>().UnitFactory;
            var camera = systems.GetShared<SharedData>().FollowCamera;
            
            var snakeView = factory.CreateSnake();
            
            camera.Follow = snakeView.transform;
            camera.LookAt = snakeView.transform;

            var world = systems.GetWorld();
            var entity = world.NewEntity();

            world.GetPool<LocalPlayerTag>().Add(entity);
            world.GetPool<InputComponent>().Add(entity);
            world.GetPool<MovementComponent>().Add(entity);
            world.GetPool<JumpComponent>().Add(entity);

            //tail
            ref var tail = ref world.GetPool<SnakeTailComponent>().Add(entity);
            tail.PartEntities = new Stack<int>();
            
            //view
            ref var view = ref world.GetPool<PlayerViewComponent>().Add(entity);
            view.ViewRef = snakeView;
            view.ViewRef.Tongue.BodyRenderer.SetPositions(new Vector3[2]);
            SetColorBody(ref view);

            //tongue
            ref var tongue = ref world.GetPool<TongueComponent>().Add(entity);
            tongue.AttackDistanceMultiplier = 1f;            
        }

        private void SetColorBody(ref PlayerViewComponent view)
        {
            var color = view.ViewRef.Config.View.ColorBodyGradient.Evaluate(UnityEngine.Random.value);
            view.ViewRef.BodyRenderer.materials[0].color = color;
            view.MyBodyColor = color;
        }
    }
}