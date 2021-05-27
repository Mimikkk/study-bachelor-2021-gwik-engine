using System;
using System.Numerics;

namespace GWiK_Sokoban.engine
{
    public static class MathHelper
    {
        public static float DegreesToRadians(float degrees)
        {
            return MathF.PI / 180f * degrees;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * (180f / MathF.PI);
        }

        public static Vector3 GenerateNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var u = p1 - p2;
            var v = p3 - p2;
            var generatedNormal = Vector3.Cross(u, v);
            return generatedNormal;
        }
    }

    public static class Vector3Extensions
    {
        public static double AngleBetween(this Vector3 @this, Vector3 other)
        {
            var angle = Vector3.Dot(@this, other) / (@this.Length() * other.Length());
            return Math.Acos(angle);
        }
    }
}