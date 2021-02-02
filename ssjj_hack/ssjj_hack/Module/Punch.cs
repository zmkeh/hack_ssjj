using Assets.Scripts.Input;
using Assets.Sources.Framework;
using Assets.Sources.Framework.System;
using Assets.Sources.Modules.Player.Orientation;
using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Punch : ModuleBase
    {
        private bool isNoRecoil = false;
        private Vector2 lastPunch = Vector2.zero;

        public override void Update()
        {
            if (Contexts.sharedInstance == null
                  || Contexts.sharedInstance.player == null
                  || Contexts.sharedInstance.player.cameraOwnerEntity == null
                  || Contexts.sharedInstance.player.cameraOwnerEntity.punchOrientation == null
                  || Contexts.sharedInstance.player.cameraOwnerEntity.punchSmooth == null)
                return;

            if (Settings.isNoRecoil && !isNoRecoil)
            {
                SetPunch(true);
            }
            else if (!Settings.isNoRecoil && isNoRecoil)
            {
                SetPunch(false);
            }

            if (!isNoRecoil)
            {
                return;
            }

            var entity = Contexts.sharedInstance.player.cameraOwnerEntity;
            var yaw = entity.punchOrientation.PunchYaw;
            var pitch = entity.punchOrientation.PunchPitch;
            var punch = new Vector2(yaw, pitch) * 2;

            if (lastPunch != punch)
            {
                var delta = lastPunch - punch;
                var _yaw = MathTool.YawToScreen(delta.x);
                var _pitch = MathTool.PitchToScreen(delta.y);
                FakeUnityInput.forceAxis += new Vector2(_yaw, _pitch);
            }

            lastPunch = punch;
        }

        private void SetPunch(bool isNoRecoil)
        {
            this.isNoRecoil = isNoRecoil;
            lastPunch = Vector2.zero;

            var sys = GameModuleFeature.Instance.ReflectField<AfterPredicationSystem>("_afterPredicationSystem")
                .ReflectField<List<IAfterPredicationSystem>>("_systems");
            var index = sys.FindIndex(a => a.GetType().Name == typeof(PunchSmoothSystem).Name);
            if (index < 0)
            {
                sys = GameModuleFeature.Instance.ReflectField<AfterPredicationSystem>("_afterPredicationSystem")
                        .ReflectField<List<IAfterPredicationSystem>>("_systems");
                index = sys.FindIndex(a => a.GetType().Name == typeof(FakePunchSmoothSystem).Name);
            }

            if (index < 0)
            {
                Log.Print("Can't find punch system.");
                return;
            }

            if (isNoRecoil)
            {
                sys[index] = new FakePunchSmoothSystem(Contexts.sharedInstance);
            }
            else
            {
                sys[index] = new PunchSmoothSystem(Contexts.sharedInstance);
            }
        }

        public override void OnDestroy()
        {
            SetPunch(false);
        }
    }
}
