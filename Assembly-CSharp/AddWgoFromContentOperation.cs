using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000474 RID: 1140
[Serializable]
public class AddWgoFromContentOperation : SaveFixWgoOperation
{
	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06001DFF RID: 7679 RVA: 0x0008CFDE File Offset: 0x0008B1DE
	public AssetReferenceGameObject ContentDataRef
	{
		get
		{
			return this.contentDataRef;
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06001E00 RID: 7680 RVA: 0x0008CFE6 File Offset: 0x0008B1E6
	public SGuid WgoUniqueId
	{
		get
		{
			return this.wgoUniqueId;
		}
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06001E01 RID: 7681 RVA: 0x0008CFEE File Offset: 0x0008B1EE
	public string DebugLabel
	{
		get
		{
			return this.debugLabel;
		}
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x0008CFF6 File Offset: 0x0008B1F6
	public override string Info()
	{
		return "add wgo";
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06001E03 RID: 7683 RVA: 0x0008CFFD File Offset: 0x0008B1FD
	protected override string SummaryBody
	{
		get
		{
			if (!string.IsNullOrEmpty(this.debugLabel))
			{
				return this.debugLabel;
			}
			SGuid sguid = this.wgoUniqueId;
			if (sguid == null)
			{
				return null;
			}
			return sguid.ToString();
		}
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x0008D024 File Offset: 0x0008B224
	public override bool TryGetTargetUniqueId(out SGuid uniqueId)
	{
		uniqueId = this.wgoUniqueId;
		return !SGuid.IsNullOrEmpty(this.wgoUniqueId);
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x0008D03C File Offset: 0x0008B23C
	public void SetTarget(AssetReferenceGameObject contentRef, SGuid uniqueId, string label)
	{
		this.contentDataRef = contentRef;
		this.wgoUniqueId = uniqueId;
		this.debugLabel = label;
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x0008D054 File Offset: 0x0008B254
	public override void Apply(SaveFixContext ctx)
	{
		if (this.contentDataRef == null || string.IsNullOrEmpty(this.contentDataRef.AssetGUID))
		{
			ctx.LogError(this.Summary + ": contentDataRef is empty");
			return;
		}
		if (SGuid.IsNullOrEmpty(this.wgoUniqueId))
		{
			ctx.LogError(this.Summary + ": wgoUniqueId is empty");
			return;
		}
		if (ctx.HasWgo(this.wgoUniqueId))
		{
			ctx.Log(this.Summary + ": already present in save, skip");
			return;
		}
		SceneWgoContentPart sceneWgoContentPart;
		GameSceneData gameSceneData;
		GameSceneConfig gameSceneConfig;
		if (!ctx.TryGetContentPart(this.contentDataRef.AssetGUID, out sceneWgoContentPart, out gameSceneData, out gameSceneConfig))
		{
			return;
		}
		WgoData wgoData = AddWgoFromContentOperation.FindWgo(sceneWgoContentPart, this.wgoUniqueId);
		if (wgoData == null)
		{
			ctx.LogError(string.Format("{0}: uniqueId [{1}] not found in content [{2}]", this.Summary, this.wgoUniqueId, sceneWgoContentPart.name));
			return;
		}
		WgoData wgoData2 = wgoData.CreateDataFromMe(gameSceneConfig.sceneGlobalPosition, gameSceneData.id, true);
		if (wgoData.startReses != null)
		{
			foreach (StartReses.StartItemData startItemData in wgoData.startReses.startItems)
			{
				wgoData2.Inventory.AddItemToInventory(new Item(startItemData.id, startItemData.count), null, false);
			}
			wgoData2.SetGameRes(wgoData.startReses.startGameRes);
			wgoData2.GameResStr.Set(wgoData.startReses.startGameResStr);
		}
		WgoData wgoData3;
		GameSceneData gameSceneData2;
		if (AddWgoFromContentOperation.IsGraveyardModule(wgoData2.id) && ctx.TryFindWgoByGroupNear("graveyard_modules", wgoData2.Position, 0.05f, out wgoData3, out gameSceneData2))
		{
			ctx.Log(string.Format("{0}: skip, [{1}] [{2}] of group [{3}] within {4} of {5}", new object[] { this.Summary, wgoData3.id, wgoData3.UniqueId, "graveyard_modules", 0.05f, wgoData2.Position }));
			return;
		}
		ctx.AddWgoData(gameSceneData, wgoData2);
		ctx.Log(this.Summary + ": added to scene [" + gameSceneData.id + "]");
		this.TryRegisterPrebuiltWorldZoneQualitySlot(ctx, sceneWgoContentPart, wgoData2);
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x0008D298 File Offset: 0x0008B498
	private void TryRegisterPrebuiltWorldZoneQualitySlot(SaveFixContext ctx, SceneWgoContentPart contentPart, WgoData wgoData)
	{
		if (wgoData == null)
		{
			return;
		}
		WorldZonePrebuiltWgoParams worldZonePrebuiltWgoParams;
		if (!WorldZoneBakedData.TryGetPrebuiltWgoParams(contentPart.WorldZones, wgoData.UniqueId, out worldZonePrebuiltWgoParams))
		{
			return;
		}
		if (ctx.GameSave.worldData.TryExecutePrebuiltWgoAfterBuildingExpressions(wgoData, worldZonePrebuiltWgoParams))
		{
			ctx.Log(this.Summary + ": registered prebuilt world zone quality slot");
		}
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x0008D2EC File Offset: 0x0008B4EC
	private static WgoData FindWgo(SceneWgoContentPart contentPart, SGuid uniqueId)
	{
		if (((contentPart != null) ? contentPart.Wgos : null) == null)
		{
			return null;
		}
		foreach (WgoData wgoData in contentPart.Wgos)
		{
			if (wgoData != null && wgoData.UniqueId == uniqueId)
			{
				return wgoData;
			}
		}
		return null;
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x0008D35C File Offset: 0x0008B55C
	private static bool IsGraveyardModule(string id)
	{
		return GameBalance.Me != null && GameBalance.Me.HasWgoIdByGroup("graveyard_modules", id);
	}

	// Token: 0x04001B8E RID: 7054
	private const float OccupiedGraveModuleRadius = 0.05f;

	// Token: 0x04001B8F RID: 7055
	[SerializeField]
	private string debugLabel;

	// Token: 0x04001B90 RID: 7056
	[SerializeField]
	private AssetReferenceGameObject contentDataRef;

	// Token: 0x04001B91 RID: 7057
	[SerializeField]
	private SGuid wgoUniqueId;
}
