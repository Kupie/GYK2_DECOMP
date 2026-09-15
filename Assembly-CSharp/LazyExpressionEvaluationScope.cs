using System;

// Token: 0x0200023E RID: 574
public static class LazyExpressionEvaluationScope
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0004C0CA File Offset: 0x0004A2CA
	// (set) Token: 0x06000E6C RID: 3692 RVA: 0x0004C0D6 File Offset: 0x0004A2D6
	public static WgoData WgoData
	{
		get
		{
			return LazyExpressionEvaluationScope.Context.WgoData;
		}
		set
		{
			LazyExpressionEvaluationScope.Context.WgoData = value;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0004C0E3 File Offset: 0x0004A2E3
	// (set) Token: 0x06000E6E RID: 3694 RVA: 0x0004C0EF File Offset: 0x0004A2EF
	public static Item Item
	{
		get
		{
			return LazyExpressionEvaluationScope.Context.Item;
		}
		set
		{
			LazyExpressionEvaluationScope.Context.Item = value;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0004C0FC File Offset: 0x0004A2FC
	// (set) Token: 0x06000E70 RID: 3696 RVA: 0x0004C108 File Offset: 0x0004A308
	public static WorldZoneData WorldZoneData
	{
		get
		{
			return LazyExpressionEvaluationScope.Context.WorldZoneData;
		}
		set
		{
			LazyExpressionEvaluationScope.Context.WorldZoneData = value;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0004C115 File Offset: 0x0004A315
	// (set) Token: 0x06000E72 RID: 3698 RVA: 0x0004C121 File Offset: 0x0004A321
	public static ICombatEntity CombatEntity
	{
		get
		{
			return LazyExpressionEvaluationScope.Context.CombatEntity;
		}
		set
		{
			LazyExpressionEvaluationScope.Context.CombatEntity = value;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0004C12E File Offset: 0x0004A32E
	// (set) Token: 0x06000E74 RID: 3700 RVA: 0x0004C13A File Offset: 0x0004A33A
	public static ICombatEntity CombatEntityAttacksMe
	{
		get
		{
			return LazyExpressionEvaluationScope.Context.CombatEntityAttacksMe;
		}
		set
		{
			LazyExpressionEvaluationScope.Context.CombatEntityAttacksMe = value;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0004C147 File Offset: 0x0004A347
	// (set) Token: 0x06000E76 RID: 3702 RVA: 0x0004C153 File Offset: 0x0004A353
	public static int DeltaValue
	{
		get
		{
			return LazyExpressionEvaluationScope.Context.DeltaValue;
		}
		set
		{
			LazyExpressionEvaluationScope.Context.DeltaValue = value;
		}
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0004C160 File Offset: 0x0004A360
	public static void Begin(LazyExpressionContext context, Action customCallback = null)
	{
		LazyExpressionEvaluationScope.Context = context;
		LazyExpressionEvaluationScope.CustomCallback = customCallback;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0004C16E File Offset: 0x0004A36E
	public static void End()
	{
		LazyExpressionEvaluationScope.Context = default(LazyExpressionContext);
		LazyExpressionEvaluationScope.CustomCallback = null;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0004C184 File Offset: 0x0004A384
	public static bool AnyOverheadItem(Predicate<Item> match)
	{
		if (match == null)
		{
			return false;
		}
		if (LazyExpressionEvaluationScope.OverheadCandidate != null && !LazyExpressionEvaluationScope.OverheadCandidate.IsEmpty)
		{
			return match(LazyExpressionEvaluationScope.OverheadCandidate);
		}
		PlayerData playerData = MainGame.PlayerData;
		Item item;
		return playerData != null && playerData.TryGetOverheadItem(match, out item);
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0004C1C9 File Offset: 0x0004A3C9
	public static Item GetOverheadItemForMutation()
	{
		if (LazyExpressionEvaluationScope.OverheadCandidate != null && !LazyExpressionEvaluationScope.OverheadCandidate.IsEmpty)
		{
			return LazyExpressionEvaluationScope.OverheadCandidate;
		}
		PlayerData playerData = MainGame.PlayerData;
		if (playerData == null)
		{
			return null;
		}
		return playerData.overheadItem;
	}

	// Token: 0x04001166 RID: 4454
	public static LazyExpressionContext Context;

	// Token: 0x04001167 RID: 4455
	public static Action CustomCallback;

	// Token: 0x04001168 RID: 4456
	public static Item OverheadCandidate;
}
