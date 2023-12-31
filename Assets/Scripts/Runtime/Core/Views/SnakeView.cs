using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class SnakeView : MonoBehaviour, IPlayerView
    {
        public Rigidbody RB {get; private set;}
        [field: SerializeField] public SnakeConfig Config {get; private set;} 
        [field: SerializeField] public Transform ViewBody {get; private set;} 
        [field: SerializeField] public Transform OrientationLook {get; private set;} 
        
        private void Awake() 
        {
            RB = GetComponent<Rigidbody>();
        }
    }
}