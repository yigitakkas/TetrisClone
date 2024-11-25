using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Board _gameBoard;
    private Spawner _spawner;
    private Shape _activeShape;
    private InputController _inputController;
    private SoundManager _soundManager;

    public float _dropInterval = .3f;
    private float _timeToDrop;
    //float _timeToNextKey;
    //[Range(0.02f,1f)]
    //public float KeyRepeatRate = .25f;

    [Range(0.02f, 1f)]
    public float KeyRepeatRateLeftRight = .2f;
    private float _timeToNextKeyLeftRight;

    [Range(0.02f, 1f)]
    public float KeyRepeatRateDown = .15f;
    private float _timeToNextKeyDown;

    [Range(0.01f, 1f)]
    public float KeyRepeatRateRotate = .2f;
    private float _timeToNextKeyRotate;

    private bool _gameOver = false;

    public GameObject GameOverPanel;

    public IconToggle RotIconToggle;

    private bool _clockwise = true;

    public bool IsPaused = false;

    public GameObject PausePanel;


    private void Awake()
    {
    }
    private void Start()
    {
        _inputController = InputController.Instance;
        _spawner = Spawner.Instance;
        _gameBoard = Board.Instance;
        _soundManager = SoundManager.Instance;

        _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
        _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;
        _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;

        if (!_gameBoard)
        {
            Debug.Log("Game Board not defined");
        }
        if (!_soundManager)
        {
            Debug.Log("Sound manager not defined");
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
        if(GameOverPanel)
        {
            GameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_gameBoard || !_spawner || !_activeShape || _gameOver || !_soundManager)
            return;
        PlayerInput();
    }

    private void PlayerInput()
    {
        if(!IsPaused)
        {
            if (_inputController.GetPressingRight() && Time.time > _timeToNextKeyLeftRight || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _activeShape.MoveRight();
                _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
                if (!_gameBoard.IsValidPosition(_activeShape))
                {
                    _activeShape.MoveLeft();
                    PlaySound(_soundManager.ErrorSound, .5f);
                }
                else
                {
                    PlaySound(_soundManager.MoveSound, .5f);
                }
            }
            else if (_inputController.GetPressingLeft() && Time.time > _timeToNextKeyLeftRight || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _activeShape.MoveLeft();
                _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
                if (!_gameBoard.IsValidPosition(_activeShape))
                {
                    _activeShape.MoveRight();
                    PlaySound(_soundManager.ErrorSound, .5f);
                }
                else
                {
                    PlaySound(_soundManager.MoveSound, .5f);
                }
            }
            else if (_inputController.GetRotating() && Time.time > _timeToNextKeyRotate)
            {
                _activeShape.RotateClockwise(_clockwise);
                _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;
                if (!_gameBoard.IsValidPosition(_activeShape))
                {
                    _activeShape.RotateClockwise(!_clockwise);
                    PlaySound(_soundManager.ErrorSound, .5f);
                }
                else
                {
                    PlaySound(_soundManager.MoveSound, .5f);
                }
            }
            else if (_inputController.GetPressingDown() && Time.time > _timeToNextKeyDown || (Time.time > _timeToDrop))
            {
                _timeToDrop = Time.time + _dropInterval;
                _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
                _activeShape.MoveDown();

                if (!_gameBoard.IsValidPosition(_activeShape))
                {
                    if (_gameBoard.IsOverLimit(_activeShape))
                    {
                        GameOver();
                    }
                    else
                    {
                        LandShape();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleRotDirection();
            }
        }
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
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

        _gameBoard.ClearAllRows();

        PlaySound(_soundManager.DropSound, 0.65f);
        if(_gameBoard.ReturnCompletedRows()>0)
        {
            if(_gameBoard.ReturnCompletedRows() > 1)
            {
                if(_gameBoard.ReturnCompletedRows() == 2)
                {
                    PlaySound(_soundManager.VocalClips[0]);
                } else if (_gameBoard.ReturnCompletedRows() == 3)
                {
                    PlaySound(_soundManager.VocalClips[1]);
                } else
                {
                    PlaySound(_soundManager.VocalClips[2]);
                }
            }
            PlaySound(_soundManager.ClearRowSound);
        }
    }
    private void GameOver()
    {
        _activeShape.MoveUp();
        _gameOver = true;
        Debug.LogWarning(_activeShape.name + " is over limit");

        if (GameOverPanel)
        {
            GameOverPanel.SetActive(true);
        }
        PlaySound(_soundManager.GameOverSound, 3f);
        PlaySound(_soundManager.GameOverVocalClip, 3f);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlaySound(AudioClip clip, float volMultiplier =1f)
    {
        if(_soundManager.MoveSound && _soundManager.FxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position,Mathf.Clamp(_soundManager.FxVolume * volMultiplier,0.05f,1f));
        }
    }

    public void ToggleRotDirection()
    {
        _clockwise = !_clockwise;
        if(RotIconToggle)
        {
            RotIconToggle.ToggleIcon(_clockwise);
        }
    }

    public void TogglePause()
    {
        if(_gameOver)
        {
            return;
        }
        IsPaused = !IsPaused;

        if (PausePanel)
        {
            PausePanel.SetActive(IsPaused);

            if (_soundManager != null)
            {
                _soundManager.MusicSource.volume = IsPaused ? _soundManager.MusicVolume * 0.25f : _soundManager.MusicVolume;
            }

            Time.timeScale = IsPaused ? 0 : 1;
        }

    }
}
