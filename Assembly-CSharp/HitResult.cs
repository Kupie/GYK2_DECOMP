using System;

// Token: 0x0200034A RID: 842
public readonly struct HitResult
{
	// Token: 0x06001645 RID: 5701 RVA: 0x0006B819 File Offset: 0x00069A19
	private HitResult(HitResultType type, float damageMultiplier = 1f)
	{
		this.type = type;
		this.damageMultiplier = damageMultiplier;
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x0006B829 File Offset: 0x00069A29
	public static HitResult Pass(float multiplier = 1f)
	{
		return new HitResult(HitResultType.Pass, multiplier);
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x0006B832 File Offset: 0x00069A32
	public static HitResult Blocked()
	{
		return new HitResult(HitResultType.Blocked, 0f);
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x0006B83F File Offset: 0x00069A3F
	public static HitResult Absorbed()
	{
		return new HitResult(HitResultType.Absorbed, 0f);
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x0006B84C File Offset: 0x00069A4C
	public static HitResult Deflected()
	{
		return new HitResult(HitResultType.Deflected, 0f);
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x0006B859 File Offset: 0x00069A59
	public static HitResult ZoneMarked()
	{
		return new HitResult(HitResultType.ZoneMarked, 1f);
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x0600164B RID: 5707 RVA: 0x0006B866 File Offset: 0x00069A66
	public bool ShouldDealDamage
	{
		get
		{
			return this.type == HitResultType.Pass && this.damageMultiplier > 0f;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x0600164C RID: 5708 RVA: 0x0006B880 File Offset: 0x00069A80
	public bool ShouldStopProjectile
	{
		get
		{
			HitResultType hitResultType = this.type;
			return hitResultType == HitResultType.Blocked || hitResultType == HitResultType.Deflected;
		}
	}

	// Token: 0x04001699 RID: 5785
	public readonly HitResultType type;

	// Token: 0x0400169A RID: 5786
	public readonly float damageMultiplier;
}
