using Assets.Sources.Components.Chat;
using Assets.Sources.Framework;
using Assets.Sources.Framework.System;
using Assets.Sources.Modules.Ui.Chat;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Fun : ModuleBase
    {
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();
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
