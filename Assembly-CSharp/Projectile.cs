using System;
using UnityEngine;

// Token: 0x02000341 RID: 833
public abstract class Projectile : MonoBehaviour
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600160F RID: 5647 RVA: 0x0006AE1C File Offset: 0x0006901C
	// (remove) Token: 0x06001610 RID: 5648 RVA: 0x0006AE54 File Offset: 0x00069054
	public event Action<Projectile> OnDespawned;

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06001611 RID: 5649 RVA: 0x0006AE89 File Offset: 0x00069089
	protected ProjectileStats Stats
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06001612 RID: 5650 RVA: 0x0006AE91 File Offset: 0x00069091
	protected bool IsSpawned
	{
		get
		{
			return this.isSpawned;
		}
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x0006AE9C File Offset: 0x0006909C
	public void Launch(ProjectileStats stats, Vector3 position, Vector3 direction)
	{
		this.isSpawned = true;
		this.data = stats;
		this.position = position;
		this.direction = direction;
		base.transform.position = position;
		if (!direction.sqrMagnitude.EqualsTo(0f, 1E-05f))
		{
			base.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
		}
		this.age = 0f;
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x0006AF0A File Offset: 0x0006910A
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.age += Time.deltaTime;
		if (this.age >= this.data.maxLifeTime)
		{
			this.Despawn();
			return;
		}
		this.Tick();
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x0006AF46 File Offset: 0x00069146
	private void Tick()
	{
		if (this.wasHit)
		{
			return;
		}
		base.transform.position += this.direction * (this.data.speed * Time.deltaTime);
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x0006AF84 File Offset: 0x00069184
	protected virtual void Despawn()
	{
		if (!this.isSpawned)
		{
			return;
		}
		this.isSpawned = false;
		this.wasHit = false;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.gameObject.SetActive(false);
		ProjectilePool.Release<Projectile>(this);
		Action<Projectile> onDespawned = this.OnDespawned;
		if (onDespawned == null)
		{
			return;
		}
		onDespawned(this);
	}

	// Token: 0x0400166E RID: 5742
	private bool isSpawned;

	// Token: 0x0400166F RID: 5743
	private ProjectileStats data;

	// Token: 0x04001670 RID: 5744
	private float age;

	// Token: 0x04001671 RID: 5745
	private Vector3 position;

	// Token: 0x04001672 RID: 5746
	private Vector3 direction;

	// Token: 0x04001673 RID: 5747
	protected bool wasHit;
}
