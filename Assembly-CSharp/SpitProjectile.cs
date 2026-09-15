using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class SpitProjectile : Projectile, IDamageDealer
{
	// Token: 0x1400002D RID: 45
	// (add) Token: 0x0600167B RID: 5755 RVA: 0x0006C1DC File Offset: 0x0006A3DC
	// (remove) Token: 0x0600167C RID: 5756 RVA: 0x0006C214 File Offset: 0x0006A414
	public event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x0600167D RID: 5757 RVA: 0x0006C24C File Offset: 0x0006A44C
	// (remove) Token: 0x0600167E RID: 5758 RVA: 0x0006C284 File Offset: 0x0006A484
	public event Action OnMiss;

	// Token: 0x0600167F RID: 5759 RVA: 0x0006C2B9 File Offset: 0x0006A4B9
	public void Activate(AttackContext ctx)
	{
		this.attackContext = ctx;
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x00002318 File Offset: 0x00000518
	public void Cancel()
	{
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x0006C2C4 File Offset: 0x0006A4C4
	private void LateUpdate()
	{
		if (this.wasHit)
		{
			return;
		}
		if (this.hitBoxCollider == null)
		{
			return;
		}
		Vector3 vector = this.hitBoxCollider.transform.TransformPoint(this.hitBoxCollider.center);
		Quaternion rotation = this.hitBoxCollider.transform.rotation;
		if (this.hasPreviousPosition && SpecialPhysicsCastUtils.GetBoxLineSweepHits(this.hitBoxCollider, this.previousPosition, vector, rotation, this.sweepLayerMask, this.sweepHits, QueryTriggerInteraction.Collide))
		{
			foreach (RaycastHit raycastHit in this.sweepHits)
			{
				if (this.wasHit)
				{
					break;
				}
				this.ProcessSweepHit(raycastHit);
			}
		}
		this.previousPosition = vector;
		this.hasPreviousPosition = true;
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x0006C3A0 File Offset: 0x0006A5A0
	private void ProcessSweepHit(RaycastHit hit)
	{
		Collider collider = hit.collider;
		if (collider == null)
		{
			return;
		}
		if (collider.GetComponentInParent<SpitProjectile>())
		{
			return;
		}
		if (collider.gameObject.layer == 11)
		{
			this.HandleHit();
			LazyAudio.PlayAtPos("spitter_hit", hit.point);
			return;
		}
		DropSearcher dropSearcher;
		PlayerInteractionComponent playerInteractionComponent;
		SwordHitBox swordHitBox;
		if (collider.TryGetComponent<DropSearcher>(out dropSearcher) || collider.TryGetComponent<PlayerInteractionComponent>(out playerInteractionComponent) || collider.TryGetComponent<SwordHitBox>(out swordHitBox))
		{
			return;
		}
		ICombatEntity componentInParent = collider.GetComponentInParent<ICombatEntity>();
		if (componentInParent == null)
		{
			return;
		}
		WeaponHitState weaponHitState;
		if (!this.hitStates.TryGetValue(componentInParent, out weaponHitState))
		{
			weaponHitState = new WeaponHitState();
			this.hitStates[componentInParent] = weaponHitState;
		}
		HitZone hitZone;
		if (collider.TryGetComponent<HitZone>(out hitZone))
		{
			ValueTuple<HitResult, HitZoneReaction> valueTuple = this.ProcessHitZone(hitZone, weaponHitState, collider, hit.point);
			HitResult item = valueTuple.Item1;
			HitZoneReaction item2 = valueTuple.Item2;
			if (item.ShouldStopProjectile)
			{
				this.FinalizeHit(collider, componentInParent, 0f, hit.point);
				hitZone.PlayHitEffect(item, item2, hit.point);
				return;
			}
			if (item.type == HitResultType.ZoneMarked)
			{
				weaponHitState.MarkZonePassed(hitZone);
				return;
			}
			if (item.type == HitResultType.Absorbed)
			{
				return;
			}
			if (item.ShouldDealDamage)
			{
				this.FinalizeHit(collider, componentInParent, item.damageMultiplier, hit.point);
				hitZone.PlayHitEffect(item, item2, hit.point);
				return;
			}
		}
		else
		{
			this.FinalizeHit(collider, componentInParent, 1f, hit.point);
		}
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0006C500 File Offset: 0x0006A700
	[return: TupleElementNames(new string[] { "result", "reaction" })]
	private ValueTuple<HitResult, HitZoneReaction> ProcessHitZone(HitZone hitZone, WeaponHitState state, Collider other, Vector3 hitPoint)
	{
		if (state.HasPassedZone(hitZone))
		{
			return new ValueTuple<HitResult, HitZoneReaction>(HitResult.Absorbed(), null);
		}
		if (hitZone.ZoneType == HitZoneType.Body && state.IsInsideProtectedZone)
		{
			return new ValueTuple<HitResult, HitZoneReaction>(HitResult.Absorbed(), null);
		}
		return hitZone.ProcessHit(this.damageSourceType, this.attackContext, hitPoint);
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x0006C554 File Offset: 0x0006A754
	private void FinalizeHit(Collider other, ICombatEntity combatEntity, float damageMultiplier, Vector3 hitPoint)
	{
		global::UnityEngine.Object @object = combatEntity as global::UnityEngine.Object;
		if (@object == null || !@object)
		{
			return;
		}
		if (combatEntity.TeamType == this.attackContext.teamType)
		{
			return;
		}
		this.HandleHit();
		AttackContext attackContext = this.attackContext;
		ProjectileStats stats = base.Stats;
		this.attackContext = AttackContext.WithHit(attackContext, hitPoint, (stats != null) ? stats.damageEffectOverride : null);
		if (damageMultiplier.EqualsTo(0f, 1E-05f))
		{
			this.attackContext = AttackContext.WithCustomDamage(this.attackContext, 0);
		}
		else
		{
			this.TryPlayDamageHitFx(hitPoint);
		}
		Action<ICombatEntity, AttackContext> onHit = this.OnHit;
		if (onHit != null)
		{
			onHit(combatEntity, this.attackContext);
		}
		this.Despawn();
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x0006C600 File Offset: 0x0006A800
	private void TryPlayDamageHitFx(Vector3 hitPoint)
	{
		ProjectileStats stats = base.Stats;
		string text = ((stats != null) ? stats.onDamageHitFxName : null);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		WorldFX.Spawn(hitPoint, text, null, default(Vector3));
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x0006C63B File Offset: 0x0006A83B
	private void HandleHit()
	{
		this.wasHit = true;
		this.hitBoxCollider.enabled = false;
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x0006C650 File Offset: 0x0006A850
	private void OnEnable()
	{
		this.hitStates.Clear();
		this.hasPreviousPosition = false;
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x0006C650 File Offset: 0x0006A850
	private void OnDisable()
	{
		this.hitStates.Clear();
		this.hasPreviousPosition = false;
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x0006C664 File Offset: 0x0006A864
	protected override void Despawn()
	{
		this.hitBoxCollider.enabled = true;
		base.Despawn();
	}

	// Token: 0x040016CB RID: 5835
	[SerializeField]
	private DamageSourceType damageSourceType = DamageSourceType.Arrow;

	// Token: 0x040016CC RID: 5836
	[SerializeField]
	private BoxCollider hitBoxCollider;

	// Token: 0x040016CD RID: 5837
	[SerializeField]
	private int sweepLayerMask = -1;

	// Token: 0x040016CE RID: 5838
	private AttackContext attackContext;

	// Token: 0x040016CF RID: 5839
	private Vector3 previousPosition;

	// Token: 0x040016D0 RID: 5840
	private bool hasPreviousPosition;

	// Token: 0x040016D1 RID: 5841
	private readonly List<RaycastHit> sweepHits = new List<RaycastHit>();

	// Token: 0x040016D2 RID: 5842
	private readonly Dictionary<ICombatEntity, WeaponHitState> hitStates = new Dictionary<ICombatEntity, WeaponHitState>();
}
