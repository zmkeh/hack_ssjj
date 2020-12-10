using UnityEngine;
namespace ssjj_hack
{
    public struct TRect
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

        public TRect(Vector2 centerPos, Vector2 size) : this(centerPos.x, centerPos.y, size.x, size.y)
        {

        }

        public TRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
        }

        /// <summary>
        /// x, y, w, h
        /// </summary>
        /// <param name="valueArray"></param>
        public TRect(float[] valueArray)
        {
            this.x = valueArray[0];
            this.y = valueArray[1];
            this.width = valueArray[2];
            this.height = valueArray[3];
        }

        public bool IsOverLapWith(TRect rect)
        {
            float verticalDistance = Mathf.Abs(y - rect.y); ;       //垂直距离
            float horizontalDistance = Mathf.Abs(x - rect.x); ;     //水平距离
            float verticalThreshold = (height + rect.height) / 2;   //两矩形分离的垂直临界值
            float horizontalThreshold = (width + rect.width) / 2;   //两矩形分离的水平临界值
            if (verticalDistance > verticalThreshold || horizontalDistance > horizontalThreshold)
                return false;
            return true;
        }

        public bool IsOverLapWith(Vector2 point)
        {
            return Mathf.Abs(point.x - x) < width * 0.5f && Mathf.Abs(point.y - y) < height * 0.5f;
        }

        /// 如果超出边界，按照Pos到Center线段与矩形边框交叉点,作为返回值
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2 LimitInside(Vector2 pos)
        {
            TLine centerToPos = new TLine(center, pos);
            if (pos.x < left)
            {
                TLine line = new TLine(new Vector2(left, top), new Vector2(left, bottom));
                Vector2 crsp = line.CrossWith(centerToPos);
                if (crsp.y <= top && crsp.y >= bottom)
                {
                    pos.x = left;
                    pos.y = crsp.y;
                }
            }
            else if (pos.x > right)
            {
                TLine line = new TLine(new Vector2(right, top), new Vector2(right, bottom));
                Vector2 crsp = line.CrossWith(centerToPos);
                if (crsp.y <= top && crsp.y >= bottom)
                {
                    pos.x = right;
                    pos.y = crsp.y;
                }
            }
            if (pos.y < bottom)
            {
                TLine line = new TLine(new Vector2(left, bottom), new Vector2(right, bottom));
                Vector2 crsp = line.CrossWith(centerToPos);
                if (crsp.x <= right && crsp.x >= left)
                {
                    pos.x = crsp.x;
                    pos.y = bottom;
                }
            }
            else if (pos.y > top)
            {
                TLine line = new TLine(new Vector2(left, top), new Vector2(right, top));
                Vector2 crsp = line.CrossWith(centerToPos);
                if (crsp.x <= right && crsp.x >= left)
                {
                    pos.x = crsp.x;
                    pos.y = top;
                }
            }
            return pos;
        }

        /// <summary>
        /// 如果超出边界，按照Pos超出方向直接拉回到边框位置,作为返回值
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2 LimitInside2(Vector2 pos)
        {
            if (pos.x < left)
                pos.x = left;
            else if (pos.x > right)
                pos.x = right;

            if (pos.y < bottom)
                pos.y = bottom;
            else if (pos.y > top)
                pos.y = top;
            return pos;
        }

        public TCircle LimitInside2(TCircle circle)
        {
            float newWidth = Mathf.Max(0, width - circle.radius * 2);
            float newHeight = Mathf.Max(0, height - circle.radius * 2);
            TRect newRect = new TRect(x, y, newWidth, newHeight);
            Vector2 circlePos = newRect.LimitInside2(circle.center);
            return new TCircle(circlePos, circle.radius);
        }

        public TRect LimitInside2(TRect rect)
        {
            float newWidth = Mathf.Max(0, width - rect.width);
            float newHeight = Mathf.Max(0, height - rect.height);
            TRect newRect = new TRect(x, y, newWidth, newHeight);
            Vector2 circlePos = newRect.LimitInside2(rect.center);
            return new TRect(circlePos, rect.size);
        }

        public TCircle LimitInside(TCircle circle)
        {
            float newWidth = Mathf.Max(0, width - circle.radius * 2);
            float newHeight = Mathf.Max(0, height - circle.radius * 2);
            TRect newRect = new TRect(x, y, newWidth, newHeight);
            Vector2 circlePos = newRect.LimitInside(circle.center);
            return new TCircle(circlePos, circle.radius);
        }

        public TRect LimitInside(TRect rect)
        {
            TRect _rect = new TRect(x, y, width - rect.width * 0.5f, height - rect.height * 0.5f);
            var pos = _rect.LimitInside(new Vector2(rect.x, rect.y));
            return new TRect(pos, rect.size);
        }

        public Vector2 LerpPoint(Vector2 lerp)
        {
            float posX = left + lerp.x * width;
            float posY = bottom + lerp.y * height;
            return new Vector2(posX, posY);
        }

        public override string ToString()
        {
            return "Pos:" + center.ToString() + " Size:" + size.ToString();
        }

        public static TRect Lerp(TRect a, TRect b, float t)
        {
            return new TRect(Vector2.Lerp(a.center, b.center, t), Vector2.Lerp(a.size, b.size, t));
        }

        public static TRect operator *(TRect rect, float a)
        {
            rect.width *= a;
            rect.height *= a;
            return rect;
        }
        public static TRect operator +(TRect a, TRect b)
        {
            var centerX = (a.x + b.x) * 0.5f;
            var centerY = (a.y + b.y) * 0.5f;
            var width = Mathf.Max(Mathf.Abs((a.right - b.left)), Mathf.Abs((b.right - a.left)));
            var height = Mathf.Max(Mathf.Abs((a.bottom - b.top)), Mathf.Abs((b.bottom - a.top)));
            return new TRect(centerX, centerY, width, height);
        }
    }
}
