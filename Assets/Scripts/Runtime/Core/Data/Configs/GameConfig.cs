using System;
using SA.Runtime.Core.Views;
using UnityEngine;

namespace SA.Runtime.Core.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/GameConfig", fileName = "GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkConfig Network {get; private set;}
        [field: Space, SerializeField] public UnitConfig Unit {get; private set;}
    }

    [Serializable]
    public sealed class NetworkConfig
    {
        [field: SerializeField, Min(2)] public int MaxPlayers {get; private set;} = 4;
    }

    [Serializable]
    public sealed class UnitConfig
    {
        [field: SerializeField] public SnakeView SnakePrefab {get; private set;}
        [field: SerializeField] public TailPartView TailPartPrefab {get; private set;}
        [field: SerializeField] public FoodView FoodPrefab {get; private set;}
    }
}