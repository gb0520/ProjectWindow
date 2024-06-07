using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB.Extension;

namespace ProjWindow.GUI
{
	public class StaticUIObject : MonoBehaviour
	{
		public static StaticUIObject instance;

		[SerializeField] private Canvas mainCanvas;
		[SerializeField] private RectTransform taskBarTransform;
		[SerializeField] private RectTransform withoutTaskBarTransform;
		[SerializeField] private RectTransform pageArea;

		public Canvas GetMainCanvas()
		{
			return mainCanvas;
		}
		public Vector2 GetTaskBarSize()
		{
			Vector2 size = taskBarTransform.sizeDelta;

			return size;
		}
		public Vector2 GetWithoutTaskBarSize()
		{
			Vector2 size = ProjWindow.GUI.Screen.GetScreenSize();

			size -= taskBarTransform.sizeDelta;

			return size;
		}

		public void SetParentPageArea(Transform transform)
		{
			pageArea.SetParentWithAdapt(transform);
		}

		private void Awake()
		{
			instance = this;
		}
	}
}