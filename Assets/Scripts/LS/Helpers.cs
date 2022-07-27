using UnityEngine;

namespace LS
{

    public class Helpers
    {
        public static GameObject FindParentWithTag(GameObject gameObject, string tag)
        {
            Transform transform = gameObject.transform;
            while (transform.parent != null)
            {
                if (transform.parent.tag == tag)
                {
                    return transform.parent.gameObject;
                }
                transform = transform.parent.transform;
            }
            return null;
        }

        public static T GetComponentInParentWithTag<T>(GameObject gameObject, string tag)
        {
            GameObject parent = FindParentWithTag(gameObject, tag);
            if (parent)
            {
                return parent.GetComponent<T>();
            }
            return default(T);
        }
    }
}