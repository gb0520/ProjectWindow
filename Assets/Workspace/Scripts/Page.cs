using ProjWindow.Sub.UI;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace ProjWindow.GUI
{
	public class Page : MonoBehaviour
	{
		[Title("DontOverwrite")]
		[SerializeField] private RectTransform rectTransform;
		[SerializeField] private Canvas canvas;

		#region
		[Title("TitleBar")]
		[SerializeField] private bool isTitleBarDrag;
		[SerializeField] private Vector2 titleBarDragEnterPos;
		[SerializeField] private DragModule dragModule;
		public void OnTitleBarDragEnter()
		{
			isTitleBarDrag = true;
			titleBarDragEnterPos = rectTransform.anchoredPosition;
		}
		public void OnTitleBarDragExit()
		{
			isTitleBarDrag = false;
		}
		public void OnTitleBarDragUpdate()
		{
			rectTransform.anchoredPosition
				= titleBarDragEnterPos + dragModule.direction;
		}
		#endregion

		private void Start()
		{
			canvas = MainCanvas.GetMain();
			dragModule.SetTargetCanvas(canvas);
			dragModule.SetDragEnterCondition(() => isTitleBarDrag);
			dragModule.SetDragExitCondition(() => !isTitleBarDrag);
			dragModule.SetDragUpdateAction(OnTitleBarDragUpdate);
		}
		private void Update()
		{
			dragModule.DragUpdate();
		}
	}
}