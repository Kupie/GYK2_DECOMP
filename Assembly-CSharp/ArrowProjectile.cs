using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class ArrowProjectile : Projectile, IDamageDealer
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600166B RID: 5739 RVA: 0x0006BC74 File Offset: 0x00069E74
	// (remove) Token: 0x0600166C RID: 5740 RVA: 0x0006BCAC File Offset: 0x00069EAC
	public event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x0600166D RID: 5741 RVA: 0x0006BCE4 File Offset: 0x00069EE4
	// (remove) Token: 0x0600166E RID: 5742 RVA: 0x0006BD1C File Offset: 0x00069F1C
	public event Action OnMiss;

	// Token: 0x0600166F RID: 5743 RVA: 0x0006BD51 File Offset: 0x00069F51
	public void Activate(AttackContext ctx)
	{
		this.attackContext = ctx;
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x00002318 File Offset: 0x00000518
	public void Cancel()
	{
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x0006BD5C File Offset: 0x00069F5C
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

	// Token: 0x06001672 RID: 5746 RVA: 0x0006BE38 File Offset: 0x0006A038
	private void ProcessSweepHit(RaycastHit hit)
	{
		Collider collider = hit.collider;
		if (collider == null)
		{
			return;
		}
		if (collider.GetComponentInParent<ArrowProjectile>())
		{
			return;
		}
		if (collider.gameObject.layer == 11)
		{
			this.HandleHit(collider.transform, hit.point);
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

	// Token: 0x06001673 RID: 5747 RVA: 0x0006BF74 File Offset: 0x0006A174
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

	// Token: 0x06001674 RID: 5748 RVA: 0x0006BFC8 File Offset: 0x0006A1C8
	private void FinalizeHit(Collider other, ICombatEntity combatEntity, float damageMultiplier, Vector3 hitPoint)
	{
		WgoPart componentInParent = other.GetComponentInParent<WgoPart>();
		if (!componentInParent)
		{
			return;
		}
		if (componentInParent.Wgo.TeamType == this.attackContext.teamType)
		{
			return;
		}
		Wgo.OnWgoDestroy -= this.OnWgoParentDestroy;
		Wgo.OnWgoDestroy += this.OnWgoParentDestroy;
		this.wgoPartInstanceId = componentInParent.Wgo.transform.GetInstanceID();
		this.HandleHit(other.transform, hitPoint);
		AttackContext attackContext = this.attackContext;
		ProjectileStats stats = base.Stats;
		this.attackContext = AttackContext.WithHit(attackContext, hitPoint, (stats != null) ? stats.damageEffectOverride : null);
		if (damageMultiplier == 0f)
		{
			this.attackContext = AttackContext.WithCustomDamage(this.attackContext, 0);
		}
		Action<ICombatEntity, AttackContext> onHit = this.OnHit;
		if (onHit == null)
		{
			return;
		}
		onHit(componentInParent.Wgo, this.attackContext);
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x0006C0A0 File Offset: 0x0006A2A0
	private void HandleHit(Transform parent, Vector3 hitPosition)
	{
		this.wasHit = true;
		Vector3 vector = this.endPointTransform.position - base.transform.position;
		base.transform.localPosition = hitPosition - vector;
		base.transform.SetParent(parent);
		this.trailRenderer.enabled = false;
		this.hitBoxCollider.enabled = false;
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x0006C106 File Offset: 0x0006A306
	private void OnWgoParentDestroy(Wgo parentWgo)
	{
		if (parentWgo.transform.GetInstanceID() == this.wgoPartInstanceId)
		{
			Wgo.OnWgoDestroy -= this.OnWgoParentDestroy;
			this.Despawn();
		}
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x0006C132 File Offset: 0x0006A332
	private void OnEnable()
	{
		this.trailRenderer.enabled = true;
		this.hitStates.Clear();
		this.hasPreviousPosition = false;
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0006C152 File Offset: 0x0006A352
	private void OnDisable()
	{
		Wgo.OnWgoDestroy -= this.OnWgoParentDestroy;
		if (base.IsSpawned)
		{
			this.Despawn();
		}
		this.trailRenderer.enabled = false;
		this.hitStates.Clear();
		this.hasPreviousPosition = false;
	}

	// Token: 0x06001679 RID: 5753 RVA: 0x0006C191 File Offset: 0x0006A391
	protected override void Despawn()
	{
		if (!base.IsSpawned)
		{
			return;
		}
		this.hitBoxCollider.enabled = true;
		base.Despawn();
	}

	// Token: 0x040016BE RID: 5822
	public TrailRenderer trailRenderer;

	// Token: 0x040016BF RID: 5823
	[SerializeField]
	private DamageSourceType damageSourceType = DamageSourceType.Arrow;

	// Token: 0x040016C0 RID: 5824
	[SerializeField]
	private BoxCollider hitBoxCollider;

	// Token: 0x040016C1 RID: 5825
	[SerializeField]
	private int sweepLayerMask = -1;

	// Token: 0x040016C2 RID: 5826
	[SerializeField]
	private Transform endPointTransform;

	// Token: 0x040016C3 RID: 5827
	private AttackContext attackContext;

	// Token: 0x040016C4 RID: 5828
	private int wgoPartInstanceId;

	// Token: 0x040016C5 RID: 5829
	private Vector3 previousPosition;

	// Token: 0x040016C6 RID: 5830
	private bool hasPreviousPosition;

	// Token: 0x040016C7 RID: 5831
	private readonly List<RaycastHit> sweepHits = new List<RaycastHit>();

	// Token: 0x040016C8 RID: 5832
	private readonly Dictionary<ICombatEntity, WeaponHitState> hitStates = new Dictionary<ICombatEntity, WeaponHitState>();
}
