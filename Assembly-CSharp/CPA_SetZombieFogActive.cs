using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200030D RID: 781
[Serializable]
public class CPA_SetZombieFogActive : CapturePointAction
{
	// Token: 0x060014C7 RID: 5319 RVA: 0x00065A54 File Offset: 0x00063C54
	public override void Execute(LazyConsts.Fighting.TeamType teamType, FightingLevel level)
	{
		if (this.teamType != teamType)
		{
			return;
		}
		this.currentLevel = level;
		ZombieFog zombieFog = this.ResolveTarget(level);
		if (zombieFog)
		{
			zombieFog.DoFade(this.isActive);
		}
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x00065A90 File Offset: 0x00063C90
	private ZombieFog ResolveTarget(FightingLevel level)
	{
		CPA_SetZombieFogActive.GameObjectSource gameObjectSource = this.source;
		ZombieFog zombieFog;
		if (gameObjectSource != CPA_SetZombieFogActive.GameObjectSource.GameObject)
		{
			if (gameObjectSource != CPA_SetZombieFogActive.GameObjectSource.Addressable)
			{
				zombieFog = this.fog;
			}
			else
			{
				zombieFog = this.ResolveFromAddressable(level);
			}
		}
		else
		{
			zombieFog = this.fog;
		}
		return zombieFog;
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x00065AC8 File Offset: 0x00063CC8
	private ZombieFog ResolveFromAddressable(FightingLevel level)
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
		if (!gameObject2)
		{
			return null;
		}
		return gameObject2.GetComponent<ZombieFog>();
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x00065B15 File Offset: 0x00063D15
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

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x060014CB RID: 5323 RVA: 0x00065B3F File Offset: 0x00063D3F
	private bool IsGameObjectSource
	{
		get
		{
			return this.source == CPA_SetZombieFogActive.GameObjectSource.GameObject;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x060014CC RID: 5324 RVA: 0x00065B4A File Offset: 0x00063D4A
	private bool IsAddressableSource
	{
		get
		{
			return this.source == CPA_SetZombieFogActive.GameObjectSource.Addressable;
		}
	}

	// Token: 0x04001584 RID: 5508
	public CPA_SetZombieFogActive.GameObjectSource source;

	// Token: 0x04001585 RID: 5509
	public ZombieFog fog;

	// Token: 0x04001586 RID: 5510
	public AssetReferenceGameObject gameObjectReference;

	// Token: 0x04001587 RID: 5511
	public string gameObjectId;

	// Token: 0x04001588 RID: 5512
	public bool isActive;

	// Token: 0x04001589 RID: 5513
	private FightingLevel currentLevel;

	// Token: 0x0200030E RID: 782
	public enum GameObjectSource
	{
		// Token: 0x0400158B RID: 5515
		GameObject,
		// Token: 0x0400158C RID: 5516
		Addressable
	}
}
