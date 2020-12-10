using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Esp : ModuleBase
    {
        public override void OnGUI()
        {
            base.OnGUI();
            GizmosPro.ins.CallOnGUI();
        }

        public override void Update()
        {
            base.Update();
            UpdateDraw();
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

                if (cam.WorldToScreenPoint(c.transform.position).z <= 0)
                    continue;

                //TODO: DRAW AIR LINE

                var spine = c.FindChildDeep("Bip01_Spine");
                var neck = c.FindChildDeep("Bip01_Neck");
                var u_head = c.FindChildDeep("Bip01_HeadNub");
                var d_head = c.FindChildDeep("Bip01_Head");

                var clavicle = c.FindChildDeep("Bip01_R_Clavicle");
                var l_arm = c.FindChildDeep("Bip01_L_UpperArm");
                var r_arm = c.FindChildDeep("Bip01_R_UpperArm");
                var l_hand = c.FindChildDeep("Bip01_L_Hand");
                var r_hand = c.FindChildDeep("Bip01_R_Hand");

                var pelvis = c.FindChildDeep("Bip01_Pelvis");
                var l_thigh = c.FindChildDeep("Bip01_L_Thigh");
                var r_thigh = c.FindChildDeep("Bip01_R_Thigh");
                var l_calf = c.FindChildDeep("Bip01_L_Calf");
                var r_calf = c.FindChildDeep("Bip01_R_Calf");
                var l_foot = c.FindChildDeep("Bip01_L_Foot");
                var r_foot = c.FindChildDeep("Bip01_R_Foot");

                if (spine == null || neck == null
                    || d_head == null || u_head == null
                    || l_arm == null || r_arm == null
                    || l_hand == null || r_hand == null
                    || clavicle == null || pelvis == null
                    || l_thigh == null || r_thigh == null
                    || l_calf == null || r_calf == null
                    || l_foot == null || r_foot == null)
                    continue;

                D_C(spine, Color.green);
                D_C(neck, Color.green);
                D_C(d_head, Color.green);
                D_C(u_head, Color.green);

                D_C(clavicle, Color.green);
                D_C(l_arm, Color.blue);
                D_C(r_arm, Color.red);
                D_C(l_hand, Color.blue);
                D_C(r_hand, Color.red);

                D_C(pelvis, Color.green);
                D_C(l_thigh, Color.blue);
                D_C(r_thigh, Color.red);
                D_C(l_calf, Color.blue);
                D_C(r_calf, Color.red);
                D_C(l_foot, Color.blue);
                D_C(r_foot, Color.red);

                D_L(pelvis, spine, Color.green);
                D_L(spine, neck, Color.green);
                D_L(neck, clavicle, Color.green);
                D_L(neck, d_head, Color.green);
                D_L(d_head, u_head, Color.green);

                D_L(clavicle, l_arm, Color.blue);
                D_L(clavicle, r_arm, Color.red);
                D_L(l_arm, l_hand, Color.blue);
                D_L(r_arm, r_hand, Color.red);

                D_L(pelvis, l_thigh, Color.blue);
                D_L(pelvis, r_thigh, Color.red);
                D_L(l_thigh, l_calf, Color.blue);
                D_L(r_thigh, r_calf, Color.red);
                D_L(l_calf, l_foot, Color.blue);
                D_L(r_calf, r_foot, Color.red);
            }
        }

        private TCircle _C(Transform t)
        {
            var cam = Camera.main;
            var uipos = cam.WorldToScreenPoint(t.position);
            return new TCircle(uipos, 1);
        }

        private Vector3 _P(Transform t1)
        {
            var cam = Camera.main;
            var uipos = cam.WorldToScreenPoint(t1.position);
            return uipos;
        }

        private void D_C(Transform t, Color color)
        {
            if (_P(t).z <= 0)
                return;
            GizmosPro.DrawCircle(_C(t), color);
        }

        private void D_L(Transform t1, Transform t2, Color color)
        {
            if (_P(t1).z <= 0 || _P(t2).z <= 0)
                return;
            GizmosPro.DrawLine(_P(t1), _P(t2), color);
        }
    }
}
