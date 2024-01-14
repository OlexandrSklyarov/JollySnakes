using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Events;

namespace SA.Runtime.Core.Systems
{
    public sealed class KillFoodSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {            
            _world = systems.GetWorld();

            _filter = _world.Filter<FoodComponent>()
                .Inc<FoodKillEvent>()
                .End();                
        }

       
        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                _world.DelEntity(ent);           
            }
        }
    }
}