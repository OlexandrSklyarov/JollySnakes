using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Views;
using UnityEngine;

namespace SA.Runtime.Core.Services.Factories
{
    public sealed class LocalUnitFactory : IUnitFactory
    {
        private GameConfig _config;
        private Transform _container;

        public LocalUnitFactory(GameConfig config)
        {
            _config = config;
            _container = new GameObject("[Units_Container]").transform;
        }

        SnakeView IUnitFactory.CreateSnake()
        {
            var snake = UnityEngine.Object.Instantiate
            (
                _config.Unit.SnakePrefab,
                Vector3.zero + Vector3.up * 30f,
                Quaternion.identity,
                _container
            );

            return snake;
        }

        TailPartView IUnitFactory.CreateTailPart()
        {
            var snake = UnityEngine.Object.Instantiate
            (
                _config.Unit.TailPartPrefab,
                _container
            );

            return snake;
        }
    }
}