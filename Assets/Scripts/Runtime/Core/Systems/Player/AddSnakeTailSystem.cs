using System;
using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Events;
using SA.Runtime.Core.Services.Factories;
using UnityEngine;

namespace SA.Runtime.Core.Systems
{
    public sealed class AddSnakeTailSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IUnitFactory _unitFactory;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PlayerViewComponent> _viewPool;
        private EcsPool<SnakeTailComponent> _tailPool;
        private EcsPool<IncreaseSnakeTailEvent> _eventPool;
        private EcsPool<TailPartComponent> _tailPartPool;
        private EcsPool<TailFollowComponent> _partFollowPool;

        public void Init(IEcsSystems systems)
        {
            _unitFactory = systems.GetShared<SharedData>().UnitFactory;
            
            _world = systems.GetWorld();

            _filter = _world
                .Filter<LocalPlayerTag>()
                .Inc<PlayerViewComponent>()
                .Inc<SnakeTailComponent>()
                .Inc<IncreaseSnakeTailEvent>()
                .End();

            _viewPool = _world.GetPool<PlayerViewComponent>();
            _tailPool = _world.GetPool<SnakeTailComponent>();
            _eventPool = _world.GetPool<IncreaseSnakeTailEvent>();
            _tailPartPool = _world.GetPool<TailPartComponent>();
            _partFollowPool = _world.GetPool<TailFollowComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {           
            foreach (var ent in _filter)
            {
                _eventPool.Del(ent);  

                ref var view = ref _viewPool.Get(ent);              
                ref var tail = ref _tailPool.Get(ent);  

                CreateTailPart(ref view, ref tail);
            }
        }

        private void CreateTailPart(ref PlayerViewComponent view, ref SnakeTailComponent tail)
        {
            var maxCount = view.ViewRef.Config.Tail.MaxVisibleCount;

            if (tail.PartEntities.Count < maxCount)
            {
                var sizeMultiplier = 1f - (tail.Count + 1) / (float)maxCount;            

                //part view
                var partView = _unitFactory.CreateTailPart();
                partView.Init(sizeMultiplier);
                var followTarget = GetPartFollowTarget(ref view, ref tail);
                partView.transform.position = followTarget.position + Vector3.up;

                //part entity
                var partEntity = _world.NewEntity();
                _tailPartPool.Add(partEntity).ViewRef = partView;
                _partFollowPool.Add(partEntity).Target = followTarget;

                //add to tail stack
                tail.PartEntities.Push(partEntity);
            }

            tail.Count++;
        }

        private Transform GetPartFollowTarget(ref PlayerViewComponent view, ref SnakeTailComponent tail)
        {
            if (tail.PartEntities.Count <= 0) 
            {
                return view.ViewRef.TailRoot;
            }

            var lastPartEntity = tail.PartEntities.Peek();            
            return _tailPartPool.Get(lastPartEntity).ViewRef.transform;
        }
    }
}