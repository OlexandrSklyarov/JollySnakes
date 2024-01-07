using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Components
{
    public struct FoodEmitterComponent
    {
        public FoodEmitterConfig Config;
        public Transform[] SpawnPoints;
        public float NextSpawnTime;
    }
}