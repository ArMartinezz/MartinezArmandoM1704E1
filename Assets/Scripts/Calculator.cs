using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calculator
{
    public static float distance = 0.523f;
    public static int length = 8;

    public static Vector3 GetPositionFromMatrix(int[]point)
    {
        return new Vector3(-(length - 1f) * distance + point[1] * 2f * distance,
            (length - 1f) * distance - point[0] * 2f * distance, 0);
    }
    public static float CheckDistanceToObj(int[] point, int[] obj)
    {
        return Vector3.Distance(GetPositionFromMatrix(obj), GetPositionFromMatrix(point));
    }
}
