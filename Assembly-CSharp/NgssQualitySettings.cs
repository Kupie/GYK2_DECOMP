using System;
using PI.NGSS;

// Token: 0x02000783 RID: 1923
public readonly struct NgssQualitySettings
{
	// Token: 0x060031BB RID: 12731 RVA: 0x000EE7E8 File Offset: 0x000EC9E8
	public NgssQualitySettings(int directionalSamplingTest, int directionalSamplingFilter, bool directionalPcssEnabled, bool directionalCascadesBlending, int localSamplingTest, int localSamplingFilter)
	{
		this.directionalSamplingTest = directionalSamplingTest;
		this.directionalSamplingFilter = directionalSamplingFilter;
		this.directionalPcssEnabled = directionalPcssEnabled;
		this.directionalCascadesBlending = directionalCascadesBlending;
		this.localSamplingTest = localSamplingTest;
		this.localSamplingFilter = localSamplingFilter;
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x000EE818 File Offset: 0x000ECA18
	public void Apply(NGSS_Directional directional, NGSS_Local local)
	{
		if (directional != null)
		{
			directional.NGSS_SAMPLING_TEST = this.directionalSamplingTest;
			directional.NGSS_SAMPLING_FILTER = this.directionalSamplingFilter;
			directional.NGSS_PCSS_ENABLED = this.directionalPcssEnabled;
			directional.NGSS_CASCADES_BLENDING = this.directionalCascadesBlending;
		}
		if (local != null)
		{
			local.NGSS_SAMPLING_TEST = this.localSamplingTest;
			local.NGSS_SAMPLING_FILTER = this.localSamplingFilter;
		}
	}

	// Token: 0x040027D2 RID: 10194
	public readonly int directionalSamplingTest;

	// Token: 0x040027D3 RID: 10195
	public readonly int directionalSamplingFilter;

	// Token: 0x040027D4 RID: 10196
	public readonly bool directionalPcssEnabled;

	// Token: 0x040027D5 RID: 10197
	public readonly bool directionalCascadesBlending;

	// Token: 0x040027D6 RID: 10198
	public readonly int localSamplingTest;

	// Token: 0x040027D7 RID: 10199
	public readonly int localSamplingFilter;
}
