using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Esp : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();

        public override void OnGUI()
        {
            base.OnGUI();
            GizmosPro.ins.CallOnGUI();
        }

        public override void Update()
        {
            if (!Settings.isEsp)
                return;
            base.Update();
            UpdateDraw();
        }

        void UpdateDraw()
        {
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

                /*
                foreach (var point in p.GetPoints())
                {
                    D_C(point, Color.black);
                }
                */

                if (Settings.isEspBox)
                {
                    var rect = p.GetRect();
                    if (rect.height != 0)
                    {
                        D_R(p.GetRect(), Color.green);
                    }
                }

                if (Settings.isEspBoneLine)
                {
                    foreach (var line in p.GetLines())
                    {
                        D_L(line, Color.green);
                    }
                }

                if (Settings.isEspAirLine)
                {
                    var rect = p.GetRect();
                    if (rect.height != 0)
                    {
                        var boxTopCenter = new Vector2(rect.x, rect.top);
                        var screenTopCenter = new Vector2(Screen.width * 0.5f, Screen.height);
                        D_L(new TLine(boxTopCenter, screenTopCenter), Color.red);
                    }
                }
            }
        }

        private void D_C(TCircle c, Color color)
        {
            GizmosPro.DrawCircle(c, color);
        }

        private void D_L(TLine l, Color color)
        {
            GizmosPro.DrawLine(l.from, l.to, color);
        }

        private void D_R(TRect r, Color color)
        {
            GizmosPro.DrawRect(r, color);
        }
    }
}
