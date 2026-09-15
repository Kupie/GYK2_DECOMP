using System;
using UnityEngine;

// Token: 0x02000B3C RID: 2876
[Serializable]
public class CPWParticleWindStartRotation : CPWParticleWind
{
	// Token: 0x06004C7C RID: 19580 RVA: 0x00169194 File Offset: 0x00167394
	protected override void DoWindAffection_Internal(float windValue)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		if (main.startRotation3D)
		{
			main.startRotationX = CPWParticleWind.AddToMinMaxCurve(main.startRotationX, this.xWindMultiplication * windValue);
			main.startRotationY = CPWParticleWind.AddToMinMaxCurve(main.startRotationY, this.yWindMultiplication * windValue);
			main.startRotationZ = CPWParticleWind.AddToMinMaxCurve(main.startRotationZ, this.zWindMultiplication * windValue);
			return;
		}
		main.startRotation = CPWParticleWind.AddToMinMaxCurve(main.startRotation, this.zWindMultiplication * windValue);
	}

	// Token: 0x04003D95 RID: 15765
	[Range(-1f, 1f)]
	public float xWindMultiplication;

	// Token: 0x04003D96 RID: 15766
	[Range(-1f, 1f)]
	public float yWindMultiplication;

	// Token: 0x04003D97 RID: 15767
	[Range(-1f, 1f)]
	public float zWindMultiplication;
}
