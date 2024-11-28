using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Fields
    private Board _gameBoard;
    private Spawner _spawner;
    private Shape _activeShape;
    private InputController _inputController;
    private SoundManager _soundManager;
    private ScoreManager _scoreManager;
    private Ghost _ghost;

    private float _dropInterval = .3f;
    private float _dropIntervalModded;
    private float _timeToDrop;

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

    public ParticlePlayer GameOverFx;
    #endregion

    #region Unity Lifecycle Methods
    private void Start()
    {
        ValidateInstances();

        _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
        _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;
        _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;

        _dropIntervalModded = _dropInterval;

        Vector3 _roundedPosition = Vector3Int.RoundToInt(_spawner.transform.position);
        _spawner.transform.position = _roundedPosition;

        if (!_activeShape)
        {
            _activeShape = _spawner.SpawnShape();
        }
        if (GameOverPanel)
        {
            GameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!AreComponentsValid() || _gameOver)
            return;
        PlayerInput();
    }

    private void LateUpdate()
    {
        if (_ghost)
        {
            _ghost.DrawGhost(_activeShape);
        }
    }
    #endregion

    #region Input Handling
    private void PlayerInput()
    {
        if (!IsPaused)
        {
            if ((_inputController.GetPressingRight() && Time.time > _timeToNextKeyLeftRight) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            else if ((_inputController.GetPressingLeft() && Time.time > _timeToNextKeyLeftRight) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (_inputController.GetRotating() && Time.time > _timeToNextKeyRotate)
            {
                HandleRotationInput();
            }
            else if ((_inputController.GetPressingDown() && Time.time > _timeToNextKeyDown) || (Time.time > _timeToDrop))
            {
                HandleSoftDropInput();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleRotDirection();
            }
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    #endregion

    #region Shape Manipulation
    private void MoveRight()
    {
        _activeShape.MoveRight();
        _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
        bool isValid = _gameBoard.IsValidPosition(_activeShape);

        if (!isValid)
        {
            _activeShape.MoveLeft();
        }
        PlayMoveOrErrorSound(isValid);
    }

    private void PlayMoveOrErrorSound(bool isValid)
    {
        PlaySound(isValid ? _soundManager.MoveSound : _soundManager.ErrorSound, 0.5f);
    }

    private void MoveLeft()
    {
        _activeShape.MoveLeft();
        _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
        bool isValid = _gameBoard.IsValidPosition(_activeShape);
        if (!isValid)
        {
            _activeShape.MoveRight();
        }
        PlayMoveOrErrorSound(isValid);
    }

    private void HandleRotationInput()
    {
        _activeShape.RotateClockwise(_clockwise);
        _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;

        if (!_gameBoard.IsValidPosition(_activeShape))
        {
            _activeShape.RotateClockwise(!_clockwise);
            PlaySound(_soundManager.ErrorSound, 0.5f);
        }
        else
        {
            PlaySound(_soundManager.MoveSound, 0.5f);
        }
    }

    private void HandleSoftDropInput()
    {
        _timeToDrop = Time.time + _dropIntervalModded;
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
    #endregion

    #region Shape Landing
    private void LandShape()
    {
        if (_activeShape)
        {
            ResetInputTimers();
            _inputController.FalsePressingDown();
            _activeShape.MoveUp();
            _gameBoard.StoreShapeInGrid(_activeShape);
            _activeShape.LandShapeFX();
            if (_ghost)
            {
                _ghost.Reset();
            }
            _activeShape = _spawner.SpawnShape();
            StartCoroutine(_gameBoard.ClearAllRows());
            PlaySound(_soundManager.DropSound, 0.65f);

            HandleCompletedRows();
        }
    }

    private void HandleCompletedRows()
    {
        int completedRows = _gameBoard.ReturnCompletedRows();
        if (completedRows > 0)
        {
            _scoreManager.ScoreLines(completedRows);
            if (_scoreManager.ReturnDidLevelUp())
            {
                PlaySound(_soundManager.LevelUpVocalClip);
                _dropIntervalModded = Mathf.Clamp(_dropInterval - ((float)(_scoreManager.ReturnLevel() - 1) * 0.05f), 0.1f, 0.75f);
            }
            else if (completedRows > 1)
            {
                PlayVocalClipForCompletedRows(completedRows);
            }
            PlaySound(_soundManager.ClearRowSound);
        }
    }

    private void PlayVocalClipForCompletedRows(int completedRows)
    {
        if (completedRows == 2)
        {
            PlaySound(_soundManager.VocalClips[0]);
        }
        else if (completedRows == 3)
        {
            PlaySound(_soundManager.VocalClips[1]);
        }
        else
        {
            PlaySound(_soundManager.VocalClips[2]);
        }
    }

    private void ResetInputTimers()
    {
        _timeToNextKeyDown = Time.time;
        _timeToNextKeyRotate = Time.time;
        _timeToNextKeyLeftRight = Time.time;
    }
    #endregion

    #region Game Control
    private void GameOver()
    {
        _activeShape.MoveUp();
        _gameOver = true;
        Debug.LogWarning(_activeShape.name + " is over limit");
        StartCoroutine(ShowGameOverEffects());
        PlaySound(_soundManager.GameOverSound, 3f);
        PlaySound(_soundManager.GameOverVocalClip, 3f);
    }

    IEnumerator ShowGameOverEffects()
    {
        if (GameOverFx)
        {
            GameOverFx.Play();
        }
        yield return new WaitForSecondsRealtime(0.5f);
        if (GameOverPanel)
        {
            GameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Sound Handling
    private void PlaySound(AudioClip clip, float volMultiplier = 1f)
    {
        if (_soundManager.MoveSound && _soundManager.FxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(_soundManager.FxVolume * volMultiplier, 0.05f, 1f));
        }
    }
    #endregion

    #region UI Handling
    public void ToggleRotDirection()
    {
        _clockwise = !_clockwise;
        if (RotIconToggle)
        {
            RotIconToggle.ToggleIcon(_clockwise);
        }
    }

    public void TogglePause()
    {
        if (_gameOver)
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
    #endregion

    #region Validation Methods
    private void ValidateInstances()
    {
        ValidateInstance(InputController.Instance, ref _inputController, "InputController");
        ValidateInstance(Spawner.Instance, ref _spawner, "Spawner");
        ValidateInstance(Board.Instance, ref _gameBoard, "Board");
        ValidateInstance(SoundManager.Instance, ref _soundManager, "SoundManager");
        ValidateInstance(ScoreManager.Instance, ref _scoreManager, "ScoreManager");
        ValidateInstance(Ghost.Instance, ref _ghost, "Ghost");
    }

    private void ValidateInstance<T>(T instance, ref T field, string instanceName) where T : class
    {
        if (instance == null)
        {
            throw new NullReferenceException($"{instanceName} instance is not defined.");
        }
        field = instance;
    }

    private bool AreComponentsValid()
    {
        return _gameBoard && _spawner && _activeShape && _soundManager && _scoreManager;
    }
    #endregion
}