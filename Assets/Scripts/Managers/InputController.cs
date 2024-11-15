using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    InputAction _test;
    PlayerControls _inputActions;
    bool pressing;

    private void Awake()
    {
        Instance = this;
        _inputActions = new PlayerControls();
        _test = _inputActions.Movement.MoveRight;
        _inputActions.Movement.MoveRight.performed += MoveRight;
        _inputActions.Movement.MoveRight.canceled += StopMovingRight;
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
        pressing = true;
    }
    private void StopMovingRight(InputAction.CallbackContext obj)
    {
        pressing = false;
    }
    public bool GetPressing()
    {
        return pressing;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
