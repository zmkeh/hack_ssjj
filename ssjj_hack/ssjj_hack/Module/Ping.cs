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
    public class Ping : ModuleBase
    {
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

    }
}
