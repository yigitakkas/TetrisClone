using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Ghost Instance;
    private Shape _ghostShape = null;
    private bool _hitBottom = false;
    private Board _gameBoard;
    public Color GhostColor = new Color(1f, 1f, 1f, 0.2f);

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _gameBoard = Board.Instance;
    }
    public void DrawGhost(Shape originalShape)
    {
        if(!_ghostShape)
        {
            _ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            _ghostShape.gameObject.name = "GhostShape";
            SpriteRenderer[] allRenderers = _ghostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach(SpriteRenderer r in allRenderers)
            {
                r.color = GhostColor;
            }
        } else
        {
            _ghostShape.transform.position = originalShape.transform.position;
            _ghostShape.transform.rotation = originalShape.transform.rotation;
        }

        _hitBottom = false;

        while(!_hitBottom)
        {
            _ghostShape.MoveDown();
            if(!_gameBoard.IsValidPosition(_ghostShape))
            {
                _ghostShape.MoveUp();
                _hitBottom = true;
            }
        }
    }

    public void Reset()
    {
        Destroy(_ghostShape.gameObject);
    }


    void Update()
    {
        
    }
}
