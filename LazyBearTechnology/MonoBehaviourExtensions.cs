using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public static class MonoBehaviourExtensions
{
	// Token: 0x060000A1 RID: 161 RVA: 0x000047B4 File Offset: 0x000029B4
	public static T Copy<T>(this T source, Transform parent = null, bool activate = true, string name = "") where T : MonoBehaviour
	{
		if (source == null)
		{
			Debug.LogError("Copy Method error, prefab is null");
			return default(T);
		}
		T t = global::UnityEngine.Object.Instantiate<T>(source, parent ?? source.transform.parent, false);
		if (!string.IsNullOrEmpty(name))
		{
			t.name = name;
		}
		t.gameObject.SetActive(activate);
		return t;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00004828 File Offset: 0x00002A28
	public static T GetComponentInParentExcludeCurrent<T>(this Component component, bool includeInactive) where T : Component
	{
		T t = default(T);
		if (component.transform.parent == null)
		{
			return t;
		}
		return component.transform.parent.GetComponentInParent<T>(includeInactive);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00004863 File Offset: 0x00002A63
	public static bool HasComponentInParentExcludeCurrent<T>(this Component component, bool includeInactive) where T : Component
	{
		return !(component.transform.parent == null) && component.transform.parent.GetComponentInParent<T>(includeInactive) != null;
	}
}
