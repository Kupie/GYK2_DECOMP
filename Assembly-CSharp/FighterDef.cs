using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020001E6 RID: 486
[Serializable]
public class FighterDef : BalanceBaseObject
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0003E08D File Offset: 0x0003C28D
	public bool HasKnockback
	{
		get
		{
			return this.knockbackForce.HasExpression;
		}
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0003E09C File Offset: 0x0003C29C
	public DockPointTag TargetFilterDockPointTag(WgoData wgoData)
	{
		ZombieWgoData zombieWgoData = wgoData as ZombieWgoData;
		Item item;
		if (zombieWgoData != null)
		{
			item = zombieWgoData.Hand;
			if (!item.IsEmpty && (item.Definition.type == ItemType.Bow || item.Definition.type == ItemType.Pike))
			{
				return (DockPointTag)this.targetFilterDockPointTag.EvaluateInt(item);
			}
		}
		item = wgoData.Inventory.GetItemByType(ItemType.Bow);
		if (item == null || item.IsEmpty)
		{
			item = wgoData.Inventory.GetItemByType(ItemType.Pike);
		}
		if (item != null && !item.IsEmpty)
		{
			return (DockPointTag)this.targetFilterDockPointTag.EvaluateInt(item);
		}
		return DockPointTag.None;
	}

	// Token: 0x04000DBD RID: 3517
	[AutoParse("hp")]
	public LazyExpression hp;

	// Token: 0x04000DBE RID: 3518
	[AutoParse("atk_damage")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression atkDamage = new LazyExpression();

	// Token: 0x04000DBF RID: 3519
	[AutoParse("atk_range")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression atkRange = new LazyExpression();

	// Token: 0x04000DC0 RID: 3520
	[AutoParse("atk_pause")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression atkPause = new LazyExpression();

	// Token: 0x04000DC1 RID: 3521
	[AutoParse("armor")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression armor = new LazyExpression();

	// Token: 0x04000DC2 RID: 3522
	[AutoParse("ret_dmg")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression retDamage = new LazyExpression();

	// Token: 0x04000DC3 RID: 3523
	[AutoParse("knockback_force")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression knockbackForce = new LazyExpression();

	// Token: 0x04000DC4 RID: 3524
	[AutoParse("movement_speed")]
	public float mvtSpeed;

	// Token: 0x04000DC5 RID: 3525
	[AutoParse("movement_acc")]
	public float mvtAcceleration;

	// Token: 0x04000DC6 RID: 3526
	[AutoParse("kill_xp")]
	public int killXp;

	// Token: 0x04000DC7 RID: 3527
	[AutoParse("target_filter_dock_point_tag")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression targetFilterDockPointTag = new LazyExpression();

	// Token: 0x04000DC8 RID: 3528
	[AutoParse("expressions_on_combat_death")]
	public List<LazyExpression> expressionsOnCombatDeath = new List<LazyExpression>();
}
