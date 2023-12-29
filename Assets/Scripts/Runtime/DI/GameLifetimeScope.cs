using Cinemachine;
using SA.Runtime.Core;
using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Services.Factories;
using SA.Runtime.Core.Services.Input;
using SA.Runtime.Core.Services.Time;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SA.Runtime.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfig _gameConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameConfig);

            builder.RegisterEntryPoint<TimeService>().AsSelf();

            builder.Register<IUnitFactory, LocalUnitFactory>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<EcsStartup>();
            builder.RegisterComponentInHierarchy<CinemachineFreeLook>();

            RegisterInput(builder);
        }

        private void RegisterInput(IContainerBuilder builder)
        {
#if UNITY_ANDROID
            builder.Register<IInputService, KeyboardInput>(Lifetime.Singleton);
#elif UNITY_EDITOR
            builder.Register<IInputService, KeyboardInput>(Lifetime.Singleton);
#endif
        }
    }
}