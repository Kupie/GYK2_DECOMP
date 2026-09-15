using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000309 RID: 777
[Serializable]
public class CPA_SetGameObjectActive : CapturePointAction
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x060014BC RID: 5308 RVA: 0x000658E3 File Offset: 0x00063AE3
	private bool IsGameObjectSource
	{
		get
		{
			return this.source == CPA_SetGameObjectActive.GameObjectSource.GameObject;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x060014BD RID: 5309 RVA: 0x000658EE File Offset: 0x00063AEE
	private bool IsAddressableSource
	{
		get
		{
			return this.source == CPA_SetGameObjectActive.GameObjectSource.Addressable;
		}
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x000658FC File Offset: 0x00063AFC
	public override void Execute(LazyConsts.Fighting.TeamType teamType, FightingLevel level)
	{
		if (this.teamType != teamType)
		{
			return;
		}
		this.currentLevel = level;
		GameObject gameObject = this.ResolveTarget(level);
		if (gameObject)
		{
			gameObject.SetActive(this.isActive);
		}
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x00065938 File Offset: 0x00063B38
	private GameObject ResolveTarget(FightingLevel level)
	{
		CPA_SetGameObjectActive.GameObjectSource gameObjectSource = this.source;
		GameObject gameObject;
		if (gameObjectSource != CPA_SetGameObjectActive.GameObjectSource.GameObject)
		{
			if (gameObjectSource != CPA_SetGameObjectActive.GameObjectSource.Addressable)
			{
				gameObject = this.go;
			}
			else
			{
				gameObject = this.ResolveFromAddressable(level);
			}
		}
		else
		{
			gameObject = this.go;
		}
		return gameObject;
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x00065970 File Offset: 0x00063B70
	private GameObject ResolveFromAddressable(FightingLevel level)
	{
		GameObject gameObject;
		if (!this.TryGetLoadedAddressableRoot(out gameObject))
		{
			return null;
		}
		FightingStageGameObjectMapper componentInChildren = gameObject.GetComponentInChildren<FightingStageGameObjectMapper>(true);
		if (!componentInChildren)
		{
			return null;
		}
		GameObject gameObject2;
		if (!componentInChildren.TryGetGameObject(this.gameObjectId, out gameObject2))
		{
			return null;
		}
		return gameObject2;
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x000659AE File Offset: 0x00063BAE
	private bool TryGetLoadedAddressableRoot(out GameObject root)
	{
		root = null;
		if (this.gameObjectReference == null)
		{
			return false;
		}
		root = this.currentLevel.GetStageInstanceFromAssetReference(this.gameObjectReference);
		return root != null;
	}

	// Token: 0x04001576 RID: 5494
	public CPA_SetGameObjectActive.GameObjectSource source;

	// Token: 0x04001577 RID: 5495
	public GameObject go;

	// Token: 0x04001578 RID: 5496
	public AssetReferenceGameObject gameObjectReference;

	// Token: 0x04001579 RID: 5497
	public string gameObjectId;

	// Token: 0x0400157A RID: 5498
	public bool isActive;

	// Token: 0x0400157B RID: 5499
	private FightingLevel currentLevel;

	// Token: 0x0200030A RID: 778
	public enum GameObjectSource
	{
		// Token: 0x0400157D RID: 5501
		GameObject,
		// Token: 0x0400157E RID: 5502
		Addressable
	}
}
