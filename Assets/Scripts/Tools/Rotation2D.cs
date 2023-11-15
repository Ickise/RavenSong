using UnityEngine;
using UnityEngine.InputSystem;

namespace Aurinaxtailer
{
    public static class Rotation2D
    {
        public static Quaternion LookAt2D(Quaternion rotation, Vector3 target)
        {
            Vector3 diff = target;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
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
            objectToLookMouse.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            return objectToLookMouse.rotation;
        }
#endif
    }
}
