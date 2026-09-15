using System;

// Token: 0x0200056E RID: 1390
[Serializable]
public class ConveyorChestSlotData
{
	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06002398 RID: 9112 RVA: 0x000A74C4 File Offset: 0x000A56C4
	public ConveyorWgoData ConveyorWgoData
	{
		get
		{
			if (this.conveyorWgoData != null)
			{
				return this.conveyorWgoData;
			}
			if (this.conveyorWgoDataUniqueId == null)
			{
				return null;
			}
			this.conveyorWgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.conveyorWgoDataUniqueId) as ConveyorWgoData;
			return this.conveyorWgoData;
		}
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000A751B File Offset: 0x000A571B
	public ConveyorChestSlotData(Direction slotPosDirection)
	{
		this.slotPosDirection = slotPosDirection;
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000A7531 File Offset: 0x000A5731
	public ConveyorChestSlotData(Direction slotPosDirection, int slotIndex)
		: this(slotPosDirection)
	{
		this.slotIndex = slotIndex;
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000A7541 File Offset: 0x000A5741
	public void Clear()
	{
		this.conveyorWgoDataUniqueId = null;
		this.slotItemId = string.Empty;
		this.conveyorWgoData = null;
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000A755C File Offset: 0x000A575C
	public void SetItemSlotId(string itemSlotId)
	{
		this.slotItemId = itemSlotId;
	}

	// Token: 0x04001FC0 RID: 8128
	public SGuid conveyorWgoDataUniqueId;

	// Token: 0x04001FC1 RID: 8129
	public string slotItemId;

	// Token: 0x04001FC2 RID: 8130
	public Direction slotPosDirection;

	// Token: 0x04001FC3 RID: 8131
	public int slotIndex = -1;

	// Token: 0x04001FC4 RID: 8132
	private ConveyorWgoData conveyorWgoData;
}
