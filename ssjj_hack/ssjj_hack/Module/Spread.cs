using Assets.Sources.Framework;
using Assets.Sources.Framework.System;
using Assets.Sources.Free.Data;
using Assets.Sources.Modules.Ui.Cross;
using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Spread : ModuleBase
    {
        private CrossContext cross = null;
        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.T))
            {
                var crossSys = GameModuleFeature.Instance.ReflectField<PlaybackSystem>("_playbackSystem")
                    .ReflectField<List<IPlaybackSystem>>("_systems").First(a => a.GetType() == typeof(CrossSpreadUpdateSystem));
                cross = crossSys.ReflectField<CrossContext>("_crossContext");
            }
        }

        public override void LateUpdate()
        {
            if (cross != null)
            {
                cross.crossSpread.RendSpread = 0;
                cross.crossSpread.Scale = 0;
                cross.crossSpread.Up = 0;
                cross.crossSpread.Down = 0;
                cross.crossSpread.Left = 0;
                cross.crossSpread.Right = 0;
            }
        }
    }
}
