using Assets.Sources.Components.Chat;
using Assets.Sources.Framework;
using Assets.Sources.Framework.System;
using Assets.Sources.Modules.Ui.Chat;
using Assets.Sources.Modules.Ui.Cross;
using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Fun : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();
        private CrossContext cross = null;
        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.T))
            {
                /*
                var m = playerMgr.models.First();
                if (m != null)
                {
                    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var cube = GameObject.Instantiate(m.root.gameObject);
                    cube.transform.localScale = Vector3.one;
                    var cam = Camera.main.transform;
                    cube.transform.position = cam.position + cam.forward * 100;
                }
                */

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

        private static readonly string[] _chatType = new string[]
        {
            "battle_team",
            "battle_all",
            "team",
            "personal",
            "big_horn2"
        };

        public static void SendMessage(string msg)
        {
            var chat = GameModuleFeature.Instance.ReflectField<PlaybackSystem>("_playbackSystem")
                .ReflectField<List<IPlaybackSystem>>("_systems").First(a => a.GetType() == typeof(ChatJobSystem));
            var data = new ChatInputData();
            data.SenderInputContent = msg;
            data.SenderType = _chatType[1];
            data.ReceiverName = string.Empty;
            data.ReceiverCid = string.Empty;
            chat.ReflectInvokeMethod("SendChatInfo", new object[] { data });
        }
    }
}
