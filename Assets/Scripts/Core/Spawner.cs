using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    public Shape[] AllShapes;
    public Transform[] QueuedXForms = new Transform[3];
    private Shape[] _queuedShapes = new Shape[3];
    private float _queueScale = 0.5f;

    private void Awake()
    {
        Instance = this;
        InitQueue();
    }

    private void Start()
    {

    }
    private Shape GetRandomShape()
    {
        int i = Random.Range(0, AllShapes.Length);
        if(AllShapes[i])
        {
            return AllShapes[i];
        } else
        {
            Debug.LogWarning("Invalid shape.");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = GetQueuedShape();
        shape.transform.position = transform.position;
        shape.transform.localScale = Vector3.one;
        if (shape)
        {
            return shape;
        } else
        {
            Debug.LogWarning("Invalid shape in spawner.");
            return null;
        }
    }

    private void InitQueue()
    {
        for(int i=0; i < _queuedShapes.Length; i++)
        {
            _queuedShapes[i] = null;
        }
        FillQueue();
    }

    private void FillQueue()
    {
        for (int i = 0; i < _queuedShapes.Length; i++)
        {
            if(!_queuedShapes[i])
            {
                _queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                _queuedShapes[i].transform.position = QueuedXForms[i].position;
                _queuedShapes[i].transform.localScale = new Vector3(_queueScale, _queueScale, _queueScale);
            }
        }
    }

    private Shape GetQueuedShape()
    {
        Shape firstShape = null;

        if(_queuedShapes[0])
        {
            firstShape = _queuedShapes[0];
        }

        ShiftQueue();

        _queuedShapes[_queuedShapes.Length - 1] = null;
        FillQueue();

        return firstShape;
    }

    private void ShiftQueue()
    {
        for (int i = 1; i < _queuedShapes.Length; i++)
        {
            if (_queuedShapes[i] != null)
            {
                _queuedShapes[i - 1] = _queuedShapes[i];
                _queuedShapes[i - 1].transform.position = QueuedXForms[i - 1].position;
            }
        }
    }
}
