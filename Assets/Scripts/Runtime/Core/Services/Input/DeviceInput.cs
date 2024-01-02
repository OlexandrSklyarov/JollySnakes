using UnityEngine;

namespace SA.Runtime.Core.Services.Input
{
    public sealed class DeviceInput : IInputService
    {
        private readonly PlayerInputActions _inputAction;

        Vector2 IInputService.Movement => _inputAction.Player.Move.ReadValue<Vector2>();
        Vector2 IInputService.Look => _inputAction.Player.Look.ReadValue<Vector2>();
        bool IInputService.IsAttack => _inputAction.Player.Fire.IsInProgress();
        bool IInputService.IsJumpPressed => _inputAction.Player.Jump.IsInProgress();

        public DeviceInput()
        {
            _inputAction = new PlayerInputActions();
            _inputAction.Enable();
        }
    }
}