using UnityEngine;

namespace DefaultNamespace
{
    public class Constraints
    {
        public const float Epsilon = 0.001f;
        public const float MaxAngle = 360f;
        
        public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -MaxAngle) lfAngle += MaxAngle;
            if (lfAngle > MaxAngle) lfAngle -= MaxAngle;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}