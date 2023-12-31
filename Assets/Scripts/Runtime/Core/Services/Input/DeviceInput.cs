using UnityEngine;
using UnityEngine.InputSystem;

namespace SA.Runtime.Core.Services.Input
{
    public sealed class DeviceInput : IInputService
    {
        private readonly DefaultInputActions _inputAction;

        Vector2 IInputService.Movement => _inputAction.Player.Move.ReadValue<Vector2>();
        Vector2 IInputService.Look => _inputAction.Player.Look.ReadValue<Vector2>();
        bool IInputService.Attack => _inputAction.Player.Fire.IsInProgress();

        public DeviceInput()
        {
            _inputAction = new DefaultInputActions();
            _inputAction.Enable();
        }
    }
}