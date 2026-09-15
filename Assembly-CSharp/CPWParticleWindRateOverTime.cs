using System;
using UnityEngine;

// Token: 0x02000B3A RID: 2874
[Serializable]
public class CPWParticleWindRateOverTime : CPWParticleWind
{
	// Token: 0x06004C78 RID: 19576 RVA: 0x001690B4 File Offset: 0x001672B4
	protected override void DoWindAffection_Internal(float windValue)
	{
		ParticleSystem.EmissionModule emission = this.particleSystem.emission;
		emission.rateOverTime = CPWParticleWind.AddToMinMaxCurve(emission.rateOverTime, Mathf.Abs(this.rateOverTimeAffectedByWind * windValue));
	}

	// Token: 0x04003D91 RID: 15761
	public float rateOverTimeAffectedByWind;
}
