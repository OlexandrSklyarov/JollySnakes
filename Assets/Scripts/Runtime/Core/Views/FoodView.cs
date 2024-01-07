using Leopotam.EcsLite;
using SA.Runtime.Core.Events;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class FoodView : MonoBehaviour
    {
        public Rigidbody RB {get; private set;}

        private Collider _collider;
        private MeshRenderer _renderer;
        private EcsWorld _world;
        private EcsPackedEntity _packedEntity;

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

        public void OnEat()
        {          
            Destroy(this.gameObject);
        }

        public void Take(Transform tip)
        {
            if (_packedEntity.Unpack(_world, out int entity))
            {
                _world.GetPool<EatFoodEvent>().Add(entity);
            }

            transform.SetParent(tip);
            _collider.enabled = false;
        }
    }
}
