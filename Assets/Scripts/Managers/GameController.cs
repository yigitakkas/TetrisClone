using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board _gameBoard;
    Spawner _spawner;
    void Start()
    {
        _spawner = Spawner.Instance;
        _gameBoard = Board.Instance;

        if (_spawner)
        {
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

    void Update()
    {
        
    }
}
