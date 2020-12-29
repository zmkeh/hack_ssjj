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

                foreach (var point in p.GetPoints())
                {
                    D_C(point, Color.black);
                }

                foreach (var line in p.GetLines())
                {
                    D_L(line, Color.green);
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
    }
}
