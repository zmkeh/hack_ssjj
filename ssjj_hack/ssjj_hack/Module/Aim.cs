using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class Aim : ModuleBase
    {
        public override void Update()
        {
            base.Update();

            UpdateAim();
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
    }
}
