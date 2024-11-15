using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    public Shape[] AllShapes;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    Shape GetRandomShape()
    {
        int i = Random.Range(0, AllShapes.Length);
        if (AllShapes[i])
        {
            return AllShapes[i];
        }
        else
        {
            Debug.LogWarning("Invalid shape.");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape _shape = null;
        _shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        if (_shape)
        {
            return _shape;
        }
        else
        {
            Debug.LogWarning("Invalid shape in spawner.");
            return null;
        }
    }
}
