using System;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
[Serializable]
public class CPParticleAlpha : ControllableParameter
{
	// Token: 0x06004C17 RID: 19479 RVA: 0x0016739C File Offset: 0x0016559C
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		ParticleSystem.MainModule main = this.particleSystem.main;
		Color color = main.startColor.color;
		color.a = v * (this.alpha / 255f);
		main.startColor = color;
	}

	// Token: 0x04003D40 RID: 15680
	public float alpha = 255f;

	// Token: 0x04003D41 RID: 15681
	public ParticleSystem particleSystem;
}
