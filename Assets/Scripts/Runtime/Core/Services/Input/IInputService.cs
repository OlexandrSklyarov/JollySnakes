using UnityEngine;

namespace SA.Runtime.Core.Services.Input
{
    public interface IInputService
    {
        public Vector2 Movement {get;}
        public bool Attack {get;}
    }
}