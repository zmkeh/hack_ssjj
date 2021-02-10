using Assets.Scripts.Input;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class Aim : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();

        public override void Start()
        {
            InputCollector.Instance.SetDeviceInput(new FakeUnityInput());
        }

        public override void Update()
        {
            if (!Settings.isOn)
                return;

            if (!Settings.isAim)
                return;
            base.Update();
            UpdateAim();
        }

        // logic
        private float dragDelay = 0;
        private float fireKeep = 0;
        void UpdateAim()
        {
            var targetPoint = Vector2.zero;
            var minDist = float.MaxValue;
            var center = new Vector2(Screen.width, Screen.height) * 0.5f;
            foreach (var p in playerMgr.models)
            {
                if (!p.isCached)
                    continue;
                if (!p.root)
                    continue;
                if (!p.isAlive)
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
                    targetPoint = point;
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
                    var l = new TLine(targetPoint, center);
                    GizmosPro.DrawLine(l.from, l.to, Color.gray);
                }

                var delta = targetPoint - center;
                dragDelay = Mathf.Max(0, dragDelay - Time.deltaTime);
                fireKeep = Mathf.Max(0, fireKeep - Time.deltaTime);
                var isMove = false;

                if (Input.GetMouseButton(0))
                {
                    fireKeep = 0.02f;
                }

                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    dragDelay = 0.02f;
                }

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    isMove = true;
                }

                if (Input.GetKey(KeyCode.LeftShift) || fireKeep > 0 
                    || !isMove && dragDelay <= 0)
                {
                    FakeUnityInput.forceAxis += delta * 0.2f;
                }
            }
        }
    }
}
