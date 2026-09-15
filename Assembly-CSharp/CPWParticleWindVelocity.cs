using System;
using UnityEngine;

// Token: 0x02000B3D RID: 2877
[Serializable]
public class CPWParticleWindVelocity : CPWParticleWind
{
	// Token: 0x06004C7E RID: 19582 RVA: 0x00169223 File Offset: 0x00167423
	protected override void DoWindAffection_Internal(float windValue)
	{
		CPWParticleWindVelocity.AddToVelocity(this.particleSystem.velocityOverLifetime, new Vector3(this.xWindMultiplication, this.yWindMultiplication, this.zWindMultiplication) * windValue);
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x00169254 File Offset: 0x00167454
	private static void AddToVelocity(ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule, Vector3 velocity)
	{
		if (velocityOverLifetimeModule.enabled)
		{
			velocityOverLifetimeModule.x = CPWParticleWind.AddToMinMaxCurve(velocityOverLifetimeModule.x, velocity.x);
			velocityOverLifetimeModule.y = CPWParticleWind.AddToMinMaxCurve(velocityOverLifetimeModule.y, velocity.y);
			velocityOverLifetimeModule.z = CPWParticleWind.AddToMinMaxCurve(velocityOverLifetimeModule.z, velocity.z);
		}
	}

	// Token: 0x04003D98 RID: 15768
	public float xWindMultiplication;

	// Token: 0x04003D99 RID: 15769
	public float yWindMultiplication;

	// Token: 0x04003D9A RID: 15770
	public float zWindMultiplication;
}
