using System;
using System.Numerics;

public static class TriangleRasterization
{
    public struct WritableBufferDescription
    {
        public delegate void SetDelegate(int x, int y, int value);
        public SetDelegate Set;
    }

    public static void RasterizeTriangle(WritableBufferDescription buffer, int width, int height, Vector2 v0, Vector2 v1, Vector2 v2, int value)
    {
        // Find bounding box around the triangle
        int minX = (int)Math.Max(0, Math.Min(v0.X, Math.Min(v1.X, v2.X)));
        int minY = (int)Math.Max(0, Math.Min(v0.Y, Math.Min(v1.Y, v2.Y)));
        int maxX = (int)Math.Min(width - 1, Math.Max(v0.X, Math.Max(v1.X, v2.X)));
        int maxY = (int)Math.Min(height - 1, Math.Max(v0.Y, Math.Max(v1.Y, v2.Y)));

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                // Check if the point (x, y) is inside the triangle
                if (IsPointInTriangle(new Vector2(x, y), v0, v1, v2))
                {
                    // Set the pixel color
                    buffer.Set(x, y, value);
                }
            }
        }
    }

    private static bool IsPointInTriangle(Vector2 p, Vector2 v0, Vector2 v1, Vector2 v2)
    {
        // Barycentric coordinates method
        float area = 0.5f * (-v1.Y * v2.X + v0.Y * (-v1.X + v2.X) + v0.X * (v1.Y - v2.Y) + v1.X * v2.Y);
        float s = 1 / (2 * area) * (v0.Y * v2.X - v0.X * v2.Y + (v2.Y - v0.Y) * p.X + (v0.X - v2.X) * p.Y);
        float t = 1 / (2 * area) * (v0.X * v1.Y - v0.Y * v1.X + (v0.Y - v1.Y) * p.X + (v1.X - v0.X) * p.Y);

        return s >= 0 && t >= 0 && (s + t) <= 1;
    }
}
