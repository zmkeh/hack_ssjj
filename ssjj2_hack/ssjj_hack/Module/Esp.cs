using UnityEngine;

namespace ssjj_hack
{
    // 透视
    public class Esp : ModuleBase
    {
        public PlayerCollector collector => Loop.GetPlugin<PlayerCollector>();

        public override void OnGUI()
        {
            if (!Settings.isEsp)
                return;
            base.Update();
            UpdateDraw();
        }

        void UpdateDraw()
        {
            foreach (var player in collector.players)
            {
                var model = player.model;
                if (!player.isValid)
                    continue;
                // if (player.isMainPlayer)
                //     continue;
                if (!Settings.isEspFriendly && player.isFriend)
                    continue;
                if (!model.root.gameObject.activeInHierarchy)
                    continue;

                var rect = model.GetRect();
                var color = player.isFriend ? Color.green : Color.red;

                // NAME
                var tempRect = rect;
                tempRect.y = Screen.height - tempRect.y;
                var nameRect = new Rect(tempRect.center, tempRect.size);
                nameRect.y -= nameRect.height * 0.5f + 30;
                nameRect.x = nameRect.x - 100;
                nameRect.height = 20;
                nameRect.width = 200;
                Color contentColor = GUI.contentColor;
                TextAnchor labelAlignment = GUI.skin.label.alignment;
                GUI.contentColor = Color.black;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.Label(nameRect, player.name);
                nameRect.position -= Vector2.one;
                GUI.contentColor = color;
                GUI.Label(nameRect, player.name);
                GUI.contentColor = contentColor;
                GUI.skin.label.alignment = labelAlignment;

                if (Settings.isEspBox)
                {
                    if (rect.height != 0)
                    {
                        D_R(model.GetRect(), color);
                    }
                }

                if (Settings.isEspBoneLine)
                {
                    foreach (var line in model.GetLines())
                    {
                        D_L(line, color);
                    }
                }

                if (Settings.isEspAirLine)
                {
                    if (rect.height != 0)
                    {
                        var boxTopCenter = new Vector2(rect.x, rect.top);
                        var screenTopCenter = new Vector2(Screen.width * 0.5f, Screen.height);
                        D_L(new TLine(boxTopCenter, screenTopCenter), color);
                    }
                }

                if (Settings.isEspHp)
                {
                    if (rect.height != 0)
                    {
                        var x = rect.right + 4;
                        var y = rect.bottom + 1;
                        var h = 1f * (rect.height - 2) * player.hpRatio;
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
