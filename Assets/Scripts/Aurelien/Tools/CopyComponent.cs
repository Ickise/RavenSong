using UnityEngine;

namespace Aurinaxtailer
{
    public static class CopyComponent
    {
        public static void CopyTransform(Transform transformToCopy, Transform transformToPast, bool inParent)
        {
            transformToPast.position = transformToCopy.position;
            transformToPast.rotation = transformToCopy.rotation;
            transformToPast.localScale = transformToCopy.localScale;
            transformToPast.parent = inParent ? transformToCopy.parent : null;
        }
    }
}
