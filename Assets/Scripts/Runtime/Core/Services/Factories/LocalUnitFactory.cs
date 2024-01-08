using System;
using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Views;
using UnityEngine;

namespace SA.Runtime.Core.Services.Factories
{
    public sealed class LocalUnitFactory : IUnitFactory, IDisposable
    {
        private GameConfig _config;
        private Transform _container;
        private UniversalPoolGO<FoodView> _foodPool;

        public LocalUnitFactory(GameConfig config)
        {
            _config = config;
            _container = new GameObject("[Units_Container]").transform;
       
            _foodPool = new UniversalPoolGO<FoodView>(_config.Unit.FoodPrefab, "[FOOD-POOL]");
        }
        

        FoodView IUnitFactory.CreateFood(Vector3 position, Quaternion rotation)
        {
            var food = _foodPool.Get();
            food.transform.SetPositionAndRotation(position, rotation);

            return food;
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

        public void Dispose()
        {
            _foodPool.Clear();
        }
    }
}