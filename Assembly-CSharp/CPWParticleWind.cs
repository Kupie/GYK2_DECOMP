using System;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
[Serializable]
public abstract class CPWParticleWind : CPWParticle
{
	// Token: 0x06004C6D RID: 19565 RVA: 0x001689D6 File Offset: 0x00166BD6
	public bool DoWindAffection(float windValue)
	{
		if (this.particleSystem == null)
		{
			Debug.LogError("CPWParticleWind null particle system");
			return false;
		}
		this.DoWindAffection_Internal(windValue);
		return true;
	}

	// Token: 0x06004C6E RID: 19566 RVA: 0x00002318 File Offset: 0x00000518
	protected virtual void DoWindAffection_Internal(float windValue)
	{
	}

	// Token: 0x06004C6F RID: 19567 RVA: 0x001689FC File Offset: 0x00166BFC
	protected static ParticleSystem.MinMaxCurve AddToMinMaxCurve(ParticleSystem.MinMaxCurve minMaxCurve, float additiveParameter)
	{
		ParticleSystemCurveMode mode = minMaxCurve.mode;
		if (mode != ParticleSystemCurveMode.Constant)
		{
			if (mode == ParticleSystemCurveMode.TwoConstants)
			{
				minMaxCurve = new ParticleSystem.MinMaxCurve(minMaxCurve.constantMin + additiveParameter, minMaxCurve.constantMax + additiveParameter);
			}
		}
		else
		{
			minMaxCurve = new ParticleSystem.MinMaxCurve(minMaxCurve.constant + additiveParameter);
		}
		return minMaxCurve;
	}

	// Token: 0x06004C70 RID: 19568 RVA: 0x00168A48 File Offset: 0x00166C48
	protected static ParticleSystem.MinMaxGradient AddToMinMaxGradient(ParticleSystem.MinMaxGradient minMaxGradient, float additiveParameter)
	{
		switch (minMaxGradient.mode)
		{
		case ParticleSystemGradientMode.Color:
		{
			Color color = minMaxGradient.color;
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Color(color.r, color.g, color.b, Mathf.Clamp01(color.a + additiveParameter)));
			break;
		}
		case ParticleSystemGradientMode.Gradient:
		{
			GradientAlphaKey[] array = new GradientAlphaKey[minMaxGradient.gradient.alphaKeys.Length];
			for (int i = 0; i < minMaxGradient.gradient.alphaKeys.Length; i++)
			{
				GradientAlphaKey gradientAlphaKey = minMaxGradient.gradient.alphaKeys[i];
				array[i] = new GradientAlphaKey(Mathf.Clamp01(gradientAlphaKey.alpha + additiveParameter), gradientAlphaKey.time);
			}
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Gradient
			{
				alphaKeys = array,
				mode = minMaxGradient.gradient.mode,
				colorKeys = minMaxGradient.gradient.colorKeys
			});
			break;
		}
		case ParticleSystemGradientMode.TwoColors:
		{
			Color colorMin = minMaxGradient.colorMin;
			Color colorMax = minMaxGradient.colorMax;
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Color(colorMin.r, colorMin.g, colorMin.b, Mathf.Clamp01(colorMin.a + additiveParameter)), new Color(colorMax.r, colorMax.g, colorMax.b, Mathf.Clamp01(colorMax.a + additiveParameter)));
			break;
		}
		case ParticleSystemGradientMode.TwoGradients:
		{
			GradientAlphaKey[] array2 = new GradientAlphaKey[minMaxGradient.gradient.alphaKeys.Length];
			for (int j = 0; j < minMaxGradient.gradient.alphaKeys.Length; j++)
			{
				GradientAlphaKey gradientAlphaKey2 = minMaxGradient.gradient.alphaKeys[j];
				array2[j] = new GradientAlphaKey(Mathf.Clamp01(gradientAlphaKey2.alpha + additiveParameter), gradientAlphaKey2.time);
			}
			GradientAlphaKey[] array3 = new GradientAlphaKey[minMaxGradient.gradientMin.alphaKeys.Length];
			for (int k = 0; k < minMaxGradient.gradientMin.alphaKeys.Length; k++)
			{
				GradientAlphaKey gradientAlphaKey3 = minMaxGradient.gradientMin.alphaKeys[k];
				array3[k] = new GradientAlphaKey(Mathf.Clamp01(gradientAlphaKey3.alpha + additiveParameter), gradientAlphaKey3.time);
			}
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Gradient
			{
				alphaKeys = array2,
				mode = minMaxGradient.gradient.mode,
				colorKeys = minMaxGradient.gradient.colorKeys
			}, new Gradient
			{
				alphaKeys = array3,
				mode = minMaxGradient.gradientMin.mode,
				colorKeys = minMaxGradient.gradientMin.colorKeys
			});
			break;
		}
		}
		return minMaxGradient;
	}

	// Token: 0x06004C71 RID: 19569 RVA: 0x00168CF8 File Offset: 0x00166EF8
	protected ParticleSystem.MinMaxGradient MultiplyToMinMaxGradient(ParticleSystem.MinMaxGradient minMaxGradient, float multiplier)
	{
		switch (minMaxGradient.mode)
		{
		case ParticleSystemGradientMode.Color:
		{
			Color color = minMaxGradient.color;
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Color(color.r, color.g, color.b, Mathf.Clamp01(color.a * multiplier)));
			break;
		}
		case ParticleSystemGradientMode.Gradient:
		{
			GradientAlphaKey[] array = new GradientAlphaKey[minMaxGradient.gradient.alphaKeys.Length];
			for (int i = 0; i < minMaxGradient.gradient.alphaKeys.Length; i++)
			{
				GradientAlphaKey gradientAlphaKey = minMaxGradient.gradient.alphaKeys[i];
				array[i] = new GradientAlphaKey(Mathf.Clamp01(gradientAlphaKey.alpha * multiplier), gradientAlphaKey.time);
			}
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Gradient
			{
				alphaKeys = array,
				mode = minMaxGradient.gradient.mode,
				colorKeys = minMaxGradient.gradient.colorKeys
			});
			break;
		}
		case ParticleSystemGradientMode.TwoColors:
		{
			Color colorMin = minMaxGradient.colorMin;
			Color colorMax = minMaxGradient.colorMax;
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Color(colorMin.r, colorMin.g, colorMin.b, Mathf.Clamp01(colorMin.a * multiplier)), new Color(colorMax.r, colorMax.g, colorMax.b, Mathf.Clamp01(colorMax.a * multiplier)));
			break;
		}
		case ParticleSystemGradientMode.TwoGradients:
		{
			GradientAlphaKey[] array2 = new GradientAlphaKey[minMaxGradient.gradient.alphaKeys.Length];
			for (int j = 0; j < minMaxGradient.gradient.alphaKeys.Length; j++)
			{
				GradientAlphaKey gradientAlphaKey2 = minMaxGradient.gradient.alphaKeys[j];
				array2[j] = new GradientAlphaKey(Mathf.Clamp01(gradientAlphaKey2.alpha * multiplier), gradientAlphaKey2.time);
			}
			GradientAlphaKey[] array3 = new GradientAlphaKey[minMaxGradient.gradientMin.alphaKeys.Length];
			for (int k = 0; k < minMaxGradient.gradientMin.alphaKeys.Length; k++)
			{
				GradientAlphaKey gradientAlphaKey3 = minMaxGradient.gradientMin.alphaKeys[k];
				array3[k] = new GradientAlphaKey(Mathf.Clamp01(gradientAlphaKey3.alpha * multiplier), gradientAlphaKey3.time);
			}
			minMaxGradient = new ParticleSystem.MinMaxGradient(new Gradient
			{
				alphaKeys = array2,
				mode = minMaxGradient.gradient.mode,
				colorKeys = minMaxGradient.gradient.colorKeys
			}, new Gradient
			{
				alphaKeys = array3,
				mode = minMaxGradient.gradientMin.mode,
				colorKeys = minMaxGradient.gradientMin.colorKeys
			});
			break;
		}
		}
		return minMaxGradient;
	}
}
