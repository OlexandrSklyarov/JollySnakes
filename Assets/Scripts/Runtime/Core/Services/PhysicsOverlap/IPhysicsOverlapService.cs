using UnityEngine;

namespace SA.Runtime.Core.Services
{
    public interface IPhysicsOverlapService
    {
        public bool TryGetBoxOverlapTarget<T>(Vector3 origin, Vector3 halfExtend, 
            Quaternion orientation, LayerMask mask, out T target) where T : MonoBehaviour;        
    }
}