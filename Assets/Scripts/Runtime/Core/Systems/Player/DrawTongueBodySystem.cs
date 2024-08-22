using Leopotam.EcsLite;
using SA.Runtime.Core.Components;

namespace SA.Runtime.Core.Systems
{
    public class DrawTongueBodySystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PlayerViewComponent> viewPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();           

            _filter = world
                .Filter<LocalPlayerTag>()
                .Inc<PlayerViewComponent>()
                .End();

            viewPool = world.GetPool<PlayerViewComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var view = ref viewPool.Get(ent);   

                var pointA = view.Tongue.Origin;   
                var pointB = view.Tongue.Tip;            

                view.Tongue.BodyRenderer.SetPosition(0, pointA.position);
                view.Tongue.BodyRenderer.SetPosition(1, pointB.position);
            }
        }
    }
}