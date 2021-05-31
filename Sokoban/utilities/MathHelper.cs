using System;
using Silk.NET.Maths;

namespace Sokoban.utilities
{
    public static class MathHelper
    {
        public static float DegreesToRadians(float degrees) { return MathF.PI / 180f * degrees; }

        public static float RadiansToDegrees(float radians) { return radians * (180f / MathF.PI); }
    }

    internal static class QuaternionExtensions
    {
        public static Vector3D<float> ToRollPitchYaw(this Quaternion<float> quaternion)
        {
            var x = quaternion.X;
            var y = quaternion.Y;
            var z = quaternion.Z;
            var w = quaternion.W;
            var roll = MathHelper.RadiansToDegrees(MathF.Atan2(2.0f * (x * y + w * z), w * w + x * x - y * y - z * z));
            var pitch = MathHelper.RadiansToDegrees(MathF.Asin(-2.0f * (x * z - w * y)));
            var yaw = MathHelper.RadiansToDegrees(MathF.Atan2(2.0f * (y * z + w * x), w * w - x * x - y * y + z * z));
            return new Vector3D<float>(roll, pitch, yaw);
        }
    }
}