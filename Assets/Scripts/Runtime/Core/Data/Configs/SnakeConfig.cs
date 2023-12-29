using UnityEngine;

namespace SA.Runtime.Core.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Snake/SnakeConfig", fileName = "SnakeConfig")]
    public sealed class SnakeConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float Speed {get; private set;} = 5f;
        
    }
}