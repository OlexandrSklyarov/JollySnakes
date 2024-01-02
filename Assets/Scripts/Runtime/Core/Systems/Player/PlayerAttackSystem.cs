using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services;
using SA.Runtime.Core.Services.Time;
using SA.Runtime.Core.Views;
using UnityEngine;
using DG.Tweening;
using System;

namespace SA.Runtime.Core.Systems
{
    public sealed class PlayerAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PlayerViewComponent> _viewPool;
        private EcsPool<TongueComponent> _tonguePool;
        private TimeService _time;
        private IPhysicsOverlapService _overlapService;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService;
            _overlapService = systems.GetShared<SharedData>().OverlapService;

            var world = systems.GetWorld();           

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<AttackStateComponent>()
                .Inc<PlayerViewComponent>()
                .Inc<TongueComponent>()
                .End();

            _viewPool = world.GetPool<PlayerViewComponent>();
            _tonguePool = world.GetPool<TongueComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var view = ref _viewPool.Get(ent);
                ref var tongue = ref _tonguePool.Get(ent);

                if (tongue.NextAttackTime > _time.Time)
                    continue;

                SetAttackCooldown(ref view, ref tongue);

                TryAttack(ref view, ref tongue);

                RestoreAttackDistanceMultiplier(ref tongue);
            }
        }

        private void SetAttackCooldown(ref PlayerViewComponent view, ref TongueComponent tongue)
        {
            tongue.NextAttackTime = _time.Time + view.ViewRef.Config.Tongue.AttackTime;
        }
      

        private void RestoreAttackDistanceMultiplier(ref TongueComponent tongue)
        {
            tongue.AttackDistanceMultiplier = 1f;  
        }

        private void TryAttack(ref PlayerViewComponent view, ref TongueComponent tongue)
        {
            var config = view.ViewRef.Config; 
            var duration = (tongue.NextAttackTime - _time.Time) * 0.5f;

            var halfExtend = (config.Tongue.BaseBoundSize + new Vector3(0f, 0f, tongue.AttackDistanceMultiplier)) * 0.5f;

            if (_overlapService.TryGetBoxOverlapTarget
            (
                view.ViewRef.Tongue.Origin.position + view.ViewRef.Tongue.Origin.forward * halfExtend.z,
                halfExtend,
                view.ViewRef.Tongue.Origin.rotation,
                config.Tongue.FoodLayerMask,
                out FoodView target
            ))
            {
                TakeFood(ref view, target, duration);
                return;
            }

            SimpleAttack(ref view, duration);
        }

        private void SimpleAttack(ref PlayerViewComponent view, float duration)
        {
            var config = view.ViewRef.Config;
            var value = config.Tongue.BaseBoundSize.z;

            var tip = view.ViewRef.Tongue.Tip;

            tip.DOLocalMoveZ(value, duration)
                .OnComplete(() =>
                {
                    tip.DOLocalMoveZ(0f, duration);
                });
        }

        private void TakeFood(ref PlayerViewComponent view, FoodView target, float duration)
        {           
            var tip = view.ViewRef.Tongue.Tip;
            var targetDir = target.transform.position - tip.position;
            tip.rotation = Quaternion.LookRotation(targetDir);

            if (targetDir.magnitude < 0.5f) duration *= 0.5f;                
            
            var endPos = view.ViewRef.Tongue.Origin.InverseTransformPoint(target.transform.position);

            //move to target
            tip.DOLocalMove(endPos, duration)
                .OnComplete(() =>
                {
                    //take
                    target.transform.SetParent(tip);

                    //return with food
                    tip.DOLocalMove(Vector3.zero, duration)
                        .OnComplete(() =>
                        {
                            //eat
                            target.OnEat();
                            tip.localRotation = Quaternion.identity;
                        });                    
                });
        }
    }
}