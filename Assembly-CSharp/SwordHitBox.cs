using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000358 RID: 856
public class SwordHitBox : MonoBehaviour, IDamageDealer
{
	// Token: 0x14000031 RID: 49
	// (add) Token: 0x0600169A RID: 5786 RVA: 0x0006CAF0 File Offset: 0x0006ACF0
	// (remove) Token: 0x0600169B RID: 5787 RVA: 0x0006CB28 File Offset: 0x0006AD28
	public event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x0600169C RID: 5788 RVA: 0x0006CB60 File Offset: 0x0006AD60
	// (remove) Token: 0x0600169D RID: 5789 RVA: 0x0006CB98 File Offset: 0x0006AD98
	public event Action OnMiss;

	// Token: 0x0600169E RID: 5790 RVA: 0x0006CBCD File Offset: 0x0006ADCD
	public void Activate(AttackContext ctx)
	{
		this.attackContext = ctx;
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x00002318 File Offset: 0x00000518
	public void Cancel()
	{
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x0006CBD8 File Offset: 0x0006ADD8
	private void OnTriggerEnter(Collider other)
	{
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent == null || !(componentInParent is global::UnityEngine.Object))
		{
			return;
		}
		if (componentInParent.TeamType == this.attackContext.teamType)
		{
			return;
		}
		WeaponHitState weaponHitState;
		if (!this.hitStatesAccumulator.hitStates.TryGetValue(componentInParent, out weaponHitState))
		{
			weaponHitState = new WeaponHitState();
			this.hitStatesAccumulator.hitStates[componentInParent] = weaponHitState;
		}
		if (this.hitStatesAccumulator.hitColliders.Contains(other))
		{
			return;
		}
		this.hitStatesAccumulator.hitColliders.Add(other);
		float num = 1f;
		HitResult hitResult = default(HitResult);
		HitZone hitZone;
		if (!other.TryGetComponent<HitZone>(out hitZone))
		{
			return;
		}
		HitZoneType zoneType = hitZone.ZoneType;
		if ((zoneType == HitZoneType.Door || zoneType == HitZoneType.DoorFrontArea) && this.IsBackstabAgainstDoor(hitZone))
		{
			return;
		}
		ValueTuple<HitResult, HitZoneReaction> valueTuple = this.ProcessHitZone(hitZone, weaponHitState, other);
		hitResult = valueTuple.Item1;
		HitZoneReaction item = valueTuple.Item2;
		if (weaponHitState.DamageDealt && hitResult.ShouldStopProjectile)
		{
			return;
		}
		if (hitResult.type == HitResultType.ZoneMarked)
		{
			if (!weaponHitState.DamageDealt)
			{
				weaponHitState.MarkZonePassed(hitZone);
			}
			return;
		}
		if (hitResult.type == HitResultType.Absorbed)
		{
			return;
		}
		if (hitResult.ShouldStopProjectile)
		{
			num = 0f;
		}
		else if (hitResult.ShouldDealDamage)
		{
			num = hitResult.damageMultiplier;
		}
		Vector3 contactPosition = other.GetContactPosition(base.transform.position, 0.5f);
		this.attackContext = new AttackContext(this.attackContext, contactPosition);
		if (num == 0f)
		{
			this.attackContext = new AttackContext(this.attackContext, 0);
		}
		Action<ICombatEntity, AttackContext> onHit = this.OnHit;
		if (onHit != null)
		{
			onHit(componentInParent, this.attackContext);
		}
		hitZone.PlayHitEffect(hitResult, item, this.attackContext.hitPosition);
		if (hitResult.ShouldDealDamage)
		{
			weaponHitState.MarkDamageDealt();
		}
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x0006CD8C File Offset: 0x0006AF8C
	[return: TupleElementNames(new string[] { "result", "reaction" })]
	private ValueTuple<HitResult, HitZoneReaction> ProcessHitZone(HitZone hitZone, WeaponHitState state, Collider other)
	{
		if (state.HasPassedZone(hitZone))
		{
			return new ValueTuple<HitResult, HitZoneReaction>(HitResult.Absorbed(), null);
		}
		if (hitZone.ZoneType == HitZoneType.Body && state.IsInsideProtectedZone)
		{
			return new ValueTuple<HitResult, HitZoneReaction>(HitResult.Absorbed(), null);
		}
		Vector3 contactPosition = other.GetContactPosition(base.transform.position, 0.5f);
		return hitZone.ProcessHit(this.damageSourceType, this.attackContext, contactPosition);
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x0006CDF8 File Offset: 0x0006AFF8
	private bool IsBackstabAgainstDoor(HitZone hitZone)
	{
		AttackComponent attackersAttackComponent = this.attackContext.attackersAttackComponent;
		AnimationComponent animationComponent = ((attackersAttackComponent != null) ? attackersAttackComponent.animationComponent : null);
		AnimationComponentBase animationComponent2 = hitZone.AnimationComponent;
		return !(animationComponent == null) && !(animationComponent2 == null) && animationComponent.GetDirection() == animationComponent2.GetDirection();
	}

	// Token: 0x040016DD RID: 5853
	[SerializeField]
	private DamageSourceType damageSourceType = DamageSourceType.Sword;

	// Token: 0x040016DE RID: 5854
	[SerializeField]
	private HitStatesAccumulator hitStatesAccumulator;

	// Token: 0x040016DF RID: 5855
	private AttackContext attackContext;
}
