using UnityEngine;

namespace SA.Runtime.Core.Services
{
    public interface IPool<T>  where T : MonoBehaviour
    {
        void Reclaim(IPoolableItem<T> item);
    }
}