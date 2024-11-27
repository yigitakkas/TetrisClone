using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    public Transform EmptySprite;
    public int Height = 30;
    public int Width = 10;
    public int Header = 8;
    private int _completedRows = 0;
    private Transform[,] _grid;
    public ParticlePlayer[] RowGlowFX = new ParticlePlayer[4];

    private void Awake()
    {
        Instance = this;
        _grid = new Transform[Width, Height];
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    private void DrawEmptyCells()
    {
        if(EmptySprite)
        {
            for (int y = 0; y < Height - Header; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Transform _clone;
                    _clone = Instantiate(EmptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    _clone.name = "Board Space ( x = " + x.ToString() + " , y =" + y.ToString() + " )";
                    _clone.transform.parent = transform;
                }
            }
        }
    }

    private bool IsWithinBoard(int x,int y)
    {
        return (x >= 0 && x < Width && y >= 0);
    }
    private bool IsOccupied(int x, int y, Shape shape)
    {
        return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            if(!IsWithinBoard((int) pos.x, (int) pos.y))
            {
                return false;
            }
            if(IsOccupied((int) pos.x, (int) pos.y, shape))
            {
                return false;
            }
        }
        return true;
    }


    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
            return;
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            _grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    private bool IsComplete(int y)
    {
        for (int x = 0; x < Width; ++x)
        {
            if (_grid[x,y] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearRow(int y)
    {
        for(int x=0;x <Width; ++x)
        {
            if(_grid[x,y] != null)
            {
                Destroy(_grid[x, y].gameObject);
            }
            _grid[x, y] = null;
        }
    }

    private void ShiftOneRowDown(int y)
    {
        for(int x = 0; x < Width; ++x)
        {
            if(_grid[x,y] != null)
            {
                _grid[x, y - 1] = _grid[x, y];
                _grid[x, y] = null;
                _grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void ShiftRowsDown(int startY)
    {
        for(int i = startY; i < Height; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    public IEnumerator ClearAllRows()
    {
        _completedRows = 0;
        for (int y = 0; y < Height; ++y)
        {
            if (IsComplete(y))
            {
                ClearRowFX(_completedRows, y);
                _completedRows++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int y=0; y < Height; ++y)
        {
            if(IsComplete(y))
            {
                ClearRow(y);
                ShiftRowsDown(y + 1);
                yield return new WaitForSeconds(0.3f);
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            if (child.transform.position.y >= Height - Header - 1)
            {
                return true;
            }
        }
        return false;
    }

    public int ReturnCompletedRows()
    {
        return _completedRows;
    }

    private void ClearRowFX(int index, int y)
    {
        if (RowGlowFX[index])
        {
            RowGlowFX[index].transform.position = new Vector3(0, y, -2f);
            RowGlowFX[index].Play();
        }
    }
}
