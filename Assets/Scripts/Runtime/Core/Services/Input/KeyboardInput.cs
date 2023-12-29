using UnityEngine;

namespace SA.Runtime.Core.Services.Input
{
    public sealed class KeyboardInput : IInputService
    {
        private readonly PlayerInputAction _inputAction;

        Vector2 IInputService.Movement => _inputAction.Player.Movement.ReadValue<Vector2>();
        bool IInputService.Attack => _inputAction.Player.Attack.IsInProgress();

        public KeyboardInput()
        {
            _inputAction = new PlayerInputAction();
            _inputAction.Enable();
        }
    }
}