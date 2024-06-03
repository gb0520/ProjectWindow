using ProjWindow.GUI;
using ProjWindow.Sub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjWindow.Sub.Event;

namespace ProjWindow.Sub.UI
{
	[System.Serializable]
	public class DragModule
	{
		public bool isPlaying;

		public Vector2 enterPosition { get; private set; }
		public Vector2 nowPosition { get; private set; }
		public Vector2 exitPosition { get; private set; }
		public Vector2 direction { get; private set; }

		[SerializeField] private Canvas targetCanvas;

		private Booldelegate dragExitCondition;
		private Booldelegate dragEnterCondition;
		private UnityEvent dragExitEvent = new UnityEvent();
		private UnityEvent dragUpdateEvent = new UnityEvent();

		public void SetDragExitAction(UnityAction action)
		{
			dragExitEvent.AddListener(action);
		}
		public void SetDragUpdateAction(UnityAction action)
		{
			dragUpdateEvent.AddListener(action);
		}

		public void SetDragEnter()
		{
			isPlaying = true;
			enterPosition = MouseInteract.GetCanvasPosition(Input.mousePosition, targetCanvas);
		}
		public void DragUpdate()
		{
			if (!isPlaying)
			{
				if (dragEnterCondition.Invoke())
				{
					isPlaying = true;
					enterPosition = MouseInteract.GetCanvasPosition(Input.mousePosition, targetCanvas);
				}
			}

			if (isPlaying)
			{
				nowPosition = MouseInteract.GetCanvasPosition(Input.mousePosition, targetCanvas);
				direction = nowPosition - enterPosition;

				dragUpdateEvent.Invoke();

				if (dragExitCondition.Invoke())
				{
					exitPosition = nowPosition;
					DragExit();
				}
			}
		}

		/// <summary>
		/// 초기화 - 드래그 시작 조건
		/// </summary>
		/// <param name="boolDelegate"></param>
		public void SetDragEnterCondition(Booldelegate boolDelegate)
		{
			this.dragEnterCondition = boolDelegate;
		}
		/// <summary>
		/// 초기화 - 드래그 종료 조건
		/// </summary>
		/// <param name="boolDelegate"></param>
		public void SetDragExitCondition(Booldelegate boolDelegate)
		{
			this.dragExitCondition = boolDelegate;
		}
		/// <summary>
		/// 초기화 - 대상 캔버스
		/// </summary>
		/// <param name="canvas"></param>
		public void SetTargetCanvas(Canvas canvas)
		{
			targetCanvas = canvas;
		}

		private void DragExit()
		{
			isPlaying = false;
			dragExitEvent?.Invoke();
			dragExitEvent.RemoveAllListeners();
			exitPosition = MouseInteract.GetCanvasPosition(Input.mousePosition, targetCanvas);
		}
	}
}