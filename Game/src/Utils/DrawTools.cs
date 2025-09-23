/* A set of tools to help draw sprites */

using System.Numerics;
using Raylib_cs;

public static class DrawTools
{
    public static void DrawFullTriangle(
        CellCoordinates direction,
        Vector2 position,
        int size,
        Color color
    )
    {
        double orientation = Math.Atan2(direction.Y, direction.X);
        Vector2 edge1 = new(
            position.X + size * (float)Math.Cos(orientation),
            position.Y + size * (float)Math.Sin(orientation)
        );
        Vector2 edge2 = new(
            position.X + size * (float)Math.Cos(orientation + 2 * Math.PI / 3),
            position.Y + size * (float)Math.Sin(orientation + 2 * Math.PI / 3)
        );
        Vector2 edge3 = new(
            position.X + size * (float)Math.Cos(orientation + 4 * Math.PI / 3),
            position.Y + size * (float)Math.Sin(orientation + 4 * Math.PI / 3)
        );
        // Order of vertices is not the same depending if you draw a full or empty triangle shape.
        Raylib.DrawTriangle(edge1, edge3, edge2, color);
        //Raylib.DrawTriangleLines(edge1, edge2, edge3, _triangleColor);
    }
}
