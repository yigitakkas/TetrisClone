using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board _gameBoard;
    Spawner _spawner;
    Shape _activeShape;

    float _dropInterval = .25f;
    float _timeToDrop;
    private void Start()
    {
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
        if (!_gameBoard || !_spawner)
        {
            return;
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
