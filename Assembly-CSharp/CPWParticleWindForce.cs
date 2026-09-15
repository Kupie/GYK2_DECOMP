using System;
using UnityEngine;

// Token: 0x02000B39 RID: 2873
[Serializable]
public class CPWParticleWindForce : CPWParticleWind
{
	// Token: 0x06004C75 RID: 19573 RVA: 0x00169021 File Offset: 0x00167221
	protected override void DoWindAffection_Internal(float windValue)
	{
		CPWParticleWindForce.AddToForce(this.particleSystem.forceOverLifetime, new Vector3(this.xWindMultiplication, this.yWindMultiplication, this.zWindMultiplication) * windValue);
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x00169050 File Offset: 0x00167250
	private static void AddToForce(ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule, Vector3 velocity)
	{
		if (forceOverLifetimeModule.enabled)
		{
			forceOverLifetimeModule.x = CPWParticleWind.AddToMinMaxCurve(forceOverLifetimeModule.x, velocity.x);
			forceOverLifetimeModule.y = CPWParticleWind.AddToMinMaxCurve(forceOverLifetimeModule.y, velocity.y);
			forceOverLifetimeModule.z = CPWParticleWind.AddToMinMaxCurve(forceOverLifetimeModule.z, velocity.z);
		}
	}

	// Token: 0x04003D8E RID: 15758
	public float xWindMultiplication;

	// Token: 0x04003D8F RID: 15759
	public float yWindMultiplication;

	// Token: 0x04003D90 RID: 15760
	public float zWindMultiplication;
}
