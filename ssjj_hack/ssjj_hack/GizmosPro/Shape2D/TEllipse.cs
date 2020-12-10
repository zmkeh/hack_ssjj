﻿using UnityEngine;
namespace ssjj_hack
{
    /// <summary>
    /// 椭圆i
    /// </summary>
    public struct TEllipse
    {
        /// <summary>
        /// Center X
        /// </summary>
        public float x;
        /// <summary>
        /// Center Y
        /// </summary>
        public float y;

        public float width;
        public float height;

        public Vector2 center
        {
            get { return new Vector2(x, y); }
            set { x = value.x; y = value.y; }
        }

        public Vector2 size
        {
            get { return new Vector2(width, height); }
            set { width = value.x; height = value.y; }
        }

        public float left
        {
            get { return x - width * 0.5f; }
        }
        public float right
        {
            get { return x + width * 0.5f; }
        }
        public float top
        {
            get { return y + height * 0.5f; }
        }
        public float bottom
        {
            get { return y - height * 0.5f; }
        }

        /// <summary>
        /// 外接矩形
        /// </summary>
        public TRect boundingRect
        {
            get { return new TRect(center, size); }
        }

        /// <summary>
        /// 横向椭圆
        /// </summary>
        public bool IsHorizontal
        {
            get { return width > height; }
        }

        /// <summary>
        /// 表面积
        /// </summary>
        public float surfaceArea
        {
            get { return Mathf.PI * a * b; }
        }

        /// <summary>
        /// 椭圆周长(L)的精确计算要用到积分或无穷级数的求和.如
        /// L = ∫[0, π/2]4a* sqrt(1-(e* cost)^2)dt≈2π√((a^2+b^2)/2)
        /// </summary>
        public float perimeter2
        {
            get { return Mathf.PI * 2 * Mathf.Sqrt((a * a + b * b) * 0.5f); }
        }

        /// <summary>
        /// 周长 l=2πb+4(a-b)
        /// </summary>
        public float perimeter
        {
            get { return Mathf.PI * 2 * b + 4 * (a - b); }
        }

        public float xRadius
        {
            get { return 0.5f * width; }
        }

        public float yRadius
        {
            get { return 0.5f * height; }
        }
        /// <summary>
        /// 长半轴
        /// </summary>
        public float a
        {
            get { return 0.5f * (IsHorizontal ? width : height); }
        }
        /// <summary>
        /// 长半轴
        /// </summary>
        public float b
        {
            get { return 0.5f * (IsHorizontal ? height : width); }
        }

        /// <summary>
        /// 焦距到中心的距离
        /// </summary>
        public float c
        {
            get { return Mathf.Sqrt(a * a - b * b); }
        }

        /// <summary>
        /// 离心率
        /// </summary>
        public float e
        {
            get { return c / a; }
        }

        /// <summary>
        /// 焦点1
        /// </summary>
        public Vector2 f1
        {
            get { return center - (IsHorizontal ? Vector2.right : Vector2.up) * c; }
        }

        /// <summary>
        /// 焦点2
        /// </summary>
        public Vector2 f2
        {
            get { return center + (IsHorizontal ? Vector2.right : Vector2.up) * c; }
        }

        public TEllipse(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
        }

        public TEllipse(Vector2 centerPos, Vector2 size) : this(centerPos.x, centerPos.y, size.x, size.y)
        {

        }

        public TEllipse(TRect boundingRect) : this(boundingRect.center, boundingRect.size)
        {

        }

        public TEllipse(TCircle circle) : this(circle.center.x, circle.center.y, circle.radius, circle.radius)
        {

        }

        public bool IsOverLapWith(TEllipse ellipse)
        {
            Vector2 s1 = this.size;
            Vector2 s2 = ellipse.size;
            Vector2 p1 = this.center;
            Vector2 p2 = ellipse.center;

            s1.x = Mathf.Max(0.001f, s1.x);
            s1.y = Mathf.Max(0.001f, s1.y);
            s2.x = Mathf.Max(0.001f, s2.x);
            s2.y = Mathf.Max(0.001f, s2.y);

            s1 *= 0.5f;
            s2 *= 0.5f;

            float ratio = s2.x / s2.y;
            s2.y = s2.x;
            p2.y *= ratio;
            s1.y *= ratio;
            p1.y *= ratio;

            s1.x += s2.x;
            s1.y += s2.x;

            float a = 0;
            float b = 0;
            float c = 0;
            float standDist = 0;
            Vector2 c0 = Vector2.zero;
            Vector2 c1 = Vector2.zero;
            if (s1.x > s1.y)
            {
                a = s1.x;
                b = s1.y;
                c = Mathf.Sqrt(a * a - b * b);
                standDist = 2 * a;
                c0 = new Vector2(p1.x - c, p1.y);
                c1 = new Vector2(p1.x + c, p1.y);
            }
            else
            {
                a = s1.y;
                b = s1.x;
                c = Mathf.Sqrt(a * a - b * b);
                standDist = 2 * a;
                c0 = new Vector2(p1.x, p1.y - c);
                c1 = new Vector2(p1.x, p1.y + c);
            }

            float p2ToCCDist = Vector2.Distance(p2, c0) + Vector2.Distance(p2, c1);

            if (p2ToCCDist < standDist)
                return true;
            return false;
        }

        public bool IsOverLapWith(TCircle circle)
        {
            return IsOverLapWith(new TEllipse(circle.boundingRect));
        }

        public bool IsOverLapWith(Vector2 point)
        {
            return IsOverLapWith(new TCircle(point, 0));
        }

        public override string ToString()
        {
            return "Pos:" + center.ToString() + " Size:" + size.ToString();
        }

        public static TEllipse Lerp(TEllipse a, TEllipse b, float t)
        {
            return new TEllipse(Vector2.Lerp(a.center, b.center, t), Vector2.Lerp(a.size, b.size, t));
        }

        public static TEllipse operator *(TEllipse rect, float a)
        {
            rect.width *= a;
            rect.height *= a;
            return rect;
        }
    }
}
