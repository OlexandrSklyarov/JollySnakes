using UnityEngine;
using SA.Runtime.Core.Services;

namespace SA.Runtime.Core.Views
{
    public class PoolMonoBehaviour : MonoBehaviour, IPoolableItem<PoolMonoBehaviour>
    {
        private IPool<PoolMonoBehaviour> _myPool;

        void IPoolableItem<PoolMonoBehaviour>.SetPool(IPool<PoolMonoBehaviour> pool) => _myPool = pool;

        protected void Reclaim() => _myPool.Reclaim(this);
    }   
}