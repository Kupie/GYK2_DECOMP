using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000342 RID: 834
public class ProjectilePool : LazySingleton<ProjectilePool>
{
	// Token: 0x06001618 RID: 5656 RVA: 0x0006AFEC File Offset: 0x000691EC
	protected override void Awake()
	{
		base.Awake();
		foreach (Projectile projectile in this.projectilePrefabs)
		{
			if (!this.projectilePrefabsByType.TryAdd(projectile.GetType(), new Pool(projectile, base.transform, 10, Pool.PoolType.ImmediateActivation, false, null)))
			{
				Debug.LogError(string.Format("Duplication of projectile prefab {0}, type: {1}", projectile.name, projectile.GetType()), this);
			}
		}
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x0006B080 File Offset: 0x00069280
	public static T GetOrCreate<T>() where T : Projectile
	{
		Pool pool;
		if (LazySingleton<ProjectilePool>.Instance.projectilePrefabsByType.TryGetValue(typeof(T), out pool))
		{
			return pool.GetOrCreateObject<T>();
		}
		return default(T);
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x0006B0BC File Offset: 0x000692BC
	public static void Release<T>(T obj) where T : Projectile
	{
		Pool pool;
		if (LazySingleton<ProjectilePool>.Instance.projectilePrefabsByType.TryGetValue(typeof(T), out pool))
		{
			pool.ReleaseObject<T>(obj);
		}
	}

	// Token: 0x04001674 RID: 5748
	[SerializeField]
	private List<Projectile> projectilePrefabs;

	// Token: 0x04001675 RID: 5749
	private Dictionary<Type, Pool> projectilePrefabsByType = new Dictionary<Type, Pool>();
}
