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
                if (!p.isAlive)
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

                var color = p.isFriend ? Color.green : Color.red;

                if (Settings.isEspBox)
                {
                    var rect = p.GetRect();
                    // Log.Print(p.name + " : " + rect);
                    if (rect.height != 0)
                    {
                        D_R(p.GetRect(), color);
                    }
                }

                if (Settings.isEspBoneLine)
                {
                    foreach (var line in p.GetLines())
                    {
                        D_L(line, color);
                    }
                }

                if (Settings.isEspAirLine)
                {
                    var rect = p.GetRect();
                    if (rect.height != 0)
                    {
                        var boxTopCenter = new Vector2(rect.x, rect.top);
                        var screenTopCenter = new Vector2(Screen.width * 0.5f, Screen.height);
                        D_L(new TLine(boxTopCenter, screenTopCenter), color);
                    }
                }

                if (Settings.isEspHp)
                {
                    var rect = p.GetRect();
                    if (rect.height != 0)
                    {
                        var x = rect.right + 4;
                        var y = rect.bottom + 1;
                        var h = (rect.height - 2) * p.hp / p.hpMax;
                        var p1 = new Vector2(x, y);
                        var p2 = new Vector2(x, y + h);
                        D_L(new TLine(p1, p2), color);
                        p1.x += 1; p2.x += 1;
                        D_L(new TLine(p1, p2), color);
                        p1.x += 1; p2.x += 1;
                        D_L(new TLine(p1, p2), color);
                        var bgRect = new TRect(x + 1, rect.center.y, 5, rect.height);
                        D_R(bgRect, Color.black * 0.7f);
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
