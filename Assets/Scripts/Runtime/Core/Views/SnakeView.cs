using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class SnakeView : MonoBehaviour
    {
        public Rigidbody RB {get; private set;}
        [field: SerializeField] public Transform TailRoot {get; private set;} 
        [field: SerializeField] public SnakeConfig Config {get; private set;} 
        [field: SerializeField] public SnakeTongueView Tongue {get; private set;}
        [field: SerializeField] public MeshRenderer BodyRenderer {get; private set;}

        private void Awake() 
        {
            RB = GetComponent<Rigidbody>();
        }        
    }
}