using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void DestroyChildren(this GameObject gameObject, bool includeInactive = true)
        {
            gameObject.transform.DestroyChildren(includeInactive);
        }
    }
}