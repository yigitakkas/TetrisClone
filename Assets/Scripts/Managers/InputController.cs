using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    InputAction _test;
    PlayerControls _inputActions;
    private bool _pressingRight;
    private bool _pressingLeft;
    private bool _rotating;
    private bool _pressingDown;

    private void Awake()
    {
        Instance = this;
        _inputActions = new PlayerControls();
        _test = _inputActions.Movement.MoveRight;
        _inputActions.Movement.MoveRight.performed += MoveRight;
        _inputActions.Movement.MoveRight.canceled += StopMovingRight;
        _inputActions.Movement.MoveLeft.performed += MoveLeft;
        _inputActions.Movement.MoveLeft.canceled += StopMovingLeft;
        _inputActions.Movement.Rotate.performed += Rotate;
        _inputActions.Movement.Rotate.canceled += StopRotate;
        _inputActions.Movement.MoveDown.performed += MoveDown;
        _inputActions.Movement.MoveDown.canceled += StopMovingDown;
    }
    private void OnEnable()
    {
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    private void MoveRight(InputAction.CallbackContext ctx)
    {
        _pressingRight = true;
    }
    private void StopMovingRight(InputAction.CallbackContext ctx)
    {
        _pressingRight = false;
    }
    public bool GetPressingRight()
    {
        return _pressingRight;
    }
    private void MoveLeft(InputAction.CallbackContext ctx)
    {
        _pressingLeft = true;
    }
    private void StopMovingLeft(InputAction.CallbackContext ctx)
    {
        _pressingLeft = false;
    }
    public bool GetPressingLeft()
    {
        return _pressingLeft;
    }
    private void Rotate(InputAction.CallbackContext ctx)
    {
        _rotating=true;
    }
    private void StopRotate(InputAction.CallbackContext ctx)
    {
        _rotating = false;
    }
    public bool GetRotating()
    {
        return _rotating;
    }
    private void MoveDown(InputAction.CallbackContext ctx)
    {
        _pressingDown = true;
    }
    private void StopMovingDown(InputAction.CallbackContext ctx)
    {
        _pressingDown = false;
    }
    public bool GetPressingDown()
    {
        return _pressingDown;
    }

    public void FalsePressingDown()
    {
        _pressingDown = false;
    }
}
