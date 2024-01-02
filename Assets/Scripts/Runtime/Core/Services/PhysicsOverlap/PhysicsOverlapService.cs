
using UnityEngine;

namespace SA.Runtime.Core.Services
{
    public class PhysicsOverlapService : IPhysicsOverlapService
    {
        private readonly Collider[] _boxResults = new Collider[32];

        public bool TryGetBoxOverlapTarget<T>(Vector3 origin, Vector3 halfExtend, 
            Quaternion orientation, LayerMask mask, out T target) where T : MonoBehaviour
        {     
            target = default;

            var count = Physics.OverlapBoxNonAlloc
            (
                origin, halfExtend, _boxResults, orientation, mask
            );

            if (count > 0)
            {
                return _boxResults[0].TryGetComponent(out target);
            }

            return false;
        }
    }
}