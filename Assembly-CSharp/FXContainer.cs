using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class FXContainer : MonoBehaviour
{
	// Token: 0x06002145 RID: 8517 RVA: 0x0009CEE0 File Offset: 0x0009B0E0
	private void Awake()
	{
		foreach (WorldFX worldFX in this.fxObjects)
		{
			worldFX.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x0009CF38 File Offset: 0x0009B138
	public void PlayFx(string fxName)
	{
		WorldFX worldFX = this.fxObjects.Find((WorldFX x) => x.gameObject.name == fxName);
		if (worldFX == null)
		{
			Debug.LogWarning("FX not found: " + fxName);
			return;
		}
		worldFX.gameObject.SetActive(true);
		worldFX.Play(null, delegate
		{
			worldFX.gameObject.SetActive(false);
		});
	}

	// Token: 0x04001DDC RID: 7644
	[SerializeField]
	private List<WorldFX> fxObjects = new List<WorldFX>();
}
