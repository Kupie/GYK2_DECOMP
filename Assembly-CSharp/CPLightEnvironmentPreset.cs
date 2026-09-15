using System;

// Token: 0x02000B24 RID: 2852
[Serializable]
public class CPLightEnvironmentPreset : ControllableParameter
{
	// Token: 0x06004C0F RID: 19471 RVA: 0x001672C8 File Offset: 0x001654C8
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		if (v == 0f)
		{
			EnvironmentEngine.Instance.ApplyOverridePreset(null, 0f);
			return;
		}
		EnvironmentEngine.Instance.ApplyOverridePreset(this.preset, v);
	}

	// Token: 0x04003D3E RID: 15678
	public LightEnvironmentPreset preset;
}
