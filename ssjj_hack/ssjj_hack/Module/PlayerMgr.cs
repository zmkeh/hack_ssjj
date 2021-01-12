using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class PlayerMgr : ModuleBase
    {
        public List<PlayerModel> models = new List<PlayerModel>();
        public static Camera camera = null;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            camera = Camera.main;
            Collet();
        }

        public void Collet()
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

                if (models.Any(a => a.root == c))
                    continue;

                models.Add(new PlayerModel(c));
            }
        }
    }

    public class PlayerModel
    {
        public Transform spine;
        public Transform neck;
        public Transform u_head;
        public Transform d_head;

        public Transform clavicle;
        public Transform l_upperarm;
        public Transform r_upperarm;
        public Transform l_forearm;
        public Transform r_forearm;
        public Transform l_hand;
        public Transform r_hand;

        public Transform pelvis;
        public Transform l_thigh;
        public Transform r_thigh;
        public Transform l_calf;
        public Transform r_calf;
        public Transform l_foot;
        public Transform r_foot;

        public Transform root;

        public bool isCached = false;

        public PlayerModel(Transform root)
        {
            this.root = root;
            Log.Print(" ------------------------------------------------ ");
            foreach (var t in root.GetComponentsInChildren<Transform>())
            {
                Log.Print(t.GetPath());
            }
            Log.Print(" ------------------------------------------------ ");
            CacheBones();
        }

        public void CacheBones()
        {
            var c = root;
            spine = c.FindChildDeep("Bip01_Spine");
            neck = c.FindChildDeep("Bip01_Neck");
            u_head = c.FindChildDeep("Bip01_HeadNub");
            d_head = c.FindChildDeep("Bip01_Head");

            clavicle = c.FindChildDeep("Bip01_R_Clavicle");
            l_upperarm = c.FindChildDeep("Bip01_L_UpperArm");
            r_upperarm = c.FindChildDeep("Bip01_R_UpperArm");
            l_forearm = c.FindChildDeep("Bip01_L_Forearm");
            r_forearm = c.FindChildDeep("Bip01_R_Forearm");
            l_hand = c.FindChildDeep("Bip01_L_Hand");
            r_hand = c.FindChildDeep("Bip01_R_Hand");

            pelvis = c.FindChildDeep("Bip01_Pelvis");
            l_thigh = c.FindChildDeep("Bip01_L_Thigh");
            r_thigh = c.FindChildDeep("Bip01_R_Thigh");
            l_calf = c.FindChildDeep("Bip01_L_Calf");
            r_calf = c.FindChildDeep("Bip01_R_Calf");
            l_foot = c.FindChildDeep("Bip01_L_Foot");
            r_foot = c.FindChildDeep("Bip01_R_Foot");


            if (spine == null || neck == null
                || d_head == null || u_head == null
                || l_upperarm == null || r_upperarm == null
                || l_forearm == null || r_forearm == null
                || l_hand == null || r_hand == null
                || clavicle == null || pelvis == null
                || l_thigh == null || r_thigh == null
                || l_calf == null || r_calf == null
                || l_foot == null || r_foot == null)
                isCached = false;
            isCached = true;
        }

        private Vector3 _P(Vector3 v)
        {
            var cam = PlayerMgr.camera;
            var uipos = cam.WorldToScreenPoint(v);
            return uipos;
        }

        private Vector3 _P(Transform t)
        {
            return _P(t.position);
        }

        private void AddPoint(List<TCircle> lst, Transform t)
        {
            var p = _P(t);
            if (p.z <= 0)
                return;
            lst.Add(new TCircle(p, 1));
        }

        public float GetHeight()
        {
            var p1 = _P(root);
            var p2 = _P(u_head);
            return p2.y - p1.y;
        }

        public TRect GetRect()
        {
            var p1 = _P(root.position);
            var p2 = _P(u_head.position);
            var x = (p1.x + p2.x) * 0.5f;
            var y = (p1.y + p2.y) * 0.5f;
            var h = p2.y - p1.y;
            var w = h * 0.4f;
            if (p1.z <= 0 || p2.z <= 0)
                return default;
            return new TRect(x, y, w, h);
        }

        public List<TCircle> GetPoints()
        {
            var points = new List<TCircle>();
            AddPoint(points, spine);
            AddPoint(points, neck);
            AddPoint(points, d_head);
            AddPoint(points, u_head);
            AddPoint(points, clavicle);
            AddPoint(points, l_upperarm);
            AddPoint(points, r_upperarm);
            AddPoint(points, r_forearm);
            AddPoint(points, r_forearm);
            AddPoint(points, l_hand);
            AddPoint(points, r_hand);
            AddPoint(points, pelvis);
            AddPoint(points, l_thigh);
            AddPoint(points, r_thigh);
            AddPoint(points, l_calf);
            AddPoint(points, r_calf);
            AddPoint(points, l_foot);
            AddPoint(points, r_foot);
            return points;
        }

        private void AddLine(List<TLine> lst, Transform t1, Transform t2)
        {
            var p1 = _P(t1);
            var p2 = _P(t2);
            if (p1.z <= 0 || p2.z <= 0)
                return;
            lst.Add(new TLine(p1, p2));
        }

        public List<TLine> GetLines()
        {
            var lines = new List<TLine>();
            AddLine(lines, pelvis, spine);
            AddLine(lines, spine, neck);
            AddLine(lines, neck, clavicle);
            AddLine(lines, neck, d_head);
            AddLine(lines, d_head, u_head);

            AddLine(lines, clavicle, l_upperarm);
            AddLine(lines, clavicle, r_upperarm);
            AddLine(lines, l_upperarm, l_forearm);
            AddLine(lines, r_upperarm, r_forearm);
            AddLine(lines, l_forearm, l_hand);
            AddLine(lines, r_forearm, r_hand);

            AddLine(lines, pelvis, l_thigh);
            AddLine(lines, pelvis, r_thigh);
            AddLine(lines, l_thigh, l_calf);
            AddLine(lines, r_thigh, r_calf);
            AddLine(lines, l_calf, l_foot);
            AddLine(lines, r_calf, r_foot);
            return lines;
        }
    }
}