using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack
{
    public static class MathTool
    {
        public static float SignedAngle(Vector2 v1, Vector2 v2)
        {
            float angle = Vector3.Angle(v1, v2);
            angle *= Mathf.Sign(Vector3.Cross(v1, v2).y);
            return angle;
        }
    }
}
