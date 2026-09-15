using System;
using UnityEngine;

// Token: 0x0200023D RID: 573
public struct LazyExpressionContext
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0004BFA4 File Offset: 0x0004A1A4
	public static LazyExpressionContext Empty
	{
		get
		{
			return default(LazyExpressionContext);
		}
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0004BFBC File Offset: 0x0004A1BC
	public static LazyExpressionContext From(WgoData wgoData)
	{
		return new LazyExpressionContext
		{
			WgoData = wgoData
		};
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0004BFDC File Offset: 0x0004A1DC
	public static LazyExpressionContext From(Item item)
	{
		return new LazyExpressionContext
		{
			Item = item
		};
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0004BFFC File Offset: 0x0004A1FC
	public static LazyExpressionContext From(WorldZoneData worldZoneData)
	{
		return new LazyExpressionContext
		{
			WorldZoneData = worldZoneData
		};
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x0004C01C File Offset: 0x0004A21C
	public static LazyExpressionContext From(ICombatEntity combatEntity)
	{
		LazyExpressionContext lazyExpressionContext = new LazyExpressionContext
		{
			CombatEntity = combatEntity
		};
		global::UnityEngine.Object @object = combatEntity as global::UnityEngine.Object;
		if (@object != null)
		{
			Wgo wgo = @object as Wgo;
			if (wgo != null && wgo)
			{
				lazyExpressionContext.WgoData = wgo.Data;
			}
		}
		return lazyExpressionContext;
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x0004C068 File Offset: 0x0004A268
	public static LazyExpressionContext From(ICraftable craftable)
	{
		return new LazyExpressionContext
		{
			WgoData = (craftable as WgoData)
		};
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0004C08C File Offset: 0x0004A28C
	public static LazyExpressionContext WithAttackerAttacksMe(ICombatEntity attacker)
	{
		return new LazyExpressionContext
		{
			CombatEntityAttacksMe = attacker
		};
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x0004C0AC File Offset: 0x0004A2AC
	public static LazyExpressionContext ValueDelta(int valueDelta)
	{
		return new LazyExpressionContext
		{
			DeltaValue = valueDelta
		};
	}

	// Token: 0x04001160 RID: 4448
	public WgoData WgoData;

	// Token: 0x04001161 RID: 4449
	public Item Item;

	// Token: 0x04001162 RID: 4450
	public WorldZoneData WorldZoneData;

	// Token: 0x04001163 RID: 4451
	public ICombatEntity CombatEntity;

	// Token: 0x04001164 RID: 4452
	public ICombatEntity CombatEntityAttacksMe;

	// Token: 0x04001165 RID: 4453
	public int DeltaValue;
}
