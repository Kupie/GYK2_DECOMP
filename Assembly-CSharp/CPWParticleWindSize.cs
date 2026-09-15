using System;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
[Serializable]
public class CPWParticleWindSize : CPWParticleWind
{
	// Token: 0x06004C7A RID: 19578 RVA: 0x001690F0 File Offset: 0x001672F0
	protected override void DoWindAffection_Internal(float windValue)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		if (main.startSize3D)
		{
			main.startSizeX = CPWParticleWind.AddToMinMaxCurve(main.startSizeX, Mathf.Abs(this.xWindMultiplication * windValue));
			main.startSizeY = CPWParticleWind.AddToMinMaxCurve(main.startSizeY, Mathf.Abs(this.yWindMultiplication * windValue));
			main.startSizeZ = CPWParticleWind.AddToMinMaxCurve(main.startSizeZ, Mathf.Abs(this.zWindMultiplication * windValue));
			return;
		}
		main.startSize = CPWParticleWind.AddToMinMaxCurve(main.startSize, Mathf.Abs(this.xWindMultiplication * windValue));
	}

	// Token: 0x04003D92 RID: 15762
	public float xWindMultiplication;

	// Token: 0x04003D93 RID: 15763
	public float yWindMultiplication;

	// Token: 0x04003D94 RID: 15764
	public float zWindMultiplication;
}
