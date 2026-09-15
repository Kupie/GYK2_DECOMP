using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class ZombieMouthSpitWeapon : Weapon, IDamageDealer
{
	// Token: 0x170003DF RID: 991
	// (get) Token: 0x060016A9 RID: 5801 RVA: 0x0006B517 File Offset: 0x00069717
	public IDamageDealer Dealer
	{
		get
		{
			return this;
		}
	}

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x060016AA RID: 5802 RVA: 0x0006CEB8 File Offset: 0x0006B0B8
	// (remove) Token: 0x060016AB RID: 5803 RVA: 0x0006CEF0 File Offset: 0x0006B0F0
	public event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x060016AC RID: 5804 RVA: 0x0006CF28 File Offset: 0x0006B128
	// (remove) Token: 0x060016AD RID: 5805 RVA: 0x0006CF60 File Offset: 0x0006B160
	public event Action OnMiss;

	// Token: 0x060016AE RID: 5806 RVA: 0x0006CF98 File Offset: 0x0006B198
	public void Activate(AttackContext ctx)
	{
		SpitProjectile orCreate = ProjectilePool.GetOrCreate<SpitProjectile>();
		if (orCreate == null)
		{
			return;
		}
		if (this.useSphereAsEmitter && this.sphereCollider)
		{
			ctx = this.GetOriginPosOnSphere(ctx);
		}
		orCreate.Activate(ctx);
		orCreate.Launch(this.projectileStats, ctx.origin, ctx.direction);
		this.SubscribeToEvents(orCreate);
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x00002318 File Offset: 0x00000518
	public void Cancel()
	{
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x0006CFFC File Offset: 0x0006B1FC
	private void OnDestroy()
	{
		foreach (SpitProjectile spitProjectile in this.spitProjectiles)
		{
			this.UnsubscribeFromEvents(spitProjectile);
		}
		this.spitProjectiles.Clear();
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x0006D05C File Offset: 0x0006B25C
	private void SubscribeToEvents(SpitProjectile projectile)
	{
		projectile.OnHit += this.HandleHit;
		projectile.OnMiss += this.HandleMiss;
		projectile.OnDespawned += this.HandleProjectileOnDestroy;
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x0006D094 File Offset: 0x0006B294
	private void UnsubscribeFromEvents(SpitProjectile projectile)
	{
		projectile.OnHit -= this.HandleHit;
		projectile.OnMiss -= this.HandleMiss;
		projectile.OnDespawned -= this.HandleProjectileOnDestroy;
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x0006D0CC File Offset: 0x0006B2CC
	private void HandleHit(ICombatEntity combatEntity, AttackContext ctx)
	{
		Action<ICombatEntity, AttackContext> onHit = this.OnHit;
		if (onHit == null)
		{
			return;
		}
		onHit(combatEntity, ctx);
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x0006D0E0 File Offset: 0x0006B2E0
	private void HandleMiss()
	{
		Action onMiss = this.OnMiss;
		if (onMiss == null)
		{
			return;
		}
		onMiss();
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x0006D0F4 File Offset: 0x0006B2F4
	private void HandleProjectileOnDestroy(Projectile projectile)
	{
		SpitProjectile spitProjectile = projectile as SpitProjectile;
		if (spitProjectile != null)
		{
			this.UnsubscribeFromEvents(spitProjectile);
			this.spitProjectiles.Remove(spitProjectile);
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x0006D120 File Offset: 0x0006B320
	private AttackContext GetOriginPosOnSphere(AttackContext ctx)
	{
		Vector3 vector = this.sphereCollider.transform.position + ctx.direction.XZ().normalized * this.sphereCollider.radius;
		return new AttackContext(ctx.attacker, ctx.teamType, ctx.fighterDef, ctx.weaponDef, vector, ctx.direction, default(Vector3), -1, false, null, null);
	}

	// Token: 0x040016E7 RID: 5863
	public ProjectileStats projectileStats;

	// Token: 0x040016E8 RID: 5864
	public bool useSphereAsEmitter;

	// Token: 0x040016E9 RID: 5865
	public SphereCollider sphereCollider;

	// Token: 0x040016EA RID: 5866
	private List<SpitProjectile> spitProjectiles = new List<SpitProjectile>();
}
