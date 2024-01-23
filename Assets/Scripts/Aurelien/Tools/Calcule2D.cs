using UnityEngine;
using UnityEngine.InputSystem;

namespace Aurinaxtailer
{
    public static class Rotation2D
    {
        public static Quaternion LookToDirection2D(Quaternion rotation, Vector3 target)
        {
            Vector3 diff = target;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, rot_z - 90f);
            return rotation;
        }

#if ENABLE_INPUT_SYSTEM
        public static Quaternion LookAtMouse2D(Transform objectToLookMouse)
        {
            Vector3 diff =
                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())
                - objectToLookMouse.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            objectToLookMouse.rotation = Quaternion.Euler(0f, 0f, rot_z - 90f);
            return objectToLookMouse.rotation;
        }

        public static Quaternion LookToMouse2DPerspective(Transform objectToLookMouse)
        {
            Vector2 target = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()).direction;
            Vector2 diff = target;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            objectToLookMouse.rotation = Quaternion.Euler(0f, 0f, rot_z - 90f);
            return objectToLookMouse.rotation;
        }
#endif
    }

    public static class Intersection2D
    {
        public static Vector2 GetIntersectionBetweenABandCD(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            var a = (B.y - A.y) / (B.x - A.x);
            var b = A.y - (a * A.x);

            var c = (D.y - C.y) / (D.x - C.x);
            var d = C.y - (c * C.x);

            Vector2 intersection;
            intersection.x = (b - d) * (1f / (c - a));
            intersection.y = a * intersection.x + b;
            return intersection;
        }
    }
}
