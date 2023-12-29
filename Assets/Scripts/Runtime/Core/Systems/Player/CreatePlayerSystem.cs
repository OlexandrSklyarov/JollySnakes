using Leopotam.EcsLite;
using SA.Runtime.Core.Components;
using SA.Runtime.Core.Components.Player;
using SA.Runtime.Core.Data;
using SA.Runtime.Core.Services.Input;

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
            
            ref var view = ref world.GetPool<SnakeViewComponent>().Add(entity);
            view.ViewRef = snakeView;
        }       
    }
}