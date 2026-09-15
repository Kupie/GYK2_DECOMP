using System;

// Token: 0x020005B3 RID: 1459
[Serializable]
public class SpawnDelayedObject
{
	// Token: 0x060025A6 RID: 9638 RVA: 0x00021B94 File Offset: 0x0001FD94
	public SpawnDelayedObject()
	{
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000B0664 File Offset: 0x000AE864
	public SpawnDelayedObject(CraftElement craftElement, WgoData wgoData)
	{
		this.craftElement = craftElement;
		this.wgoUniqueId = SGuid.Empty;
		this.wgoUniqueId.SetGuid(wgoData.UniqueId);
	}

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000B0690 File Offset: 0x000AE890
	public WgoData ResolvedWgoData
	{
		get
		{
			SGuid uniqueId = this.wgoUniqueId;
			if (SGuid.IsNullOrEmpty(uniqueId) && this.wgoData != null)
			{
				uniqueId = this.wgoData.UniqueId;
			}
			if (SGuid.IsNullOrEmpty(uniqueId))
			{
				return null;
			}
			return MainGame.Instance.GameSave.worldData.GetWgoData(uniqueId);
		}
	}

	// Token: 0x040020E2 RID: 8418
	public CraftElement craftElement;

	// Token: 0x040020E3 RID: 8419
	public WgoData wgoData;

	// Token: 0x040020E4 RID: 8420
	public SGuid wgoUniqueId;
}
