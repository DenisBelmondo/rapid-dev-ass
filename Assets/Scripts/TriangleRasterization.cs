using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public static class TriangleRasterization
{
    private const float UNIFORM_DURATION = 0.5f;

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

    public static int HowManyTiles(Vector2 v0, Vector2 v1, Vector2 v2)
    {
        int numTiles = 0;

        //
        // old implementation (RESTORE LATER)
        //

        /*
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
                    numTiles += 1;
                }
            }
        } */

        // Get the minimum and maximum bounding box
        int minX = (int)Math.Min(Math.Min(v0.X, v1.X), v2.X);
        int maxX = (int)Math.Max(Math.Max(v0.X, v1.X), v2.X);
        int minY = (int)Math.Min(Math.Min(v0.Y, v1.Y), v2.Y);
        int maxY = (int)Math.Max(Math.Max(v0.Y, v1.Y), v2.Y);

        // Create a list to store cells and their distances
        var cells = new List<(int x, int y, double distance)>();

        // Iterate through the bounding box and calculate distances
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                double distance = Math.Sqrt((x - v0.X) * (x - v0.X) + (y - v0.Y) * (y - v0.Y));
                cells.Add((x, y, distance));
            }
        }

        // Sort cells by distance to v0
        cells.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Fill the cells starting from v0 based on sorted distance
        foreach (var (x, y, distance) in cells)
        {
            if (IsPointInTriangle(new Vector2(x, y), v0, v1, v2))
            {
                numTiles += 1;
            }
        }

        return numTiles;
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
        //const int AVERAGE_NUM_TILES = 20;

        //
        // old implementation (RESTORE LATER)
        //

        /*
        int numTiles = HowManyTiles(v0, v1, v2);
        if (numTiles <= 0)
        {
            Debug.LogError("Cannot rasterize a triangle with a negative number of tiles.");
        }
        float delayPerTile = UNIFORM_DURATION / numTiles;

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
                    //yield return new UnityEngine.WaitForSeconds(1 / 200F / speedMul);
                    yield return new UnityEngine.WaitForSeconds(delayPerTile);
                }
            }
        } */

        int numTiles = HowManyTiles(v0, v1, v2);

        if (numTiles <= 0)
        {
            Debug.LogError("Cannot rasterize a triangle with a negative number of tiles.");
        }

        float delayPerTile = UNIFORM_DURATION / numTiles;

        // Get the minimum and maximum bounding box
        int minX = (int)Math.Min(Math.Min(v0.X, v1.X), v2.X);
        int maxX = (int)Math.Max(Math.Max(v0.X, v1.X), v2.X);
        int minY = (int)Math.Min(Math.Min(v0.Y, v1.Y), v2.Y);
        int maxY = (int)Math.Max(Math.Max(v0.Y, v1.Y), v2.Y);

        // Create a list to store cells and their distances
        var cells = new List<(int x, int y, double distance)>();

        // Iterate through the bounding box and calculate distances
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                double distance = Math.Sqrt((x - v0.X) * (x - v0.X) + (y - v0.Y) * (y - v0.Y));
                cells.Add((x, y, distance));
            }
        }

        // Sort cells by distance to v0
        cells.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Fill the cells starting from v0 based on sorted distance
        foreach (var (x, y, distance) in cells)
        {
            if (IsPointInTriangle(new Vector2(x, y), v0, v1, v2))
            {
                bufferInterface.Set?.Invoke(x, y, value);
                //yield return new UnityEngine.WaitForSeconds(1 / 200F / speedMul);
                yield return new UnityEngine.WaitForSeconds(delayPerTile);
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
