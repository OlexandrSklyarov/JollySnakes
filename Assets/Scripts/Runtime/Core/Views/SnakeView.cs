using System.Diagnostics;
using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class SnakeView : MonoBehaviour, IPlayerView
    {
        public Rigidbody RB {get; private set;}
        [field: SerializeField] public SnakeConfig Config {get; private set;} 
        [field: SerializeField] public SnakeTongueView Tongue {get; private set;} 
        
        private void Awake() 
        {
            RB = GetComponent<Rigidbody>();
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.green;                
            Gizmos.DrawLine(Tongue.Origin.position, Tongue.Origin.position + Tongue.Origin.forward * Config.Tongue.BaseBoundSize.z);
        }
    }
}