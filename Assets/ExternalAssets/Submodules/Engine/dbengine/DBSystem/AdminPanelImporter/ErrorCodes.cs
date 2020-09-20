using System;
using UnityEngine;


public static class ErrorCodes
{
    public const float Tolerance = 0.000001f;

    public const int IntError = -1;
    public const float FloatError = -1;
    public const double DoubleError = -1;
    public static readonly Vector2 ErrorVector2 = new Vector2(-1, -1);

    public static bool IsError(int exemplar)
    {
        return exemplar == IntError;
    }

    public static bool IsError(float exemplar)
    {
        return Math.Abs(exemplar - FloatError) < Tolerance;
    }

    public static bool IsError(double exemplar)
    {
        return Math.Abs(exemplar - DoubleError) < Tolerance;
    }

    public static bool IsError(Vector2 exemplar)
    {
        return Math.Abs(exemplar.x - ErrorVector2.x) < Tolerance && Math.Abs(exemplar.y - ErrorVector2.y) < Tolerance;
    }
}
