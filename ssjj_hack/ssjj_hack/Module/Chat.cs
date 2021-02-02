using Assets.Sources.Components.Chat;
using Assets.Sources.Framework;
using Assets.Sources.Framework.System;
using Assets.Sources.Modules.Ui.Chat;
using System.Collections.Generic;
using UnityEngine;

namespace ssjj_hack.Module
{
    // 透视
    public class Chat : ModuleBase
    {
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

        private float interval = 300;
        private float cd = 0;

        public override void Start()
        {
            cd = 10;
        }

        public override void Update()
        {
            cd = Mathf.Max(0, cd - Time.deltaTime);
            if (cd <= 0)
            {
                // Log.Print("哈哈哈");
                // SendMessage("哈哈哈");
                cd = interval;
            }

            if (Input.GetKeyUp(KeyCode.T))
            { 
                Log.Print("ttt");
                SendMessage("ttt");
            }
        }
    }
}
