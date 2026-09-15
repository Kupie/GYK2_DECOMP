using System;
using UnityEngine;

// Token: 0x02000B28 RID: 2856
[Serializable]
public class CPParticleEmission : ControllableParameter
{
	// Token: 0x06004C19 RID: 19481 RVA: 0x001673FC File Offset: 0x001655FC
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		this.particleSystem.emission.rateOverTime = Mathf.Lerp(0f, this.defaultValue, v);
		this.particleSystem.gameObject.SetActive(v > 0f);
	}

	// Token: 0x04003D42 RID: 15682
	[SerializeField]
	private float defaultValue;

	// Token: 0x04003D43 RID: 15683
	public ParticleSystem particleSystem;
}
