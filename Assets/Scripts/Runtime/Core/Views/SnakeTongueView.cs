using System;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    [Serializable]
    public class SnakeTongueView
    {
        [field: SerializeField] public Transform Tip {get; private set;} 
        [field: SerializeField] public Transform Origin {get; private set;} 
    }
}