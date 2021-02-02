using Assets.Scripts.Input;
using UnityEngine;

namespace ssjj_hack
{
    public class FakeUnityInput : IDeviceInput
    {
        public bool AnyKey()
        {
            return Input.anyKey;
        }

        public bool AnyKeyDown()
        {
            return Input.anyKeyDown;
        }

        public bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        public static Vector2 forceAxis = Vector2.zero;

        public float GetAxis(string axis)
        {
            if (axis == "Mouse X")
            {
                var x = forceAxis.x;
                forceAxis.x = 0;
                return Input.GetAxis(axis) + x;
            }
            if (axis == "Mouse Y")
            {
                var y = forceAxis.y;
                forceAxis.y = 0;
                return Input.GetAxis(axis) + y;
            }
            return Input.GetAxis(axis);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        public bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        public bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }
    }

}
