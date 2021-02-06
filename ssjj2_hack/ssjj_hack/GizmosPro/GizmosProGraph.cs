using UnityEngine;

namespace ssjj_hack
{
    public class GizmosProGraph
    {
        public RectTransform trans;
        public bool isShow { get; private set; } = false;

        private Material s_material = null;

        public int smooth = 50;
        Color m_color = Color.white;
        enum EGraphType
        {
            None,
            Line,
            Rect,
            Circle,
            Ellipse,
        }

        EGraphType m_type = EGraphType.None;
        Vector2[] m_points;
        TRect m_rect = new TRect();
        TCircle m_circle = new TCircle();
        TEllipse m_ellipse = new TEllipse();

        public void Show(Vector2[] points, Color color)
        {
            m_type = EGraphType.Line;
            m_points = points;
            m_color = color;
            isShow = true;
        }
        public void Show(Vector2 p1, Vector2 p2, Color color)
        {
            m_type = EGraphType.Line;
            m_points = new Vector2[2];
            m_points[0] = p1;
            m_points[1] = p2;
            m_color = color;
            isShow = true;
        }

        public void Show(TCircle circle, Color color)
        {
            m_type = EGraphType.Circle;
            m_circle = circle;
            m_color = color;
            isShow = true;
        }

        public void Show(TRect rect, Color color)
        {
            m_type = EGraphType.Rect;
            m_rect = rect;
            m_color = color;
            isShow = true;
        }

        public void Show(TEllipse ellipse, Color color)
        {
            m_type = EGraphType.Ellipse;
            m_ellipse = ellipse;
            m_color = color;
            isShow = true;
        }

        public void Draw()
        {
            switch (m_type)
            {
                case EGraphType.Line:
                    DrawScreenLine(m_points, m_color);
                    break;
                case EGraphType.Rect:
                    DrawScreenRect(m_rect, m_color);
                    break;
                case EGraphType.Circle:
                    DrawScreenCircle(m_circle, m_color, smooth);
                    break;
                case EGraphType.Ellipse:
                    DrawScreenEllipse(m_ellipse, m_color);
                    break;
            }

            isShow = false;
        }

        void Begin(Color color)
        {
            if (s_material == null)
                s_material = new Material(Shader.Find("Hidden/Internal-Colored"));
            GL.PushMatrix();
            s_material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            s_material.SetColor(Shader.PropertyToID("_Color"), color);
        }

        void End()
        {
            GL.End();
            GL.PopMatrix();
        }

        public void DrawScreenLine(Vector2[] points, Color color)
        {
            Begin(color);
            for (int i = 0; i < points.Length; ++i)
            {
                GL.Vertex3(points[i].x / Screen.width, points[i].y / Screen.height, 0);
            }
            End();
        }

        public void DrawScreenRect(TRect rect, Color color)
        {
            Begin(color);
            
            GL.Vertex3(rect.left / Screen.width, rect.top / Screen.height, 0);
            GL.Vertex3(rect.left / Screen.width, rect.bottom / Screen.height, 0);

            GL.Vertex3(rect.left / Screen.width, rect.bottom / Screen.height, 0);
            GL.Vertex3(rect.right / Screen.width, rect.bottom / Screen.height, 0);

            GL.Vertex3(rect.right / Screen.width, rect.bottom / Screen.height, 0);
            GL.Vertex3(rect.right / Screen.width, rect.top / Screen.height, 0);

            GL.Vertex3(rect.right / Screen.width, rect.top / Screen.height, 0);
            GL.Vertex3(rect.left / Screen.width, rect.top / Screen.height, 0);

            End();
        }

        public void DrawScreenEllipse(Vector2 center, float xRadius, float yRadius, Color color, int smooth = 50)
        {
            Begin(color);
            for (int i = 0; i < smooth; ++i)
            {
                int nextStep = (i + 1) % smooth;
                GL.Vertex3((center.x + xRadius * Mathf.Cos(2 * Mathf.PI / smooth * i)) / Screen.width,
                    (center.y + yRadius * Mathf.Sin(2 * Mathf.PI / smooth * i)) / Screen.height, 0);
                GL.Vertex3((center.x + xRadius * Mathf.Cos(2 * Mathf.PI / smooth * nextStep)) / Screen.width,
                    (center.y + yRadius * Mathf.Sin(2 * Mathf.PI / smooth * nextStep)) / Screen.height, 0);
            }

            End();
        }

        public void DrawScreenCircle(TCircle circle, Color color, int smooth = 50)
        {
            DrawScreenEllipse(circle.center, circle.radius, circle.radius, color, smooth);
        }
        public void DrawScreenEllipse(TEllipse ellipse, Color color, int smooth = 50)
        {
            DrawScreenEllipse(ellipse.center, ellipse.xRadius, ellipse.yRadius, color, smooth);
        }
    }
}
