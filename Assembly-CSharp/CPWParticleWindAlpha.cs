using System;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
[Serializable]
public class CPWParticleWindAlpha : CPWParticleWind
{
	// Token: 0x06004C73 RID: 19571 RVA: 0x00168FB0 File Offset: 0x001671B0
	protected override void DoWindAffection_Internal(float windValue)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		if (Mathf.Abs(this.alphaAffectedByWind) > 0.001f)
		{
			float num = ((this.alphaAffectedByWind > 0f) ? this.alphaAffectedByWind : (1f - this.alphaAffectedByWind));
			main.startColor = base.MultiplyToMinMaxGradient(main.startColor, num * Mathf.Abs(windValue));
		}
	}

	// Token: 0x04003D8D RID: 15757
	[Range(-1f, 1f)]
	public float alphaAffectedByWind;
}
