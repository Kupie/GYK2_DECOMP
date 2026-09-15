using System;
using UnityEngine;

// Token: 0x02000792 RID: 1938
public static class PlatformUnityShadowPresets
{
	// Token: 0x060031DF RID: 12767 RVA: 0x000EF1C4 File Offset: 0x000ED3C4
	public static PlatformUnityShadowSettings Get(PlatformUnityShadowPreset preset)
	{
		switch (preset)
		{
		case PlatformUnityShadowPreset.DesktopLike:
			return new PlatformUnityShadowSettings(ShadowQuality.All, ShadowResolution.High, 3000f, 4, LightShadows.Soft, 8, null);
		case PlatformUnityShadowPreset.Balanced:
			return new PlatformUnityShadowSettings(ShadowQuality.HardOnly, ShadowResolution.Medium, 120f, 2, LightShadows.Hard, 8, null);
		case PlatformUnityShadowPreset.Performance:
			return new PlatformUnityShadowSettings(ShadowQuality.HardOnly, ShadowResolution.Low, 80f, 2, LightShadows.Hard, 8, null);
		case PlatformUnityShadowPreset.Low:
			return new PlatformUnityShadowSettings(ShadowQuality.HardOnly, ShadowResolution.Low, 50f, 0, LightShadows.Hard, 8, null);
		case PlatformUnityShadowPreset.Minimal:
			return new PlatformUnityShadowSettings(ShadowQuality.HardOnly, ShadowResolution.Low, 30f, 0, LightShadows.Hard, 8, null);
		case PlatformUnityShadowPreset.Off:
			return new PlatformUnityShadowSettings(ShadowQuality.Disable, ShadowResolution.Low, 0f, 0, LightShadows.None, 8, null);
		case PlatformUnityShadowPreset.Console:
			return new PlatformUnityShadowSettings(ShadowQuality.All, ShadowResolution.Low, 50f, 0, LightShadows.Soft, 8, new LightShadows?(LightShadows.None));
		default:
			return PlatformUnityShadowPresets.Get(PlatformUnityShadowPreset.Balanced);
		}
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x000EF2AC File Offset: 0x000ED4AC
	public static string GetDisplayName(PlatformUnityShadowPreset preset)
	{
		switch (preset)
		{
		case PlatformUnityShadowPreset.DesktopLike:
			return "Shadow Preset: Desktop";
		case PlatformUnityShadowPreset.Balanced:
			return "Shadow Preset: Balanced";
		case PlatformUnityShadowPreset.Performance:
			return "Shadow Preset: Performance";
		case PlatformUnityShadowPreset.Low:
			return "Shadow Preset: Low";
		case PlatformUnityShadowPreset.Minimal:
			return "Shadow Preset: Minimal";
		case PlatformUnityShadowPreset.Off:
			return "Shadow Preset: Off";
		case PlatformUnityShadowPreset.Console:
			return "Shadow Preset: Console";
		default:
			return preset.ToString();
		}
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x000EF314 File Offset: 0x000ED514
	public static int IndexOf(PlatformUnityShadowPreset preset)
	{
		for (int i = 0; i < PlatformUnityShadowPresets.DebugPresetOrder.Length; i++)
		{
			if (PlatformUnityShadowPresets.DebugPresetOrder[i] == preset)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x04002826 RID: 10278
	public static readonly PlatformUnityShadowPreset[] DebugPresetOrder = new PlatformUnityShadowPreset[]
	{
		PlatformUnityShadowPreset.DesktopLike,
		PlatformUnityShadowPreset.Balanced,
		PlatformUnityShadowPreset.Performance,
		PlatformUnityShadowPreset.Low,
		PlatformUnityShadowPreset.Minimal,
		PlatformUnityShadowPreset.Off,
		PlatformUnityShadowPreset.Console
	};
}
