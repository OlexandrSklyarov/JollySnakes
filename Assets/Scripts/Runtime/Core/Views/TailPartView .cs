using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    public class TailPartView : MonoBehaviour
    {        
        public float Radius => _config.Radius;
        public float MoveSpeed => _config.MoveSpeed;

        [SerializeField] private TailPartConfig _config;

        public void Restore()
        {
            Destroy(this.gameObject);
        }

        public void Init(float sizeMultiplier)
        {
            transform.localScale = Vector3.one * Mathf.Clamp(sizeMultiplier, _config.MinScale, 1f);
        }
    }
}