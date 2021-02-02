using Assets.Sources.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ssjj_hack
{
    public static class MathTool
    {
        public static float PitchToScreen(float angle)
        {
            // y = x * 2 * rev * (0.01f + 0.001f * sen) * (fov / 90f)
            var rev = GameSetting.Config.MouseReversal == 1 ? -1 : 1;
            var sen = GameSetting.Config.MouseSensitivity;
            var fov = Contexts.sharedInstance.player.cameraOwnerEntity.fov.Fov;
            var x = angle / (2f * rev * (0.01f + 0.001f * sen) * (fov / 90f));
            return x;
        }

        public static float YawToScreen(float angle)
        {
            // y = x * -2 * (0.01f + 0.001f * sen) * (fov / 90f)
            var sen = GameSetting.Config.MouseSensitivity;
            var fov = Contexts.sharedInstance.player.cameraOwnerEntity.fov.Fov;
            var x = angle / (-2f * (0.01f + 0.001f * sen) * (fov / 90f));
            return x;
        }
    }
}
