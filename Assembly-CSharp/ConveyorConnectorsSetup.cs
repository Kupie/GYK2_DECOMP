using System;

// Token: 0x0200021D RID: 541
[Serializable]
public class ConveyorConnectorsSetup : IAutoParsable
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00040CA8 File Offset: 0x0003EEA8
	public bool IsEmpty
	{
		get
		{
			return this.slotsCount <= 0 || this.direction == Direction.None;
		}
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x00021B94 File Offset: 0x0001FD94
	public ConveyorConnectorsSetup()
	{
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00040CBE File Offset: 0x0003EEBE
	public ConveyorConnectorsSetup(Direction direction, int slotsCount)
	{
		this.direction = direction;
		this.slotsCount = slotsCount;
	}

	// Token: 0x04000F71 RID: 3953
	public Direction direction;

	// Token: 0x04000F72 RID: 3954
	public int slotsCount;
}
