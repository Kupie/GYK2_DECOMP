using System;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public static class HudFX
{
	// Token: 0x0600215C RID: 8540 RVA: 0x0009D744 File Offset: 0x0009B944
	public static GameObject Spawn(RectTransform parent, string fxName)
	{
		if (parent == null || string.IsNullOrEmpty(fxName))
		{
			return null;
		}
		GameObject gameObject;
		if (!WorldFXPool.TryLoadPrefab(fxName, out gameObject))
		{
			Debug.LogError("Error spawning HudFX \"" + fxName + "\": Effect not found.");
			return null;
		}
		GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject, parent, false);
		gameObject2.name = fxName;
		gameObject2.transform.SetAsLastSibling();
		RectTransform rectTransform = gameObject2.transform as RectTransform;
		if (rectTransform != null)
		{
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.localPosition = Vector3.zero;
			rectTransform.localRotation = Quaternion.identity;
		}
		global::UnityEngine.Object.Destroy(gameObject2, HudFX.GetLifetime(gameObject2));
		return gameObject2;
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x0009D808 File Offset: 0x0009BA08
	private static float GetLifetime(GameObject instance)
	{
		float num = 0.5f;
		ParticleSystem[] componentsInChildren = instance.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ParticleSystem.MainModule main = componentsInChildren[i].main;
			float num2 = Mathf.Max(main.startLifetime.constantMin, main.startLifetime.constantMax);
			float num3 = main.startDelay.constant + main.duration + num2;
			if (main.simulationSpeed > 0f)
			{
				num3 /= main.simulationSpeed;
			}
			if (num3 > num)
			{
				num = num3;
			}
		}
		return num + 0.1f;
	}
}
