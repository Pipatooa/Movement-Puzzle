using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 DirectionToVector(int dir)
    {
        switch (dir)
        {
            case 0:
                return new Vector3(0, 0, 1);
            case 1:
                return new Vector3(1, 0, 0);
            case 2:
                return new Vector3(0, 0, -1);
            case 3:
                return new Vector3(-1, 0, 0);
            default:
                return Vector3.zero;
        }
    }
}
