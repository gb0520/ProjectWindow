using ProjWindow.Sub.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjWindow.GUI
{
	public static class MainCanvas
	{
		public static Canvas GetMain()
		{
			var transform = ObjectStaticList.GetObject("Canvas", "Main");
			Canvas result = transform?.GetComponent<Canvas>();

			return result;
		}
	}
}