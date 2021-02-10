using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Esp : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();

        public override void OnGUI()
        {
            if (!Settings.isOn)
                return;
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

                var color = p.isFriend ? Color.green : Color.red;

                // NAME
                var _name = p.root.name;
                Color lastColor = GUI.color;
                TextAnchor lastAnchor = GUI.skin.label.alignment;
                GUI.contentColor = Color.black;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                var _rect = p.GetRect();
                _rect.y = Screen.height - _rect.y;
                var __rect = new Rect(_rect.center, _rect.size);
                __rect.y -= __rect.height;
                __rect.x = __rect.x - 100;
                __rect.height = 20;
                __rect.width = 200;
                GUI.Label(__rect, _name);
                __rect.x -= 1;
                __rect.y -= 1;
                GUI.contentColor = color;
                GUI.Label(__rect, _name);
                GUI.contentColor = lastColor;
                GUI.skin.label.alignment = lastAnchor;

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
