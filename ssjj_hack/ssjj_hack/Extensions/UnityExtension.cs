using UnityEngine;
using UnityEngine.UI;
namespace ssjj_hack
{
    public static class UnityExtension
    {
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var comp = obj.GetComponent<T>();
            if (comp == null)
                comp = obj.AddComponent<T>();
            return comp;
        }

        public static T GetOrAddComponent<T>(this Component obj) where T : Component
        {
            var comp = obj.GetComponent<T>();
            if (comp == null)
                comp = obj.gameObject.AddComponent<T>();
            return comp;
        }

        public static void SetAlpha(this Graphic gra, float alpha)
        {
            var c = gra.color;
            c.a = alpha;
            gra.color = c;
        }

        public static void SetSprite(this Image gra, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                gra.sprite = null;
                return;
            }
            //var sprite = ResUtil.Load<Sprite>(name);
            //gra.sprite = sprite;
        }

        //Breadth-first search
        public static Transform FindChildDeep(this Transform aParent, string aName)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindChildDeep(aName);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
