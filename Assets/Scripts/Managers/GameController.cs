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
    InputController _inputController;

    float _dropInterval = .3f;
    float _timeToDrop;
    //float _timeToNextKey;
    //[Range(0.02f,1f)]
    //public float KeyRepeatRate = .25f;

    [Range(0.02f, 1f)]
    public float KeyRepeatRateLeftRight = .2f;
    float _timeToNextKeyLeftRight;

    [Range(0.02f, 1f)]
    public float KeyRepeatRateDown = .15f;
    float _timeToNextKeyDown;

    [Range(0.01f, 1f)]
    public float KeyRepeatRateRotate = .2f;
    float _timeToNextKeyRotate;

    private void Awake()
    {
        _inputController = InputController.Instance;
    }
    private void Start()
    {
        _spawner = Spawner.Instance;
        _gameBoard = Board.Instance;

        _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
        _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;
        _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;

        if (!_gameBoard)
        {
            Debug.Log("Game Board not defined");
        }
        if (!_spawner)
        {
            Debug.Log("Spawner not defined");
        } else
        {
            Vector3 _roundedPosition = Vector3Int.RoundToInt(_spawner.transform.position);
            _spawner.transform.position = _roundedPosition;
            if (!_activeShape)
            {
                _activeShape = _spawner.SpawnShape();
            }
        }

        //_gameBoard = GameObject.FindObjectOfType<Board>();
        //_spawner = GameObject.FindObjectOfType<Spawner>();
    }

    private void PlayerInput()
    {
        if (_inputController.GetPressingRight() && Time.time > _timeToNextKeyLeftRight || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _activeShape.MoveRight();
            _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.MoveLeft();
            }
        }
        else if (_inputController.GetPressingLeft() && Time.time > _timeToNextKeyLeftRight || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _activeShape.MoveLeft();
            _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.MoveRight();
            }
        }
        else if (_inputController.GetRotating() && Time.time > _timeToNextKeyRotate)
        {
            _activeShape.RotateRight();
            _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;
            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.RotateLeft();
            }
        }
        else if(_inputController.GetPressingDown() && Time.time > _timeToNextKeyDown || (Time.time > _timeToDrop))
        {
            _timeToDrop = Time.time + _dropInterval;
            _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
            _activeShape.MoveDown();

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                LandShape();
            }
        }
    }

    private void LandShape()
    {
        _timeToNextKeyDown = Time.time;
        _timeToNextKeyRotate = Time.time;
        _timeToNextKeyLeftRight = Time.time;
        _inputController.FalsePressingDown();
        _activeShape.MoveUp();
        _gameBoard.StoreShapeInGrid(_activeShape);
        _activeShape = _spawner.SpawnShape();
    }

    private void Update()
    {
        if (!_gameBoard || !_spawner || !_activeShape)
            return;
        PlayerInput();
    }
}
