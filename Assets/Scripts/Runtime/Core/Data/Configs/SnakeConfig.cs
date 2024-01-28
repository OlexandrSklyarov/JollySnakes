using System;
using UnityEngine;

namespace SA.Runtime.Core.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Snake/SnakeConfig", fileName = "SnakeConfig")]
    public sealed class SnakeConfig : ScriptableObject
    {
        [field: SerializeField] public Movement Movement {get; private set;}
        [field: Space, SerializeField] public Tongue Tongue {get; private set;}
        [field: Space, SerializeField] public Jump Jump {get; private set;}
        [field: Space, SerializeField] public Tail Tail {get; private set;}
        [field: Space, SerializeField] public View View {get; private set;}
    }

    [Serializable]
    public class Movement
    {
        [field: SerializeField, Min(1f)] public float MaxSpeed {get; private set;} = 12f;
        [field: SerializeField, Min(0.01f)] public float MinSpeed {get; private set;} = 0.5f;
        [field: SerializeField, Min(1f)] public float Acceleration {get; private set;} = 20f;
        [field: SerializeField, Min(1f)] public float RotationSpeed {get; private set;} = 6f;
        [field: SerializeField, Min(1f)] public float AdditionalGravity {get; private set;} = 6f;
        [field: SerializeField, Min(0f)] public float GroundDrag {get; private set;} = 2f;
        [field: SerializeField, Min(0f)] public float AirDrag {get; private set;} = 0f;
        [field: SerializeField] public LayerMask GroundLayerMask {get; private set;}
        [field: SerializeField] public Vector3 CheckGroundBounds {get; private set;} = new Vector3(1f, 0.1f, 1f);       
    }

    [Serializable]
    public class Tongue
    {
        [field: SerializeField] public Vector3 BaseBoundSize {get; private set;} = new Vector3(1f, 1f, 4f);
        [field: SerializeField, Min(0.1f)] public float AttackReloadingTime {get; private set;} = 0.5f;  
        [field: SerializeField] public LayerMask FoodLayerMask {get; private set;}
        [field: SerializeField] public AnimationCurve EatCurve {get; private set;}
    }

    [Serializable]
    public class Jump
    {
        [field: SerializeField, Min(1f)] public float Force {get; private set;} = 500f;
        [field: SerializeField, Min(0.1f)] public float Cooldown {get; private set;} = 0.25f;  
    }

    [Serializable]
    public class Tail
    {
        [field: SerializeField, Min(1)] public int MaxVisibleCount {get; private set;} = 8;
    }

    [Serializable]
    public class View
    {
        [field: SerializeField] public Gradient ColorBodyGradient {get; private set;} = new Gradient();
    }
}