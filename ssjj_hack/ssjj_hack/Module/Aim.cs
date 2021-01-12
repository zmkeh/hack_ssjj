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
            if (!Settings.isAim)
                return;
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
                if (!Settings.isEspFriendly && p.isFriend)
                    continue;

                Vector2 point;
                if (Settings.aimPos == AimPos.HEAD)
                {
                    var p1 = p.u_head.GetUIPos();
                    var p2 = p.d_head.GetUIPos();
                    if (p1.z <= 0 || p2.z <= 0)
                        continue;
                    point = (p1 + p2) * 0.5f;
                }
                else if (Settings.aimPos == AimPos.CHEST)
                {
                    var p3 = p.clavicle.GetUIPos();
                    if (p3.z <= 0)
                        continue;
                    point = p3;
                }
                else
                {
                    continue;
                }

                var dist = Vector2.Distance(point, center);
                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = point;
                }
            }


            var range = Screen.height * 0.05f * Settings.aimRange;
            if (Settings.isAimCircle)
            {
                var c = new TCircle(center, range);
                GizmosPro.DrawCircle(c, Color.gray);
            }

            if (minDist <= range)
            {
                if (Settings.isAimLine)
                {
                    var l = new TLine(minPoint, center);
                    GizmosPro.DrawLine(l.from, l.to, Color.gray);
                }

                if (Input.GetMouseButton(0))
                {
                    var delta = minPoint - center;
                    Sim.Move(delta);
                }
            }
        }

        void UpdateAccuracy()
        {
            /*
            Contexts.sharedInstance.player.myPlayerEntity.currentWeapon.WeaponInfo.AccuracyOffset = 0;
            Contexts.sharedInstance.player.myPlayerEntity.currentWeapon.WeaponInfo.DefaultAccuracy = 0;
            Contexts.sharedInstance.player.myPlayerEntity.currentWeapon.WeaponInfo.MaxInaccuracy = 100;
            Contexts.sharedInstance.player.myPlayerEntity.currentWeapon.WeaponInfo.SpreadScaleY = 0;
            */
        }





















    }
}
