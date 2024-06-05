using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjWindow.GUI
{
	public class Screen
	{
		public static Vector2 GetScreenSize()
		{
			Vector2 result = new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height);
			return result;
		}
	}
}