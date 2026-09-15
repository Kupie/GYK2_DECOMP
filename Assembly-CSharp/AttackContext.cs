using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x020002AD RID: 685
public readonly struct AttackContext
{
	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x0600117D RID: 4477 RVA: 0x00058188 File Offset: 0x00056388
	public int Damage
	{
		get
		{
			if (this.hasCustomDamage)
			{
				return this.customDamage;
			}
			FighterDef fighterDef = this.fighterDef;
			if (fighterDef != null)
			{
				return fighterDef.atkDamage.EvaluateInt(this.attacker);
			}
			ItemDef itemDef = this.weaponDef;
			if (itemDef == null)
			{
				return 0;
			}
			return itemDef.damage.EvaluateInt(this.attacker);
		}
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x000581DC File Offset: 0x000563DC
	public AttackContext(ICombatEntity attacker, LazyConsts.Fighting.TeamType teamType, FighterDef fighterDef, ItemDef weaponDef, Vector3 origin, Vector3 direction, Vector3 hitPosition = default(Vector3), int customDamage = -1, bool isReturnDamage = false, AttackComponent attackersAttackComponent = null, DamageEffectSettings damageEffectOverride = null)
	{
		this.attacker = attacker;
		this.teamType = teamType;
		this.fighterDef = fighterDef;
		this.weaponDef = weaponDef;
		this.origin = origin;
		this.direction = direction.normalized;
		this.hitPosition = hitPosition;
		this.hasCustomDamage = customDamage >= 0;
		this.customDamage = customDamage;
		this.isReturnDamage = isReturnDamage;
		this.attackersAttackComponent = attackersAttackComponent;
		this.damageEffectOverride = damageEffectOverride;
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x00058254 File Offset: 0x00056454
	public AttackContext(ICombatEntity attacker, LazyConsts.Fighting.TeamType teamType, FighterDef fighterDef, ItemDef weaponDef, Vector3 origin, Vector3 direction, Vector3 hitPosition)
	{
		this = new AttackContext(attacker, teamType, fighterDef, weaponDef, origin, direction, hitPosition, -1, false, null, null);
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x00058278 File Offset: 0x00056478
	public AttackContext(AttackContext context, Vector3 hitPosition)
	{
		this = new AttackContext(context.attacker, context.teamType, context.fighterDef, context.weaponDef, context.origin, context.direction, hitPosition, context.hasCustomDamage ? context.customDamage : (-1), context.isReturnDamage, context.attackersAttackComponent, context.damageEffectOverride);
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x000582D4 File Offset: 0x000564D4
	public AttackContext(AttackContext context, int customDamage)
	{
		this = new AttackContext(context.attacker, context.teamType, context.fighterDef, context.weaponDef, context.origin, context.direction, context.hitPosition, customDamage, context.isReturnDamage, context.attackersAttackComponent, context.damageEffectOverride);
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00058324 File Offset: 0x00056524
	public static AttackContext WithHit(AttackContext context, Vector3 hitPosition, DamageEffectSettings damageEffectOverride = null)
	{
		return new AttackContext(context.attacker, context.teamType, context.fighterDef, context.weaponDef, context.origin, context.direction, hitPosition, context.hasCustomDamage ? context.customDamage : (-1), context.isReturnDamage, context.attackersAttackComponent, damageEffectOverride ?? context.damageEffectOverride);
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x00058384 File Offset: 0x00056584
	public static AttackContext WithCustomDamage(AttackContext context, int customDamage)
	{
		return new AttackContext(context.attacker, context.teamType, context.fighterDef, context.weaponDef, context.origin, context.direction, context.hitPosition, customDamage, context.isReturnDamage, context.attackersAttackComponent, context.damageEffectOverride);
	}

	// Token: 0x04001365 RID: 4965
	public readonly ICombatEntity attacker;

	// Token: 0x04001366 RID: 4966
	public readonly LazyConsts.Fighting.TeamType teamType;

	// Token: 0x04001367 RID: 4967
	[CanBeNull]
	public readonly ItemDef weaponDef;

	// Token: 0x04001368 RID: 4968
	[CanBeNull]
	public readonly FighterDef fighterDef;

	// Token: 0x04001369 RID: 4969
	public readonly Vector3 origin;

	// Token: 0x0400136A RID: 4970
	public readonly Vector3 direction;

	// Token: 0x0400136B RID: 4971
	public readonly Vector3 hitPosition;

	// Token: 0x0400136C RID: 4972
	public readonly bool hasCustomDamage;

	// Token: 0x0400136D RID: 4973
	public readonly int customDamage;

	// Token: 0x0400136E RID: 4974
	public readonly bool isReturnDamage;

	// Token: 0x0400136F RID: 4975
	[CanBeNull]
	public readonly AttackComponent attackersAttackComponent;

	// Token: 0x04001370 RID: 4976
	[CanBeNull]
	public readonly DamageEffectSettings damageEffectOverride;
}
