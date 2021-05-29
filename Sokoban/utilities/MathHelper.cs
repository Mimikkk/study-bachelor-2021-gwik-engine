using System;
using Silk.NET.Maths;

namespace Sokoban.utilities
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
    }
}
