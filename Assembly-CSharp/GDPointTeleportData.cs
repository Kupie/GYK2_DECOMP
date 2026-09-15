using System;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class GDPointTeleportData : TeleportDataBase
{
	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x0600199A RID: 6554 RVA: 0x00078FE4 File Offset: 0x000771E4
	public GDPointData GDPointData
	{
		get
		{
			if (this.gdPoint != null)
			{
				return this.gdPoint;
			}
			if (!this.isTag)
			{
				return MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.gdPointStr);
			}
			return MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataByCustomTag(this.gdPointStr);
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x00079047 File Offset: 0x00077247
	public GDPointTeleportData(GDPointData gdPoint, string environmentPreset = "outdoor", string soundOnTeleport = "", Action onGameSceneLoaded = null, bool donNotFade = false, float delayInFade = 0.3f)
		: base(environmentPreset, soundOnTeleport, onGameSceneLoaded, donNotFade, delayInFade)
	{
		this.gdPoint = gdPoint;
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x0007905E File Offset: 0x0007725E
	public GDPointTeleportData(string gdPointStr, bool isTag = false, string environmentPreset = "outdoor", string soundOnTeleport = "", Action onGameSceneLoaded = null, bool donNotFade = false, float delayInFade = 0.3f)
		: base(environmentPreset, soundOnTeleport, onGameSceneLoaded, donNotFade, delayInFade)
	{
		this.gdPointStr = gdPointStr;
		this.isTag = isTag;
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x0007907D File Offset: 0x0007727D
	public override string GetDestinationId()
	{
		return this.GDPointData.Id;
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x0007908A File Offset: 0x0007728A
	public override GameSceneData GetDestinationSceneData()
	{
		return MainGame.Instance.GameSave.worldData.GetGameSceneDataById(this.GDPointData.GameSceneDataId);
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x000790AB File Offset: 0x000772AB
	public override Vector3 GetPosition()
	{
		return this.GDPointData.Position;
	}

	// Token: 0x040018EA RID: 6378
	private GDPointData gdPoint;

	// Token: 0x040018EB RID: 6379
	private string gdPointStr;

	// Token: 0x040018EC RID: 6380
	private bool isTag;
}
