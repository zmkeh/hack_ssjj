using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ssjj_hack
{
    public class Viewer : ModuleBase
    {
        /*
        private ViewObj viewObj = null;
        private Vector2 scroll = Vector2.zero;

        public override void OnGUI()
        {
            base.OnGUI();

            GUI.contentColor = Color.green;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.box.alignment = TextAnchor.MiddleLeft;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;

            if (viewObj != null)
            {
                GUI.Box(new Rect(0, 0, 700, Screen.height), "");
                GUI.Box(new Rect(0, 0, 700, Screen.height), "");
            }

            if (viewObj == null && GUILayout.Button("Collect"))
            {
                viewObj = new ViewObj(Camera.main);
            }
            else if (viewObj != null && GUILayout.Button("Close"))
            {
                viewObj = null;
            }

            if (viewObj != null)
            {
                scroll = GUILayout.BeginScrollView(scroll);
                viewObj.DrawGUI();
                GUILayout.EndScrollView();
            }

            scroll += Input.mouseScrollDelta.y * Vector2.up * 0.1f;
        }
        */
    }
}
