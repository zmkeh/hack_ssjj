using System;
using UnityEngine;

namespace ssjj_hack
{
    public class Loop : MonoBehaviour
    {
        void Awake()
        {
            try
            {
                Log.Print("Loop Awake");
                DontDestroyOnLoad(this);
            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }
        }

        void OnGUI()
        {
            try
            {
                GizmosPro.ins.CallOnGUI();
            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }
        }

        void Update()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }
        }

        void LateUpdate()
        {
            try
            {
                if (Screen.width != 800 || Screen.fullScreen)
                {
                    Screen.SetResolution(800, 480, false);
                }

                UpdateDraw();
                UpdateAim();
            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }
        }


        // logic
        private Transform lockTrans = null;
        void UpdateAim()
        {
            var rootGo = GameObject.Find("thirdPersonResources");
            if (rootGo == null || Camera.main == null)
                return;
            var cam = Camera.main;

            if (Input.GetMouseButton(0))
            {
                if (lockTrans == null)
                {
                    var root = rootGo.transform;
                    for (int i = 0; i < root.childCount; i++)
                    {
                        var c = root.GetChild(i);

                        if (!c.gameObject.activeSelf)
                            continue;

                        var p = c.FindChildDeep("Bip01_Head");
                        var p2 = p.Find("Bip01_HeadNub");
                        if (p == null || p2 == null)
                            continue;

                        var uipos = cam.WorldToScreenPoint(p.position);
                        var uipos2 = cam.WorldToScreenPoint(p2.position);
                        var center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                        var headRadius = (uipos2 - uipos).magnitude;
                        if (uipos.z > 0 && new TCircle(uipos, headRadius).IsOverLapWith(center))
                        {
                            lockTrans = p2;
                            break;
                        }
                    }
                }
                else
                {
                    cam.transform.LookAt(lockTrans.position);
                }
            }
            else
            {
                lockTrans = null;
            }
        }

        void UpdateDraw()
        {
            var rootGo = GameObject.Find("thirdPersonResources");
            if (rootGo == null || Camera.main == null)
                return;
            var cam = Camera.main;
            var root = rootGo.transform;
            for (int i = 0; i < root.childCount; i++)
            {
                var c = root.GetChild(i);

                if (!c.gameObject.activeSelf)
                    continue;

                var p = c.FindChildDeep("Bip01_Head");
                var p2 = p.Find("Bip01_HeadNub");
                if (p == null || p2 == null)
                    continue;

                var uipos = cam.WorldToScreenPoint(p.position);
                var uipos2 = cam.WorldToScreenPoint(p2.position);
                var topCenter = new Vector2(Screen.width * 0.5f, Screen.height);
                var headRadius = (uipos2 - uipos).magnitude;
                if (uipos.z > 0)
                {
                    GizmosPro.DrawLine(topCenter, uipos, Color.red);
                    GizmosPro.DrawCircle(new TCircle(uipos, headRadius), Color.red);
                }
            }
        }
        private string GetTransformPath(Transform t)
        {
            if (t.parent != null)
                return GetTransformPath(t.parent) + "/" + t.name;
            return t.name;
        }
    }
}
