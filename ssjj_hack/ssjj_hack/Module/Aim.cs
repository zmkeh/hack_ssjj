using UnityEngine;

namespace ssjj_hack.Module
{
    public class Aim : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();

            UpdateAim();
        }

        // logic
        void UpdateAim()
        {
            var minPoint = Vector2.zero;
            var minDist = float.MaxValue;
            var center = new Vector2(Screen.width, Screen.height) * 0.5f;
            foreach (var p in playerMgr.models)
            {
                if (!p.isCached)
                    continue;
                if (!p.root)
                    continue;
                if (!p.root.gameObject.activeInHierarchy)
                    continue;
                var points = p.GetPoints();
                if (points.Count <= 3)
                    continue;
                var point = (points[2].center + points[3].center) / 2;
                var dist = Vector2.Distance(point, center);
                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = point;
                }
            }

            if (minDist <= Screen.width * 0.1f)
            {
                var l = new TLine(minPoint, center);
                GizmosPro.DrawLine(l.from, l.to, Color.red);

                if (Input.GetMouseButton(0))
                {
                    var delta = minPoint - center;
                    Sim.Move(delta);
                }
            }

        }

        void UpdateAccuracy()
        {
        }





















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
