using System;

// Token: 0x02000577 RID: 1399
[Serializable]
public class ConveyorMovableItemData
{
	// Token: 0x060023CB RID: 9163 RVA: 0x000A7A8C File Offset: 0x000A5C8C
	public ConveyorMovableItemData(string itemId, int rotationIndex, bool isCommon)
	{
		this.itemId = itemId;
		switch (rotationIndex)
		{
		case 0:
			this.direction = Direction.Down;
			break;
		case 1:
			this.direction = Direction.Left;
			break;
		case 2:
			this.direction = Direction.Up;
			break;
		case 3:
			this.direction = Direction.Right;
			break;
		}
		this.isCommon = isCommon;
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x000A7AE7 File Offset: 0x000A5CE7
	public ConveyorMovableItemData(string itemId, Direction direction, bool isCommon)
	{
		this.itemId = itemId;
		this.direction = direction;
		this.isCommon = isCommon;
	}

	// Token: 0x04001FE9 RID: 8169
	public Direction direction;

	// Token: 0x04001FEA RID: 8170
	public string itemId;

	// Token: 0x04001FEB RID: 8171
	public bool isCommon;
}
