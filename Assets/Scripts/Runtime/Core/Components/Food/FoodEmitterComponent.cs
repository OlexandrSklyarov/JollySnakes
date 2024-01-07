using UnityEngine;

namespace SA.Runtime.Core.Components
{
    public struct FoodEmitterComponent
    {
        public float NextSpawnTime;
        public Transform[] SpawnPoints;
    }
}