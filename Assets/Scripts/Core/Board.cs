using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform EmptySprite;
    public int Height = 30;
    public int Width = 10;
    public int Header = 8;

    Transform[,] _grid;

    private void Awake()
    { 
        _grid = new Transform[Width, Height];
    }

    void Start()
    {
        DrawEmptyCells();
    }

    void DrawEmptyCells()
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
}
