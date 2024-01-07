using SA.Runtime.Core.Views;
using UnityEngine;

namespace SA.Runtime.Core.Services.Factories
{
    public interface IUnitFactory
    {
        public FoodView CreateFood(Vector3 position, Quaternion rotation);
        public SnakeView CreateSnake();
        public TailPartView CreateTailPart();
    }
}