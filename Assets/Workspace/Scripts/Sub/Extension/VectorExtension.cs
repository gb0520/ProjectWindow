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
	}
}