using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class PikeHitBox : MonoBehaviour, IDamageDealer
{
	// Token: 0x1400002F RID: 47
	// (add) Token: 0x0600168B RID: 5771 RVA: 0x0006C6A4 File Offset: 0x0006A8A4
	// (remove) Token: 0x0600168C RID: 5772 RVA: 0x0006C6DC File Offset: 0x0006A8DC
	public event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x0600168D RID: 5773 RVA: 0x0006C714 File Offset: 0x0006A914
	// (remove) Token: 0x0600168E RID: 5774 RVA: 0x0006C74C File Offset: 0x0006A94C
	public event Action OnMiss;

	// Token: 0x0600168F RID: 5775 RVA: 0x0006C781 File Offset: 0x0006A981
	public void Activate(AttackContext ctx)
	{
		this.attackContext = ctx;
		this.isActivated = true;
		this.hitStates.Clear();
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x00002318 File Offset: 0x00000518
	public void Cancel()
	{
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0006C79C File Offset: 0x0006A99C
	public void SweepForTargets(Vector3 origin, Vector3 direction, float length, float radius)
	{
		if (!this.isActivated || direction.sqrMagnitude < 0.0001f || length <= 0f || radius <= 0f)
		{
			return;
		}
		Vector3 normalized = direction.normalized;
		Vector3 vector = origin + normalized * length;
		int num = Physics.OverlapCapsuleNonAlloc(origin, vector, radius, PikeHitBox.sweepHits, this.sweepMask, QueryTriggerInteraction.Collide);
		for (int i = 0; i < num; i++)
		{
			Collider collider = PikeHitBox.sweepHits[i];
			if (collider)
			{
				this.TryProcessCollider(collider);
			}
		}
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0006C828 File Offset: 0x0006AA28
	private void OnTriggerEnter(Collider other)
	{
		this.TryProcessCollider(other);
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0006C834 File Offset: 0x0006AA34
	private void TryProcessCollider(Collider other)
	{
		if (!this.isActivated || other == null)
		{
			return;
		}
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent == null)
		{
			return;
		}
		if (componentInParent.TeamType == this.attackContext.teamType)
		{
			return;
		}
		WeaponHitState weaponHitState;
		if (!this.hitStates.TryGetValue(componentInParent, out weaponHitState))
		{
			weaponHitState = new WeaponHitState();
			this.hitStates[componentInParent] = weaponHitState;
		}
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
		if (weaponHitState.DamageDealt && hitResult.ShouldDealDamage)
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
		if (hitZone != null)
		{
			hitZone.PlayHitEffect(hitResult, item, this.attackContext.hitPosition);
		}
		if (hitResult.ShouldDealDamage)
		{
			weaponHitState.MarkDamageDealt();
		}
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0006C9D8 File Offset: 0x0006ABD8
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

	// Token: 0x06001695 RID: 5781 RVA: 0x0006CA44 File Offset: 0x0006AC44
	private bool IsBackstabAgainstDoor(HitZone hitZone)
	{
		AttackComponent attackersAttackComponent = this.attackContext.attackersAttackComponent;
		AnimationComponent animationComponent = ((attackersAttackComponent != null) ? attackersAttackComponent.animationComponent : null);
		AnimationComponentBase animationComponent2 = hitZone.AnimationComponent;
		return !(animationComponent == null) && !(animationComponent2 == null) && animationComponent.GetDirection() == animationComponent2.GetDirection();
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0006CA95 File Offset: 0x0006AC95
	private void OnEnable()
	{
		this.hitStates.Clear();
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x0006CAA2 File Offset: 0x0006ACA2
	private void OnDisable()
	{
		this.isActivated = false;
		this.hitStates.Clear();
	}

	// Token: 0x040016D5 RID: 5845
	private static readonly Collider[] sweepHits = new Collider[32];

	// Token: 0x040016D6 RID: 5846
	[SerializeField]
	private DamageSourceType damageSourceType = DamageSourceType.Spear;

	// Token: 0x040016D7 RID: 5847
	[SerializeField]
	private LayerMask sweepMask = 536871937;

	// Token: 0x040016D8 RID: 5848
	private AttackContext attackContext;

	// Token: 0x040016D9 RID: 5849
	private bool isActivated;

	// Token: 0x040016DA RID: 5850
	private readonly Dictionary<ICombatEntity, WeaponHitState> hitStates = new Dictionary<ICombatEntity, WeaponHitState>();
}
