using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Extension
{
	public static class VectorExtension
	{
		public static Vector2 Abs(this Vector2 vector2)
		{
			Vector2 result = new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
			return result;
		}
		public static Vector2 Min(this Vector2 vector2, Vector2 min)
		{
			float x = Mathf.Min(vector2.x, min.x);
			float y = Mathf.Min(vector2.y, min.y);

			Vector2 result = new Vector2(x, y);
			return result;
		}
		public static Vector2 Max(this Vector2 vector2, Vector2 max)
		{
			float x = Mathf.Max(vector2.x, max.x);
			float y = Mathf.Max(vector2.y, max.y);

			Vector2 result = new Vector2(x, y);
			return result;
		}
	}
}