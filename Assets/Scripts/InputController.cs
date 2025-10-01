using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MoveInputEvent : UnityEvent<float> { }
[Serializable]
public class JumpInputEvent : UnityEvent { }
[Serializable]
public class RollInputEvent : UnityEvent { }

public class InputController : MonoBehaviour
{
    InputSystem_Actions controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    public RollInputEvent rollInputEvent;

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

        controls.Player.Roll.performed += OnRollPerformed;
        //if you want something to happen when the player
        //lets go of the button, add in a .canceled line (like move has)
        //and create a new OnRollCanceled function
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x);
        //Debug.Log(moveInput);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpInputEvent.Invoke();
    }

    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        rollInputEvent.Invoke();
    }
}