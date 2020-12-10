using System;
using UnityEngine;

namespace ssjj_hack
{
    public class Main
    {
        static GameObject globalGo;

        public static void Start()
        {
            try
            {
                Log.Print("[HACK START]");
                globalGo = new GameObject("HACK");
                globalGo.AddComponent<Loop>();
            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }
        }

        public static void Destroy()
        {
            try
            {
                Log.Print("[HACK DESTROY]");
                globalGo.DestroyImmediate();
            }
            catch (Exception ex)
            {
                Log.Print(ex);
            }
        }
    }
}
