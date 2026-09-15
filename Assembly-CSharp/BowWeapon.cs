using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class BowWeapon : Weapon, IDamageDealer
{
	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06001636 RID: 5686 RVA: 0x0006B517 File Offset: 0x00069717
	public IDamageDealer Dealer
	{
		get
		{
			return this;
		}
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06001637 RID: 5687 RVA: 0x0006B51C File Offset: 0x0006971C
	// (remove) Token: 0x06001638 RID: 5688 RVA: 0x0006B554 File Offset: 0x00069754
	public event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06001639 RID: 5689 RVA: 0x0006B58C File Offset: 0x0006978C
	// (remove) Token: 0x0600163A RID: 5690 RVA: 0x0006B5C4 File Offset: 0x000697C4
	public event Action OnMiss;

	// Token: 0x0600163B RID: 5691 RVA: 0x0006B5FC File Offset: 0x000697FC
	public void Activate(AttackContext ctx)
	{
		ArrowProjectile orCreate = ProjectilePool.GetOrCreate<ArrowProjectile>();
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

	// Token: 0x0600163C RID: 5692 RVA: 0x00002318 File Offset: 0x00000518
	public void Cancel()
	{
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x0006B660 File Offset: 0x00069860
	private void OnDestroy()
	{
		foreach (ArrowProjectile arrowProjectile in this.arrows)
		{
			this.UnsubscribeFromEvents(arrowProjectile);
		}
		this.arrows.Clear();
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x0006B6C0 File Offset: 0x000698C0
	private void SubscribeToEvents(ArrowProjectile projectile)
	{
		projectile.OnHit += this.HandleHit;
		projectile.OnMiss += this.HandleMiss;
		projectile.OnDespawned += this.HandleProjectileOnDestroy;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x0006B6F8 File Offset: 0x000698F8
	private void UnsubscribeFromEvents(ArrowProjectile projectile)
	{
		projectile.OnHit -= this.HandleHit;
		projectile.OnMiss -= this.HandleMiss;
		projectile.OnDespawned -= this.HandleProjectileOnDestroy;
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x0006B730 File Offset: 0x00069930
	private void HandleHit(ICombatEntity combatEntity, AttackContext ctx)
	{
		Action<ICombatEntity, AttackContext> onHit = this.OnHit;
		if (onHit == null)
		{
			return;
		}
		onHit(combatEntity, ctx);
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x0006B744 File Offset: 0x00069944
	private void HandleMiss()
	{
		Action onMiss = this.OnMiss;
		if (onMiss == null)
		{
			return;
		}
		onMiss();
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0006B758 File Offset: 0x00069958
	private void HandleProjectileOnDestroy(Projectile projectile)
	{
		ArrowProjectile arrowProjectile = projectile as ArrowProjectile;
		if (arrowProjectile != null)
		{
			this.UnsubscribeFromEvents(arrowProjectile);
			this.arrows.Remove(arrowProjectile);
		}
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x0006B784 File Offset: 0x00069984
	private AttackContext GetOriginPosOnSphere(AttackContext ctx)
	{
		Vector3 vector = this.sphereCollider.transform.TransformPoint(this.sphereCollider.center) + ctx.direction.XZ().normalized * this.sphereCollider.radius;
		return new AttackContext(ctx.attacker, ctx.teamType, ctx.fighterDef, ctx.weaponDef, vector, ctx.direction, default(Vector3), -1, false, null, null);
	}

	// Token: 0x0400168E RID: 5774
	public ProjectileStats projectileStats;

	// Token: 0x0400168F RID: 5775
	public bool useSphereAsEmitter;

	// Token: 0x04001690 RID: 5776
	public SphereCollider sphereCollider;

	// Token: 0x04001691 RID: 5777
	private List<ArrowProjectile> arrows = new List<ArrowProjectile>();
}
