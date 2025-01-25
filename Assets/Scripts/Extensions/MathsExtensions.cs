using System;
using UnityEngine;

namespace Extensions
{
    public static class MathsExtensions
    {
        public static float DistanceBetween01UnClamped(float current, float target)
        {
            var distance = Mathf.Abs(current - target);
            return distance > 0.5f ? 1f - distance : distance;
        }
    
        public static double ToAngle(this Vector2 vector)
        {
            return Math.Atan2(vector.y, vector.x);
        }
    
        public static Vector2 AngleToVec(this double angle)
        {
            return new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            );
        }
    }
}