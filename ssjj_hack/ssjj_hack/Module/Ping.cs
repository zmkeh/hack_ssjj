using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ssjj_hack.Module
{
    // 透视
    public class Ping : ModuleBase
    {
        bool isJoin = false;
        public PlayerMgr playerMgr => Loop.GetPlugin<PlayerMgr>();

        public override void Update()
        {
            if (!isJoin && Contexts.sharedInstance != null && Contexts.sharedInstance.player != null 
                && Contexts.sharedInstance.player.myPlayerEntity != null
                && Contexts.sharedInstance.player.myPlayerEntity.basicInfo != null)
            {
                var info = Contexts.sharedInstance.player.myPlayerEntity.basicInfo;
                var _name = info.PlayerName;
                var _date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SendQQMsg($"{_date}:【{_name}】 加入了游戏！");
                isJoin = true;
            }
        }

        const string host = "http://39.105.150.229:8748/send_group_msg";

        public void SendQQMsg(string msg)
        {
            PostWebRequest(host, "{\"group_id\": 957235191,\"message\": \"" + msg + "\"}");
        }

        private void PostWebRequest(string url, string postData)
        {
            Loop.Instance.StartCoroutine(PostUrl(url, postData));
        }

        IEnumerator PostUrl(string url, string postData)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))//第二种写法此处注释
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(postData);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                yield return webRequest.Send();
            }
        }
    }
}
