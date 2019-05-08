using System;
using UnityEngine;

namespace Pentago
{
    public class Geometry
    {
        static public Vector3 PointFromGrid(int col, int row)
        {
            float x = -2.5f + 1.0f * col;
            float z = 2.5f - 1.0f * row;
            return new Vector3(x, 0, z);
        }

        static public Vector3 PointFromGrid(Vector2Int gridPoint)
        {
            return PointFromGrid(gridPoint.x, gridPoint.y);
        }

        static public Vector2Int GridFromPoint(Vector3 point)
        {
            int col = Mathf.FloorToInt(3.0f + point.x);
            int row = Math.Abs(Mathf.FloorToInt(point.z - 2.0f));
            return new Vector2Int(col, row);
        }
    }
}