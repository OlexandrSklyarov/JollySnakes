using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Factories;
using SA.Runtime.Core.Services.Time;
using SA.Runtime.Core.Views;
using UnityEngine;
using Util;

namespace SA.Runtime.Core.Systems
{
    public sealed class SpawnFoodSystem : IEcsInitSystem, IEcsRunSystem
    {
        private TimeService _time;
        private IUnitFactory _unitFactory;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<FoodEmitterComponent> _emitterPool;
        private EcsPool<FoodComponent> _foodPool;
        private EcsPool<PhysicBodyComponent> _bodyPool;
        private EcsPool<JumpFoodComponent> _jumpPool;
        private Gradient _gradient;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();

            _time = data.TimeService;
            _unitFactory = data.UnitFactory;

            _world = systems.GetWorld();

            _filter = _world.Filter<FoodEmitterComponent>().End();

            _emitterPool = _world.GetPool<FoodEmitterComponent>();
            _foodPool = _world.GetPool<FoodComponent>();
            _bodyPool = _world.GetPool<PhysicBodyComponent>();
            _jumpPool = _world.GetPool<JumpFoodComponent>();

            SetupColorGradient();

            //init emitters
            foreach(var view in data.EmittersRoot.GetComponentsInChildren<FoodEmitterView>())
            {
                var entity = _world.NewEntity();
                ref var emitter = ref _emitterPool.Add(entity);
                emitter.SpawnPoints = view.SpawnPoints;
            }    
        }

        private void SetupColorGradient()
        {
            _gradient = new Gradient();

            _gradient.SetKeys
            (
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.yellow, 0f),
                    new GradientColorKey(Color.green, 0.2f),
                    new GradientColorKey(Color.blue, 0.4f),
                    new GradientColorKey(Color.magenta, 0.6f),
                    new GradientColorKey(Color.cyan, 0.8f),
                    new GradientColorKey(Color.red, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 0.2f),
                    new GradientAlphaKey(1f, 0.4f),
                    new GradientAlphaKey(1f, 0.6f),
                    new GradientAlphaKey(1f, 0.8f),
                    new GradientAlphaKey(1f, 1f),
                }
            );            
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var emitter = ref _emitterPool.Get(ent);  

                if (emitter.NextSpawnTime > _time.Time) continue;

                emitter.NextSpawnTime = _time.Time + 3f;

                SpawnFood(ref emitter);
            }
        }

        private void SpawnFood(ref FoodEmitterComponent emitter)
        {
            //create
            var entity = _world.NewEntity();

            var point = emitter.SpawnPoints.RandomElement();
            var randomRotVector = point.transform.forward.AddDirectionSpread(0.5f);
            var rotation = Quaternion.LookRotation(randomRotVector);

            var view = _unitFactory.CreateFood(point.position, rotation);            
            var color = _gradient.Evaluate(UnityEngine.Random.Range(0, 1f));
            view.Init(color, _world, _world.PackEntity(entity));

            //add components
            _foodPool.Add(entity);

            ref var jump = ref _jumpPool.Add(entity);
            jump.Force = UnityEngine.Random.Range(800f, 1000f);

            ref var body = ref _bodyPool.Add(entity);
            body.RbRef = view.RB;

            //push
            body.RbRef.AddForce
            (
                body.RbRef.transform.forward * UnityEngine.Random.Range(10f, 15f),
                ForceMode.Impulse
            );
        }
    }
}