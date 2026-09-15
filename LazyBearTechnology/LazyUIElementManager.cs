using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000153 RID: 339
	public class LazyUIElementManager : MonoBehaviour
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x000252FC File Offset: 0x000234FC
		private static LazyUIElementManager Instance
		{
			get
			{
				if (LazyUIElementManager.instance == null)
				{
					LazyUIElementManager.instance = global::UnityEngine.Object.FindObjectOfType<LazyUIElementManager>();
					if (LazyUIElementManager.instance == null)
					{
						Debug.LogError("LazyUIElementManager.Instance error: Couldn't find a LazyUIElementManager object.");
					}
				}
				return LazyUIElementManager.instance;
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00025331 File Offset: 0x00023531
		private void Awake()
		{
			LazyUIElementManager.instance = this;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002533C File Offset: 0x0002353C
		public static bool TryRegisterElement(ILazyUIElementWithId elementWithId)
		{
			if (string.IsNullOrEmpty(elementWithId.LazyUIElementId))
			{
				Debug.LogWarning("#lazy_ui# Trying to register elementWithId with empty id!");
			}
			foreach (ILazyUIElementWithId lazyUIElementWithId in LazyUIElementManager.Instance.elements)
			{
				if (lazyUIElementWithId != elementWithId && lazyUIElementWithId.LazyUIElementId == elementWithId.LazyUIElementId)
				{
					Debug.LogWarning("#lazy_ui# Duplicate elementWithId id:[" + elementWithId.LazyUIElementId + "]!");
				}
			}
			if (!LazyUIElementManager.Instance.elements.Contains(elementWithId))
			{
				LazyUIElementManager.Instance.elements.Add(elementWithId);
				return true;
			}
			return false;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000253FC File Offset: 0x000235FC
		public static bool TryUnregisterElement(ILazyUIElementWithId elementWithId)
		{
			if (LazyUIElementManager.Instance == null)
			{
				return false;
			}
			if (LazyUIElementManager.Instance.elements.Contains(elementWithId))
			{
				LazyUIElementManager.Instance.elements.Remove(elementWithId);
				return true;
			}
			return false;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00025434 File Offset: 0x00023634
		public static bool TryGetById(string id, out ILazyUIElementWithId elementWithId)
		{
			elementWithId = LazyUIElementManager.Instance.elements.Find((ILazyUIElementWithId b) => b.LazyUIElementId == id);
			return elementWithId != null;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00025470 File Offset: 0x00023670
		public static bool TryGetById<T>(string id, out T element) where T : MonoBehaviour
		{
			ILazyUIElementWithId lazyUIElementWithId;
			element = (LazyUIElementManager.TryGetById(id, out lazyUIElementWithId) ? lazyUIElementWithId.MonoBehaviour.GetComponent<T>() : default(T));
			return element != null;
		}

		// Token: 0x0400046A RID: 1130
		private static LazyUIElementManager instance;

		// Token: 0x0400046B RID: 1131
		public List<ILazyUIElementWithId> elements = new List<ILazyUIElementWithId>();
	}
}
