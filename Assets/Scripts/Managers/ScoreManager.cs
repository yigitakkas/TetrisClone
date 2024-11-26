using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int _score = 0;
    private int _lines;
    private int _level = 1;

    public int LinesPerLevel = 5;
    private const int _minLines = 1;
    private const int _maxLines = 4;

    public TMP_Text LinesText;
    public TMP_Text LevelText;
    public TMP_Text ScoreText;

    private bool _didLevelUp = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Reset();
    }
    public void ScoreLines(int n)
    {
        _didLevelUp = false;
        n = Mathf.Clamp(n, _minLines, _maxLines);
        switch(n)
        {
            case 1:
                _score += 40 * _level;
                break;
            case 2:
                _score += 100 * _level;
                break;
            case 3:
                _score += 300 * _level;
                break;
            case 4:
                _score += 1200 * _level;
                break;
        }
        _lines -= n;
        if(_lines <= 0)
        {
            LevelUp();
        }

        UpdateUIText();
    }
    public void Reset()
    {
        _level = 1;
        _lines = LinesPerLevel * _level;
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        if (LinesText)
        {
            LinesText.text = _lines.ToString();
        }

        if (LevelText)
        {
            LevelText.text = _level.ToString();
        }

        if (ScoreText)
        {
            ScoreText.text = PadZero(_score, 5);
        }

    }
    private string PadZero(int number, int padDigits)
    {
        string numberStr = number.ToString();
        while (numberStr.Length < padDigits)
        {
            numberStr = "0" + numberStr;
        }
        return numberStr;
    }

    public void LevelUp()
    {
        _level++;
        _lines = LinesPerLevel * _level;
        _didLevelUp = true;
    }

    public bool ReturnDidLevelUp()
    {
        return _didLevelUp;
    }

    public int ReturnLevel()
    {
        return _level;
    }


    void Update()
    {
        
    }
}
