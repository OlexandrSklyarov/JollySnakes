using System;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    public class FoodView : MonoBehaviour
    {
        private Collider _collider;

        private void Awake() 
        {
            _collider = GetComponent<Collider>();
            Init();
        }

        public void Init()
        {     
            var gradient = new Gradient();

            gradient.SetKeys
            (
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.yellow, 0f),
                    new GradientColorKey(Color.green, 0.2f),
                    new GradientColorKey(Color.blue, 0.4f),
                    new GradientColorKey(Color.magenta, 0.6f),
                    new GradientColorKey(Color.cyan, 0.8f),
                    new GradientColorKey(Color.red, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 0.2f),
                    new GradientAlphaKey(1f, 0.4f),
                    new GradientAlphaKey(1f, 0.6f),
                    new GradientAlphaKey(1f, 0.8f),
                    new GradientAlphaKey(1f, 1f),
                }
            );

            var value = UnityEngine.Random.Range(0, 1f);
            var rndColor = gradient.Evaluate(value);

            GetComponentInChildren<MeshRenderer>().materials[0].SetColor("_BaseColor", rndColor);
        }

        public void OnEat()
        {
            Destroy(this.gameObject);
        }

        public void Take(Transform tip)
        {
            transform.SetParent(tip);
            _collider.enabled = false;
        }
    }
}
