using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    public class FoodEmitterView : MonoBehaviour
    {
        [field: SerializeField] public Transform[] SpawnPoints {get; private set;}
        [field: SerializeField] public FoodEmitterConfig Config {get; private set;}
    }
}
