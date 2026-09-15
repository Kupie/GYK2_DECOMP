using System;

// Token: 0x020004B5 RID: 1205
[Serializable]
public class RiverFloatingDropData
{
	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06002013 RID: 8211 RVA: 0x00097FE0 File Offset: 0x000961E0
	public SGuid UniqueId
	{
		get
		{
			if (this.item == null)
			{
				return SGuid.Empty;
			}
			return this.item.UniqueId;
		}
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x00097FFC File Offset: 0x000961FC
	public float GetCurrentSplineLength()
	{
		if (this.splineWorldLengths == null || this.splineIndex < 0 || this.splineIndex >= this.splineWorldLengths.Length)
		{
			return 1f;
		}
		float num = this.splineWorldLengths[this.splineIndex];
		if (num <= 0.0001f)
		{
			return 1f;
		}
		return num;
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x0009804D File Offset: 0x0009624D
	public float GetCurrentSpeed()
	{
		if (this.splineIndex > 0)
		{
			return this.fallSpeed;
		}
		return this.flowSpeed;
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x00098065 File Offset: 0x00096265
	public bool HasMoreSplinesAfterCurrent()
	{
		return this.splineWorldLengths != null && this.splineIndex + 1 < this.splineWorldLengths.Length;
	}

	// Token: 0x04001CBD RID: 7357
	public Item item;

	// Token: 0x04001CBE RID: 7358
	public SGuid wgoUniqueId;

	// Token: 0x04001CBF RID: 7359
	public string worldId;

	// Token: 0x04001CC0 RID: 7360
	public int splineIndex;

	// Token: 0x04001CC1 RID: 7361
	public float t;

	// Token: 0x04001CC2 RID: 7362
	public float[] splineWorldLengths;

	// Token: 0x04001CC3 RID: 7363
	public float flowSpeed;

	// Token: 0x04001CC4 RID: 7364
	public float fallSpeed;
}
