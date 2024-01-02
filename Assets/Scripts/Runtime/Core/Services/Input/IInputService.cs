using UnityEngine;

namespace SA.Runtime.Core.Services.Input
{
    public interface IInputService
    {
        public Vector2 Movement {get;}
        public Vector2 Look {get;}
        public bool IsAttack {get;}
        bool IsJumpPressed { get; }
    }
}