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

    Transform[,] _grid;

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
}
