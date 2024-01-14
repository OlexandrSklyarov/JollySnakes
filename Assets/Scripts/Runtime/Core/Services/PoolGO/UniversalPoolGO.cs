using UnityEngine;
using UnityEngine.Pool;

namespace SA.Runtime.Core.Services
{
    public class UniversalPoolGO<T> : IPool<T> where T : MonoBehaviour, IPoolableItem<T>
    {
        private readonly T _prefab;
        private readonly Transform _container;
        private readonly IObjectPool<T>_innerPool;

        public UniversalPoolGO(T prefab, string poolName, int startCount = 10, int maxCount = 10000)
        {            
            _prefab = prefab;
            _container = new GameObject($"[{poolName}]").transform;
            
            _innerPool = new ObjectPool<T>
            (
                OnCreateItem, OnTakeItem, OnReturnItem, OnDestroyItem, true, startCount, maxCount
            );            
        }

        private void OnDestroyItem(T item)
        {
            if (item == null) return;
            UnityEngine.Object.Destroy(item.gameObject);
        }

        private void OnReturnItem(T item)
        {
            item.gameObject.SetActive(false);
        }

        private void OnTakeItem(T item)
        {
            item.gameObject.SetActive(true);
        }

        private T OnCreateItem()
        {
            var item = UnityEngine.Object.Instantiate(_prefab, _container);
            item.SetPool(this);
            return item;            
        }

        public T Get()
        {
            return _innerPool.Get();
        }

        public void Clear()
        {
            _innerPool?.Clear();
        }

        void IPool<T>.Reclaim(IPoolableItem<T> item) => _innerPool.Release(item as T);
    }
}