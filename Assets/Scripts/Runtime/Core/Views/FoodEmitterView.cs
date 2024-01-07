using UnityEngine;

namespace SA.Runtime.Core.Views
{
    public class FoodEmitterView : MonoBehaviour
    {
        [field: SerializeField] public Transform[] SpawnPoints {get; private set;}
    }
}
