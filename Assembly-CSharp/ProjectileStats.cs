using System;
using UnityEngine;

// Token: 0x02000343 RID: 835
[CreateAssetMenu(fileName = "ProjectileStats", menuName = "GK2/Fighting/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
	// Token: 0x04001676 RID: 5750
	public float speed = 40f;

	// Token: 0x04001677 RID: 5751
	public float maxLifeTime = 8f;

	// Token: 0x04001678 RID: 5752
	[Tooltip("World FX spawned by the projectile at the hit point when damage is dealt.")]
	public string onDamageHitFxName;

	// Token: 0x04001679 RID: 5753
	public DamageEffectSettings damageEffectOverride;
}
