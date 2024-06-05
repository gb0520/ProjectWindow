using ProjWindow.Sub.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Extension
{
	public static class MonoBehaviourExtension
	{
		public static IEnumerator DelayEvent(this MonoBehaviour monoBehaviour, float delay, VoidDelegate voidDelegate)
		{
			IEnumerator result = DelayCycle(delay, voidDelegate);
			monoBehaviour.StartCoroutine(result);
			return result;
		}
		private static IEnumerator DelayCycle(float delay, VoidDelegate voidDelegate)
		{
			yield return new WaitForSeconds(delay);
			voidDelegate.Invoke();
		}
	}
}