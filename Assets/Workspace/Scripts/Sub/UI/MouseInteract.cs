using ProjWindow.Sub;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ProjWindow.Sub.UI
{
	public class MouseInteract
	{
		public static Vector2 GetCanvasPosition(Vector2 mousePosition, Canvas canvas)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				canvas.transform as RectTransform,
				mousePosition,
				canvas.worldCamera, 
				out Vector2 localPoint);

			return localPoint;
		}
	}
}