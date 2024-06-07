using ProjWindow.Sub.UI;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using ProjWindow.Sub.Event;
using DG.Tweening;
using static ProjWindow.GUI.Page;
using UnityEngine.Events;
using ZB.Extension;
using UnityEngine.UIElements;

namespace ProjWindow.GUI
{
	public class Page : MonoBehaviour
	{
		public bool isClosing;

		[Title("Editable")]
		public bool isCloseable;
		[SerializeField] private float defaultCloseDuration;

		[Title("DontOverwrite")]
		[SerializeField] private RectTransform rectTransform;
		[SerializeField] private Canvas canvas;
		[SerializeField] private CanvasGroup group;
		[SerializeField] private Vector2 lastPosition;
		private FloatDelegate closeProduction;

		public void Close()
		{
			if (isClosing || !isCloseable)
				return;

			isClosing = true;

			float duration = defaultCloseDuration;
			if (closeProduction != null)
				duration = closeProduction.Invoke();
			else
			{
				group.DOKill();
				group.DOFade(0, duration);
				rectTransform.DOKill();
				transform.DOKill();
				transform.DOScale(Vector2.one * 0.9f, duration).SetEase(Ease.OutQuart);
			}
			this.DelayEvent(duration, () => Destroy(this.gameObject));
		}
		public void OnBtnClicked_Close()
		{
			Close();
		}

		public void OnPageSelect()
		{
			transform.SetAsLastSibling();
		}

		#region TitleBar
		[Title("TitleBar - Editable")]
		public bool isDragMoveable;
		[Title("TitleBar - DontOverwrite")]
		[Space]
		[SerializeField] private bool isTitleBarDrag;
		[SerializeField] private Vector2 titleBarDragEnterPos;
		[SerializeField] private DragModule dragModule;

		public void OnTitleBarDragEnter()
		{
			isTitleBarDrag = true;

			Vector2 enterPos = rectTransform.anchoredPosition;
			if (sizeState == SizeState.maximized)
			{
				enterPos = MouseInteract.GetCanvasPosition(Input.mousePosition, canvas) + Vector2.down * lastSize.y / 2;
				SetAnchoredPos(enterPos);
				SetRestore(false);
			}

			titleBarDragEnterPos = enterPos;
		}
		public void OnTitleBarDragExit()
		{
			isTitleBarDrag = false;
		}
		public void OnTitleBarDragUpdate()
		{
			SetAnchoredPos(titleBarDragEnterPos + dragModule.direction);
		}
		#endregion

		#region Size
		public enum SizeState { restored, maximized, minimized }

		[Title("Size - Editable")]
		public bool isMinimizeable;
		public bool isMaximizeable;
		public bool isResizable;
		public Vector2 minSize;
		[SerializeField] private float defaultMinimizeDuration;
		[SerializeField] private float defaultMaximizeDuration;
		[SerializeField] private float defaultRestoreDuration;

		[Title("Size - DontOverwrite")]
		[SerializeField] private bool isResizing;
		[SerializeField] private Vector2 lastSize;
		[SerializeField] private SizeState sizeState;
		[SerializeField] private Vector2 resizeEnterSize;
		[SerializeField]private Vector2 resizeHandlerFocus;
		[SerializeField]private Vector2 resizeHandlerDragNowPos;
		private FloatDelegate minimizeProduction;
		private FloatDelegate maximizeProduction;
		private FloatDelegate restoreProduction;
		[SerializeField] private Vector2 cacedSize;

		public void SetMinimize()
		{
			if (isClosing || !isMinimizeable)
				return;

			sizeState = SizeState.minimized;
			float duration = defaultMinimizeDuration;
			if (minimizeProduction != null)
			{
				duration = minimizeProduction.Invoke();
			}
		}
		public void SetMaximize()
		{
			if (isClosing || !isMaximizeable)
				return;

			sizeState = SizeState.maximized;
			float duration = defaultMaximizeDuration;
			if (maximizeProduction != null)
			{
				duration = maximizeProduction.Invoke();
			}
			else
			{
				Vector2 size = StaticUIObject.instance.GetWithoutTaskBarSize();
				rectTransform.DOKill();
				rectTransform.DOSizeDelta(size, duration).SetEase(Ease.InOutQuart);
				rectTransform.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.InOutQuart);
			}
			this.DelayEvent(duration, () =>
			{
				rectTransform.sizeDelta = Vector2.zero;
				rectTransform.SetAnchor(AnchorPresets.StretchAll);
			});
		}
		public void SetRestore(bool isSetPos = true)
		{
			if (isClosing)
				return;

			sizeState = SizeState.restored;
			float duration = defaultRestoreDuration;
			if (restoreProduction != null)
			{
				duration = restoreProduction.Invoke();
			}
			else
			{
				Vector2 position = lastPosition;
				Vector2 size = lastSize;
				rectTransform.SetAnchor(AnchorPresets.MiddleCenter);
				rectTransform.sizeDelta = StaticUIObject.instance.GetWithoutTaskBarSize();
				rectTransform.DOKill();
				rectTransform.DOSizeDelta(size, duration).SetEase(Ease.InOutQuart);
				if (isSetPos) 
					rectTransform.DOAnchorPos(position, duration).SetEase(Ease.InOutQuart);
			}
		}

		public void OnBtnClicked_Minimize()
		{
			SetMinimize();
		}
		public void OnBtnClicked_Maximize()
		{
			if (isClosing || !isMaximizeable)
				return;

			if (sizeState != SizeState.maximized)
			{
				SetMaximize();
			}
			else if (sizeState != SizeState.restored)
			{
				SetRestore();
			}
		}
		public void OnDrag_ResizeHandler(int dir)
		{
			isResizing = true;

			Vector2 position = Vector2.zero;
			switch (dir)
			{
				case 0:	position = new Vector2(0, 1);	break;
				case 1:	position = new Vector2(1, 1);	break;
				case 2:	position = new Vector2(1, 0);	break;
				case 3:	position = new Vector2(1, -1);	break;
				case 4:	position = new Vector2(0, -1);	break;
				case 5:	position = new Vector2(-1, -1);	break;
				case 6:	position = new Vector2(-1, 0);	break;
				case 7:	position = new Vector2(-1, 1);	break;
			}
			resizeHandlerFocus = position;

			resizeEnterSize = lastSize;
		}
		public void OnDrop_ResizeHandler()
		{
			isResizing = false;
		}
		public void ResizeUpdate()
		{
			if (isResizable && isResizing)
			{
				// 현재 마우스 위치를 가져옴
				resizeHandlerDragNowPos = MouseInteract.GetCanvasPosition(Input.mousePosition, canvas);

				Vector2 frontPoint = lastPosition - resizeHandlerFocus * (lastSize / 2);
				Vector2 backPoint = resizeHandlerDragNowPos
					+ Vector2.down * GUI.StaticUIObject.instance.GetTaskBarSize().y / 2;
				Vector2 direction = backPoint - frontPoint;

				//사이즈 조절
				Vector2 addScale = (direction).Abs();
				//-커지는 방향 보정
				addScale *= resizeHandlerFocus.Abs();
				//-이외의 요소 고정
				Vector2 fix = new Vector2(resizeHandlerFocus.y, resizeHandlerFocus.x).Abs();
				if (fix.x != 0 && fix.y != 0) fix = Vector2.zero;
				Vector2 fixScale = fix * resizeEnterSize;

				Vector2 newSize = addScale + fixScale;
				Vector2 cacedSize = newSize.Max(minSize);
				rectTransform.sizeDelta = cacedSize;
				lastSize = cacedSize;

				//위치 조절
				Vector2 addPos = (backPoint + frontPoint) / 2;
				//-위치 이동방향 고정
				addPos *= resizeHandlerFocus.Abs();
				//-이외의 요소 고정
				Vector2 fixPos = fix * lastPosition;
				Vector2 position = addPos + fixPos;

				float x = rectTransform.anchoredPosition.x;
				float y = rectTransform.anchoredPosition.y;
				if (lastSize.x > minSize.x && (resizeHandlerFocus.x * (direction.x) >= 0)) x = position.x;
				if (lastSize.y > minSize.y && (resizeHandlerFocus.y * (direction.y) >= 0)) y = position.y;
				//if (lastSize.x >= minSize.x && newSize.x >= minSize.x) x = position.x;
				//if (lastSize.y >= minSize.y && newSize.y >= minSize.y) y = position.y;

				position = new Vector2(x, y);
				this.cacedSize = cacedSize;

				SetAnchoredPos(position);
			}
		}


		#endregion

		private void Start()
		{
			canvas = StaticUIObject.instance.GetMainCanvas();

			dragModule.SetTargetCanvas(canvas);
			dragModule.SetDragEnterCondition(() => isTitleBarDrag);
			dragModule.SetDragExitCondition(() => (!isTitleBarDrag || !isDragMoveable || isClosing));
			dragModule.SetDragUpdateAction(OnTitleBarDragUpdate);
			dragModule.SetDragPosDelegate(()=>MouseInteract.GetCanvasPosition(Input.mousePosition, canvas));

			lastSize = rectTransform.sizeDelta;
			lastPosition = rectTransform.anchoredPosition;
		}
		private void Update()
		{
			dragModule.DragUpdate();
			ResizeUpdate();
		}
		private void SetAnchoredPos(Vector2 position)
		{
			rectTransform.anchoredPosition = position;
			lastPosition = position;
		}
	}
}