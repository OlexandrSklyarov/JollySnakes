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
            view.RB = snakeView.RB;
            view.Tongue = snakeView.Tongue;
            view.Config = snakeView.Config;
            view.TailRoot = snakeView.TailRoot;
            view.BodyRenderer = snakeView.BodyRenderer;
            
            view.Tongue.BodyRenderer.SetPositions(new Vector3[2]);
            SetColorBody(ref view);

            //tongue
            ref var tongue = ref world.GetPool<TongueComponent>().Add(entity);
            tongue.AttackDistanceMultiplier = 1f; 

            //destroy monoBehaviour
            UnityEngine.Object.Destroy(snakeView);           
        }

        private void SetColorBody(ref PlayerViewComponent view)
        {
            var color = view.Config.View.ColorBodyGradient.Evaluate(UnityEngine.Random.value);
            view.BodyRenderer.materials[0].color = color;
            view.MyBodyColor = color;
        }
    }
}