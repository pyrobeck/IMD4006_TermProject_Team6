using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MoveInputEvent : UnityEvent<float> { }
[Serializable]
public class JumpInputEvent : UnityEvent { }
[Serializable]
public class JumpCancelEvent : UnityEvent { }
[Serializable]
public class RollInputEvent : UnityEvent { }
[Serializable]
public class PickupInputEvent : UnityEvent { }
[Serializable]
public class PickupCancelEvent : UnityEvent { }
public class InputController : MonoBehaviour
{
    InputSystem_Actions controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    public JumpCancelEvent jumpCancelEvent;
    public RollInputEvent rollInputEvent;
    public PickupInputEvent pickupInputEvent;
    public PickupCancelEvent pickupCancelEvent;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMovePerformed;

        controls.Player.Jump.performed += OnJumpPerformed;
        controls.Player.Jump.canceled += OnJumpCanceled;

        controls.Player.Roll.performed += OnRollPerformed;

        controls.Player.Pickup.performed += OnPickupPerformed;
        controls.Player.Pickup.canceled += OnPickupCancelled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpInputEvent.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpCancelEvent.Invoke();
    }

    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        rollInputEvent.Invoke();
    }

    private void OnPickupPerformed(InputAction.CallbackContext context)
    {
        pickupInputEvent.Invoke();
    }
    private void OnPickupCancelled(InputAction.CallbackContext context)
    {
        pickupCancelEvent.Invoke();
    }
}