using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    Board _gameBoard;
    Spawner _spawner;
    Shape _activeShape;
    InputAction _test;
    PlayerControls _inputActions;
    bool pressing;
    

    float _dropInterval = .15f;
    float _timeToDrop;
    float _timeToNextKey;
    [Range(0.02f,1f)]
    public float KeyRepeatRate = .25f;

    private void Awake()
    {
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
    private void Start()
    {
        _timeToNextKey = Time.time;
        _spawner = Spawner.Instance;
        _gameBoard = Board.Instance;

        if (_spawner)
        {
            if(_activeShape == null)
            {
                _activeShape = _spawner.SpawnShape();
            }
            Vector3 _roundedPosition = Vector3Int.RoundToInt(_spawner.transform.position);
            _spawner.transform.position = _roundedPosition;
        }

        if (!_spawner)
        {
            Debug.Log("Spawner not defined");
        }
        if (!_gameBoard)
        {
            Debug.Log("Game Board not defined");
        }

        //_gameBoard = GameObject.FindObjectOfType<Board>();
        //_spawner = GameObject.FindObjectOfType<Spawner>();
    }

    private void Update()
    {
        if (GetPressing() && Time.time > _timeToNextKey)
        {
            _activeShape.MoveRight();
            _timeToNextKey = Time.time + KeyRepeatRate;
            if (_gameBoard.IsValidPosition(_activeShape))
            {
                Debug.Log("Move right");
            }
            else
            {
                _activeShape.MoveLeft();
                Debug.Log("hit the boundary");
            }
        }
        if(Time.time > _timeToDrop)
        {
            _timeToDrop = Time.time + _dropInterval;
            if (_activeShape)
            {
                _activeShape.MoveDown();

                if(!_gameBoard.IsValidPosition (_activeShape))
                {
                    //shape landing
                    _activeShape.MoveUp();
                    _gameBoard.StoreShapeInGrid(_activeShape);
                    if(_spawner)
                    {
                        _activeShape = _spawner.SpawnShape();
                    }
                }
            }
        }
    }
}
