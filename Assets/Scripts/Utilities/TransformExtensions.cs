using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform, bool includeInactive = true)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (transform.GetChild(i).gameObject.activeSelf || includeInactive)
                {
                    Object.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
    }
}