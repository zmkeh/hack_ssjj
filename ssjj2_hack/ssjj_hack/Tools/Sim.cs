using UnityEngine;

namespace ssjj_hack
{
    public static class Sim
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;

        public static void Move(Vector2 delta)
        {
            mouse_event(MOUSEEVENTF_MOVE, (int)delta.x, (int)-delta.y, 0, 0);
        }
    }
}
