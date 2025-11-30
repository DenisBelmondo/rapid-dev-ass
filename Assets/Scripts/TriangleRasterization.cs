using System.Numerics;
using System;
using System.Collections;

public static class TriangleRasterization
{
    public struct WritableBufferInterface<TValue>
    {
        public delegate void SetDelegate(int x, int y, TValue value);

        public SetDelegate Set;
        public int BufferWidth;
        public int BufferHeight;

        public WritableBufferInterface(SetDelegate set, int bufferWidth, int bufferHeight)
        {
            Set = set;
            BufferWidth = bufferWidth;
            BufferHeight = bufferHeight;
        }
    }

    public static void RasterizeTriangle<TValue>(WritableBufferInterface<TValue> bufferInterface, Vector2 v0, Vector2 v1, Vector2 v2, TValue value)
    {
        // Get the minimum and maximum bounding box
        int minX = (int)Math.Min(Math.Min(v0.X, v1.X), v2.X);
        int maxX = (int)Math.Max(Math.Max(v0.X, v1.X), v2.X);
        int minY = (int)Math.Min(Math.Min(v0.Y, v1.Y), v2.Y);
        int maxY = (int)Math.Max(Math.Max(v0.Y, v1.Y), v2.Y);

        // Iterate through the bounding box
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                // Calculate barycentric coordinates
                if (IsPointInTriangle(new(x, y), v0, v1, v2))
                {
                    bufferInterface.Set?.Invoke(x, y, value);
                }
            }
        }
    }

    public static IEnumerator RasterizeTriangleAsync<TValue>(WritableBufferInterface<TValue> bufferInterface, Vector2 v0, Vector2 v1, Vector2 v2, TValue value)
    {
        // Get the minimum and maximum bounding box
        int minX = (int)Math.Min(Math.Min(v0.X, v1.X), v2.X);
        int maxX = (int)Math.Max(Math.Max(v0.X, v1.X), v2.X);
        int minY = (int)Math.Min(Math.Min(v0.Y, v1.Y), v2.Y);
        int maxY = (int)Math.Max(Math.Max(v0.Y, v1.Y), v2.Y);

        // Iterate through the bounding box
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                // Calculate barycentric coordinates
                if (IsPointInTriangle(new(x, y), v0, v1, v2))
                {
                    bufferInterface.Set?.Invoke(x, y, value);
                    yield return new UnityEngine.WaitForSeconds(1 / 200F);
                }
            }
        }

        yield return null;
    }

    private static bool IsPointInTriangle(Vector2 p, Vector2 v0, Vector2 v1, Vector2 v2)
    {
        // Barycentric coordinates method
        float area = 0.5f * (-v1.Y * v2.X + v0.Y * (-v1.X + v2.X) + v0.X * (v1.Y - v2.Y) + v1.X * v2.Y);
        float s = 1 / (2 * area) * (v0.Y * v2.X - v0.X * v2.Y + (v2.Y - v0.Y) * p.X + (v0.X - v2.X) * p.Y);
        float t = 1 / (2 * area) * (v0.X * v1.Y - v0.Y * v1.X + (v0.Y - v1.Y) * p.X + (v1.X - v0.X) * p.Y);


        float epsilon = 0.1f;

        //return s >= 0 && t >= 0 && (s + t) <= 1;
        return s >= -epsilon && t >= -epsilon && (s + t) <= 1 + epsilon;
    }

    private static float TriangleArea(Vector2 v0, Vector2 v1, Vector2 v2)
    {
        return Math.Abs((v0.X * (v1.Y - v2.Y) + v1.X * (v2.Y - v0.Y) + v2.X * (v0.Y - v1.Y)) / 2.0f);
    }
}
