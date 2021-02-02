using Assets.Scripts.Input;
using Assets.Sources.Components.Chat;
using Assets.Sources.Config;
using Assets.Sources.Framework;
using Assets.Sources.Framework.System;
using Assets.Sources.Free.Data;
using Assets.Sources.Modules.Player.Orientation;
using Assets.Sources.Modules.Ui.Chat;
using Assets.Sources.Modules.Ui.Cross;
using Entitas;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ssjj_hack.Module
{
    public class Fun : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();
        public override void Update()
        {
            base.Update();

            if (GameModelLocator.GetInstance() == null)
                return;

            // Watcher.Record("Camera Shake X: ", GameModelLocator.GetInstance().GameModel.ShakeAngleOffect.x);
            // Watcher.Record("Camera Shake Y: ", GameModelLocator.GetInstance().GameModel.ShakeAngleOffect.y);
            Watcher.Record("Models: ", playerMgr.models.Count);
        }
    }
}
