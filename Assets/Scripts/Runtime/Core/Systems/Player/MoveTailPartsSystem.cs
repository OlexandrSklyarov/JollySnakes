using UnityEngine;
using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Time;

namespace SA.Runtime.Core.Systems
{
    public sealed class MoveTailPartsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private TimeService _time;
        private EcsFilter _filter;
        private EcsPool<TailPartComponent> _tailPartPool;
        private EcsPool<TailFollowComponent> _tailFollowPool;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService;
            
            var world = systems.GetWorld();

            _filter = world
                .Filter<TailPartComponent>()
                .Inc<TailFollowComponent>()
                .End();

            _tailPartPool = world.GetPool<TailPartComponent>();
            _tailFollowPool = world.GetPool<TailFollowComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var part = ref _tailPartPool.Get(ent);   
                ref var follow = ref _tailFollowPool.Get(ent);   
                
                var target = follow.Target;
                var myTR = part.ViewRef.transform;

                var newPos = target.position + (myTR.position - target.position).normalized * part.ViewRef.Radius; 
                myTR.position = Vector3.Lerp(myTR.position, newPos, part.ViewRef.MoveSpeed * _time.FixedDeltaTime);

                var dir = newPos - myTR.position;
                myTR.rotation = (dir != Vector3.zero) ?  Quaternion.LookRotation(dir) : myTR.rotation ;
            }
        }
    }
}