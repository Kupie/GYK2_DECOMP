using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
[Serializable]
public class CPWindValue : ControllableParameter
{
	// Token: 0x06004C1D RID: 19485 RVA: 0x0016748C File Offset: 0x0016568C
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		float num = v * this.windValue;
		CPWindValue.values[this] = new CPWindValue.Wind(num, weatherComponent);
	}

	// Token: 0x06004C1E RID: 19486 RVA: 0x001674B4 File Offset: 0x001656B4
	public static void ApplyParameters()
	{
		if (CPWindValue.values.Count == 0)
		{
			return;
		}
		CPWindValue.Wind wind = null;
		CPWindValue.Wind wind2 = null;
		foreach (KeyValuePair<CPWindValue, CPWindValue.Wind> keyValuePair in CPWindValue.values)
		{
			if (keyValuePair.Value.weatherComponent.Animating == WeatherComponent.AnimationType.FadeIn)
			{
				wind = keyValuePair.Value;
			}
			else if (keyValuePair.Value.weatherComponent.Animating == WeatherComponent.AnimationType.FadeOut)
			{
				wind2 = keyValuePair.Value;
			}
		}
		float num = 0f;
		foreach (CPWindValue.Wind wind3 in CPWindValue.values.Values)
		{
			num = Mathf.Max(num, wind3.value);
		}
		if (wind != null && wind2 != null && wind.weatherComponent.intensity > 0f && wind2.weatherComponent.intensity > 0f)
		{
			CPWindValue controllableParameterOfType = wind.weatherComponent.GetControllableParameterOfType<CPWindValue>();
			num = Mathf.Lerp(wind2.weatherComponent.GetControllableParameterOfType<CPWindValue>().windValue, controllableParameterOfType.windValue, wind.weatherComponent.intensity);
		}
		WeatherSystem.Instance.WindValue = num;
	}

	// Token: 0x04003D45 RID: 15685
	[Range(0f, 1f)]
	public float windValue;

	// Token: 0x04003D46 RID: 15686
	private static Dictionary<CPWindValue, CPWindValue.Wind> values = new Dictionary<CPWindValue, CPWindValue.Wind>();

	// Token: 0x02000B2B RID: 2859
	private class Wind
	{
		// Token: 0x06004C21 RID: 19489 RVA: 0x00167618 File Offset: 0x00165818
		public Wind(float value, WeatherComponent weatherComponent)
		{
			this.value = value;
			this.weatherComponent = weatherComponent;
		}

		// Token: 0x04003D47 RID: 15687
		public float value;

		// Token: 0x04003D48 RID: 15688
		public WeatherComponent weatherComponent;
	}
}
