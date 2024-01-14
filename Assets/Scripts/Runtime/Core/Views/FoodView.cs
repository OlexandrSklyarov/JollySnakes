using Leopotam.EcsLite;
using SA.Runtime.Core.Events;
using SA.Runtime.Core.Services;
using UnityEngine;
using Util;

namespace SA.Runtime.Core.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class FoodView : MonoBehaviour, IPoolableItem<FoodView>
    {
        public Rigidbody RB {get; private set;}

        [SerializeField] private LayerMask _autoDestroyLayerMask;

        private Collider _collider;
        private MeshRenderer _renderer;
        private EcsWorld _world;
        private EcsPackedEntity _packedEntity;
        private IPool<FoodView> _myPool;    

        private void Awake() 
        {
            _collider = GetComponent<Collider>();
            _renderer = GetComponentInChildren<MeshRenderer>();
            RB = GetComponent<Rigidbody>();
        }

        public void Init(Color color, EcsWorld world, EcsPackedEntity ecsPackedEntity)
        {   
            _world = world;
            _packedEntity = ecsPackedEntity;
            
            _renderer.materials[0].SetColor("_BaseColor", color);
        }

        public void Take(Transform tip)
        {
            AddKillEntityEvent();
            transform.SetParent(tip);
            _collider.enabled = false;
        }

        public void OnEat() => _myPool.Reclaim(this);

        private void OnAutoDestroy()
        {
            AddKillEntityEvent();
            Reclaim();
        }

        private void AddKillEntityEvent()
        {
            if (_packedEntity.Unpack(_world, out int entity))
            {
                _world.GetPool<FoodKillEvent>().Add(entity);
            }
        }

        private void Reclaim() => _myPool.Reclaim(this);

        void IPoolableItem<FoodView>.SetPool(IPool<FoodView> pool) => _myPool = pool;

        private void OnCollisionEnter(Collision other) 
        {
            if (other.gameObject.layer.LayerContains(_autoDestroyLayerMask))
            {
                OnAutoDestroy(); 
            }
        }
    }
}
