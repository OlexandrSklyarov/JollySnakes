using System;
using UnityEngine;

namespace SA.Runtime.Core.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Snake/TailPartConfig", fileName = "TailPartConfig")]
    public sealed class TailPartConfig : ScriptableObject
    {
        [field: SerializeField] public float Radius {get; private set;} = 0.3f;
        [field: SerializeField] public float MoveSpeed {get; private set;} = 10f;
        [field: SerializeField] public float MinScale {get; private set;} = 0.1f;
    }
}