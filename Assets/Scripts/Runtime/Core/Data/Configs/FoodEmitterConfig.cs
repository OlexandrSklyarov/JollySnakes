using UnityEngine;

namespace SA.Runtime.Core.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Env/FoodEmitterConfig", fileName = "FoodEmitterConfig")]
    public sealed class FoodEmitterConfig : ScriptableObject
    {
        [field: SerializeField, Min(0.01f)] public float SpawnSpread {get; private set;} = 0.5f;
        [field: SerializeField, Min(0.01f)] public float MinPushItemForce {get; private set;} = 20f;
        [field: SerializeField, Min(0.01f)] public float MaxPushItemForce {get; private set;} = 40f;
    }
}