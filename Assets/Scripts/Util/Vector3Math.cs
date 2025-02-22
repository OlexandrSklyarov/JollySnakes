﻿using UnityEngine;

namespace Util
{
    public static class Vector3Math
    {
        public static Vector2 Mult(this Vector2 a, Vector2 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y);
        }
        
        public static Vector3 Mult(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static bool CompareApproximately(this Vector3 a, Vector3 b, float gap)
        {
            return Vector3.SqrMagnitude(a - b) < gap;
        }

        public static Vector3 Direction(this Vector3 pos, Vector3 target)
        {
            return (target - pos).normalized;
        }

        public static float GetSqrDistanceXZ(this Vector3 myPosition, Vector3 position)
        {
            myPosition.y = 0f;
            position.y = 0f;
            return (myPosition - position).sqrMagnitude;
        }
        
        public static float GetSqrDistanceXY(this Vector3 myPosition, Vector3 position)
        {
            myPosition.z = 0f;
            position.z = 0f;
            return (myPosition - position).sqrMagnitude;
        }
        
        public static float GetSqrDistanceYZ(this Vector3 myPosition, Vector3 position)
        {
            myPosition.x = 0f;
            position.x = 0f;
            return (myPosition - position).sqrMagnitude;
        }
        
        public static float GetDistanceXZ(this Vector3 myPosition, Vector3 position)
        {
            myPosition.y = 0f;
            position.y = 0f;
            return (myPosition - position).magnitude;
        }
        
        public static float GetDistanceXY(this Vector3 myPosition, Vector3 position)
        {
            myPosition.z = 0f;
            position.z = 0f;
            return (myPosition - position).magnitude;
        }
        
        public static float GetDistanceYZ(this Vector3 myPosition, Vector3 position)
        {
            myPosition.x = 0f;
            position.x = 0f;
            return (myPosition - position).magnitude;
        }
        
        public static float GetDistanceX(this Vector3 myPosition, Vector3 position)
        {
            return Mathf.Abs((myPosition - position).x);
        }
        
        public static float GetDistanceY(this Vector3 myPosition, Vector3 position)
        {
            return Mathf.Abs((myPosition - position).y);
        }
        
        public static float GetDistanceZ(this Vector3 myPosition, Vector3 position)
        {
            return Mathf.Abs((myPosition - position).z);
        }


        public static Quaternion DirToQuaternion(Vector3 dir)
        {
            var angle = GetUpAxisAngleRotate(dir);
            return Quaternion.Euler(new Vector3(0f, angle, 0f));
        }


        public static float GetUpAxisAngleRotate(Vector3 dir)
        {
            dir.Normalize();
            return Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        }

        public static Vector3 AddDirectionSpread(this Vector3 dir, float spread)
        {
            var modifierDir =  dir + new Vector3
            (
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread)
            );

            return modifierDir.normalized;
        }
    }
}