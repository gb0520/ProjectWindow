using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace ProjWindow.Sub.Object
{
	public class ObjectStaticList : MonoBehaviour
	{
		public static Dictionary<string, List<StringTransformPair>> collections = new Dictionary<string, List<StringTransformPair>>();

		[ShowInInspector] private List<string> keys
		{
			get => collections.Keys.ToList(); 
		}
		[ShowInInspector] private List<List<StringTransformPair>> values
		{
			get => collections.Values.ToList(); 
		}

		[SerializeField] private string listID;
		[SerializeField] private string objectID;

		public static List<StringTransformPair> GetList(string listID)
		{
			foreach (var item in collections)
			{
				if (item.Key == listID)
					return item.Value;
			}

			return null;
		}
		public static Transform GetObject(string listID, string objectID)
		{
			var list = GetList(listID);
			if (list == null)
				return null;

			foreach (var item in list)
			{
				if (item.id == objectID)
					return item.transform;
			}
			return null;
		}
		public static void AddObject(string listID, string objectID, Transform transform)
		{
			List<StringTransformPair> list = GetList(listID);

			if (list == null)
			{
				list = new List<StringTransformPair>();
				collections.Add(listID, list);
			}

			StringTransformPair pair = new StringTransformPair()
			{
				id = objectID,
				transform = transform
			};
			list.Add(pair);
		}

		private void Awake()
		{
			AddObject(listID, objectID, transform);
		}

		public class StringTransformPair
		{
			public string id;
			public Transform transform;
		}
	}
}