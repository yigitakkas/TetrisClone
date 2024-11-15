using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vectorf
{
    public static Vector2 Round(Vector2 _v)
    {
        return new Vector2(Mathf.Round(_v.x), Mathf.Round(_v.y));
    }

    public static Vector3 Round(Vector3 _v)
    {
        return new Vector3(Mathf.Round(_v.x), Mathf.Round(_v.y), Mathf.Round(_v.z));
    }
}
