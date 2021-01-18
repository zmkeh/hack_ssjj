using UnityEngine;
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

        /*
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
        }*/

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

        public static string GetPath(this Transform transform)
        {
            if (transform.parent != null)
                return transform.parent.GetPath() + "/" + transform.name;
            return transform.name;
        }

        public static Vector3 GetUIPos(this Transform tranform)
        {
            if (tranform == null)
                return Vector3.zero;
            return tranform.position.ToUIPos();
        }

        public static Vector3 ToUIPos(this Vector3 vector3)
        {
            var cam = camera;
            if (cam == null)
                return Vector3.zero;
            var uipos = cam.WorldToScreenPoint(vector3);
            return uipos;
        }

        private static Camera _mainCamera;
        private static float _lastGetTime;
        public static Camera camera
        {
            get
            {
                if (_mainCamera == null || Time.time - _lastGetTime <= 0.5f)
                {
                    _mainCamera = Camera.main;
                    _lastGetTime = Time.time;
                }
                return _mainCamera;
            }
        }


        //MATH UTILITY
        public static Vector3 GetNearstPointInLine(Vector3 start, Vector3 end, Vector3 targetPoint, out float factor, int needExtend = -1)
        {
            Vector3 _delta = end - start;
            if (_delta == Vector3.zero)
            {
                factor = 0;
                return start;
            }
            factor = (Vector3.Dot(targetPoint, _delta) - (Vector3.Dot(start, _delta))) / (_delta.sqrMagnitude);
            if ((factor >= 0 && factor <= 1) || needExtend == 2)
            {
                return start + _delta * factor;
            }
            else
            {
                if (needExtend == -1)
                {
                    if (factor < 0) return start;
                    if (factor > 1) return end;
                }
                else if (needExtend == 0)
                {
                    if (factor < 0) return start + _delta * factor;
                    if (factor > 1) return end;
                }
                else if (needExtend == 1)
                {
                    if (factor < 0) return start;
                    if (factor > 1) return start + _delta * factor;
                }
            }
            return Vector3.zero;
        }
    }
}
