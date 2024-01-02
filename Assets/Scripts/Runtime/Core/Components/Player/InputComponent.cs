using UnityEngine;

namespace SA.Runtime.Core.Components
{
    public struct InputComponent
    {
        public Vector2 Movement;
        public Vector2 LookDirection;
        public bool IsAttack;
        public bool IsJumpPressed;
    }
}