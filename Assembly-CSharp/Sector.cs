using System;

// Token: 0x02000251 RID: 593
public struct Sector
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0004E10C File Offset: 0x0004C30C
	// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0004E114 File Offset: 0x0004C314
	public float AngleL { readonly get; private set; }

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0004E11D File Offset: 0x0004C31D
	// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0004E125 File Offset: 0x0004C325
	public float AngleG { readonly get; private set; }

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0004E12E File Offset: 0x0004C32E
	// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0004E136 File Offset: 0x0004C336
	public Sector.RoundType RoundApproach { readonly get; private set; }

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0004E13F File Offset: 0x0004C33F
	// (set) Token: 0x06000F1A RID: 3866 RVA: 0x0004E147 File Offset: 0x0004C347
	public Direction Direction { readonly get; private set; }

	// Token: 0x06000F1B RID: 3867 RVA: 0x0004E150 File Offset: 0x0004C350
	public Sector(float angleL, float angleG, Sector.RoundType roundType, Direction direction)
	{
		this.AngleL = angleL;
		this.AngleG = angleG;
		this.RoundApproach = roundType;
		this.Direction = direction;
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0004E16F File Offset: 0x0004C36F
	public Sector(float angleL, float angleG)
	{
		this.AngleL = angleL;
		this.AngleG = angleG;
		this.RoundApproach = Sector.RoundType.GetLower;
		this.Direction = Direction.None;
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0004E190 File Offset: 0x0004C390
	public float GetClosestAngleByGiven(float directionAngle)
	{
		if (((this.AngleG + this.AngleL) / 2f).EqualsTo(directionAngle, 1E-05f))
		{
			Sector.RoundType roundApproach = this.RoundApproach;
			if (roundApproach == Sector.RoundType.GetLower)
			{
				return this.AngleL;
			}
			if (roundApproach == Sector.RoundType.GetGreater)
			{
				return this.AngleG;
			}
		}
		if (this.AngleG - directionAngle > directionAngle - this.AngleL)
		{
			return this.AngleL;
		}
		return this.AngleG;
	}

	// Token: 0x02000252 RID: 594
	public enum RoundType
	{
		// Token: 0x040011FA RID: 4602
		GetLower,
		// Token: 0x040011FB RID: 4603
		GetGreater
	}
}
