using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Extension
{
	public static class TransformExtension
	{
		public static void SetParentWithAdapt(this Transform transform, Transform parent)
		{
			transform.SetParent(parent);
			transform.localPosition = Vector2.zero;
			transform.localScale = Vector2.one;
			transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}
}