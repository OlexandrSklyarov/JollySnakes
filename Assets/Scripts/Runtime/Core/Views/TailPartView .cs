using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Services;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    public class TailPartView : MonoBehaviour, IPoolableItem<TailPartView>
    {        
        public float Radius => _config.Radius;
        public float MoveSpeed => _config.MoveSpeed;

        [SerializeField] private TailPartConfig _config;
        [SerializeField] private MeshRenderer _bodyRenderer;

        private IPool<TailPartView> _pool;

        public void Restore()
        {
            _pool.Reclaim(this);
        }

        public void Init(float sizeMultiplier, Color newColor)
        {
            transform.localScale = Vector3.one * Mathf.Clamp(sizeMultiplier, _config.MinScale, 1f);

            _bodyRenderer.materials[0].color = newColor;
        }

        void IPoolableItem<TailPartView>.SetPool(IPool<TailPartView> pool)
        {
            _pool = pool;
        }
    }
}