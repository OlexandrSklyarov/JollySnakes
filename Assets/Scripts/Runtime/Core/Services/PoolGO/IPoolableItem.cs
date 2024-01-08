
namespace SA.Runtime.Core.Services
{
    public interface IPoolableItem<T> where T : UnityEngine.MonoBehaviour
    {
        void SetPool(IPool<T> pool);
    }
}