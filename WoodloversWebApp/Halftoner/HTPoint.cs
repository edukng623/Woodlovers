using System;

namespace Halftoner
{
	public class HTPoint
	{
		public float x;

		public float y;

		public float w;

		public float Length => (float)Math.Sqrt((double)(x * x + y * y + w * w));

		public HTPoint()
		{
			x = (y = (w = 0f));
		}

		public HTPoint(float _x, float _y, float _w)
		{
			x = _x;
			y = _y;
			w = _w;
		}

		public static HTPoint operator +(HTPoint a, HTPoint b)
		{
			return new HTPoint(a.x + b.x, a.y + b.y, a.w + b.w);
		}

		public static HTPoint operator -(HTPoint a, HTPoint b)
		{
			return new HTPoint(a.x - b.x, a.y - b.y, a.w - b.w);
		}

		public static HTPoint operator *(HTPoint a, float b)
		{
			return new HTPoint(a.x * b, a.y * b, a.w * b);
		}
	}
}
