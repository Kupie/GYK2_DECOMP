using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000785 RID: 1925
public static class PlatformFeatureConfig
{
	// Token: 0x060031BE RID: 12734 RVA: 0x000EE8B8 File Offset: 0x000ECAB8
	private static Dictionary<GamePlatform, PlatformFeatureEntry> BuildLookup()
	{
		Dictionary<GamePlatform, PlatformFeatureEntry> dictionary = new Dictionary<GamePlatform, PlatformFeatureEntry>();
		for (int i = 0; i < PlatformFeatureConfig.Entries.Count; i++)
		{
			dictionary[PlatformFeatureConfig.Entries[i].platform] = PlatformFeatureConfig.Entries[i];
		}
		return dictionary;
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x000EE904 File Offset: 0x000ECB04
	public static PlatformFeatureEntry Get(GamePlatform platform)
	{
		PlatformFeatureEntry platformFeatureEntry;
		if (PlatformFeatureConfig.entriesByPlatform.TryGetValue(platform, out platformFeatureEntry))
		{
			return platformFeatureEntry;
		}
		Debug.LogWarning(string.Format("[PlatformFeatureConfig] Missing entry for {0}, using PC defaults.", platform));
		return PlatformFeatureConfig.PC;
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000EE93C File Offset: 0x000ECB3C
	public static Dictionary<string, PlatformTextureCompression> GetTextureCompressionSettings()
	{
		Dictionary<string, PlatformTextureCompression> dictionary = new Dictionary<string, PlatformTextureCompression>();
		for (int i = 0; i < PlatformFeatureConfig.Entries.Count; i++)
		{
			PlatformFeatureEntry platformFeatureEntry = PlatformFeatureConfig.Entries[i];
			if (platformFeatureEntry.textureCompression != PlatformTextureCompression.Disabled)
			{
				dictionary[platformFeatureEntry.platform.ToTextureImporterPlatformName()] = platformFeatureEntry.textureCompression;
			}
		}
		return dictionary;
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000EE990 File Offset: 0x000ECB90
	public static Dictionary<string, int> GetAudioCompressionSettings()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < PlatformFeatureConfig.Entries.Count; i++)
		{
			PlatformFeatureEntry platformFeatureEntry = PlatformFeatureConfig.Entries[i];
			if (platformFeatureEntry.audioCompression > 0 && platformFeatureEntry.audioCompression < 100)
			{
				dictionary[platformFeatureEntry.platform.ToTextureImporterPlatformName()] = platformFeatureEntry.audioCompression;
			}
		}
		return dictionary;
	}

	// Token: 0x040027D8 RID: 10200
	public static readonly PlatformFeatureEntry PC = new PlatformFeatureEntry
	{
		platform = GamePlatform.PC,
		shadowMode = PlatformShadowMode.NGSS,
		pointLightMode = PlatformPointLightMode.Realtime,
		waterTier = PlatformWaterTier.High,
		renderMode = PlatformRenderMode.Native,
		hbaoQuality = PlatformHBAOQuality.Highest,
		textureCompression = PlatformTextureCompression.Disabled,
		audioCompression = 100,
		physicsDelta = PlatformPhysicsDelta.Default
	};

	// Token: 0x040027D9 RID: 10201
	public static readonly PlatformFeatureEntry Switch = new PlatformFeatureEntry
	{
		platform = GamePlatform.Switch,
		shadowMode = PlatformShadowMode.Off,
		cloudAppearance = PlatformCloudAppearance.RenderTexture,
		pointLightMode = PlatformPointLightMode.Faked,
		backLightEnabled = false,
		waterTier = PlatformWaterTier.Light,
		renderMode = PlatformRenderMode.Lightweight,
		hbaoQuality = PlatformHBAOQuality.Off,
		textureCompression = PlatformTextureCompression.ASTC_6x6,
		audioCompression = 50,
		physicsDelta = PlatformPhysicsDelta.Fps30
	};

	// Token: 0x040027DA RID: 10202
	public static readonly PlatformFeatureEntry Switch2 = new PlatformFeatureEntry
	{
		platform = GamePlatform.Switch2,
		shadowMode = PlatformShadowMode.Off,
		cloudAppearance = PlatformCloudAppearance.RenderTexture,
		pointLightMode = PlatformPointLightMode.Realtime,
		backLightEnabled = true,
		waterTier = PlatformWaterTier.Light,
		renderMode = PlatformRenderMode.Native,
		hbaoQuality = PlatformHBAOQuality.Lowest,
		textureCompression = PlatformTextureCompression.ASTC_4x4,
		audioCompression = 60,
		physicsDelta = PlatformPhysicsDelta.Default
	};

	// Token: 0x040027DB RID: 10203
	public static readonly PlatformFeatureEntry PS4 = new PlatformFeatureEntry
	{
		platform = GamePlatform.PS4,
		shadowMode = PlatformShadowMode.Off,
		cloudAppearance = PlatformCloudAppearance.RenderTexture,
		pointLightMode = PlatformPointLightMode.Faked,
		waterTier = PlatformWaterTier.Light,
		renderMode = PlatformRenderMode.Native,
		hbaoQuality = PlatformHBAOQuality.Lowest,
		textureCompression = PlatformTextureCompression.BC7,
		audioCompression = 100,
		physicsDelta = PlatformPhysicsDelta.Default,
		backLightEnabled = false
	};

	// Token: 0x040027DC RID: 10204
	public static readonly PlatformFeatureEntry PS5 = new PlatformFeatureEntry
	{
		platform = GamePlatform.PS5,
		shadowMode = PlatformShadowMode.Unity,
		cloudAppearance = PlatformCloudAppearance.Replaced,
		unityShadowPreset = PlatformUnityShadowPreset.Console,
		pointLightMode = PlatformPointLightMode.Realtime,
		waterTier = PlatformWaterTier.High,
		renderMode = PlatformRenderMode.Native,
		hbaoQuality = PlatformHBAOQuality.High,
		textureCompression = PlatformTextureCompression.Disabled,
		audioCompression = 100,
		physicsDelta = PlatformPhysicsDelta.Default
	};

	// Token: 0x040027DD RID: 10205
	public static readonly PlatformFeatureEntry XboxOne = new PlatformFeatureEntry
	{
		platform = GamePlatform.XboxOne,
		shadowMode = PlatformShadowMode.Off,
		cloudAppearance = PlatformCloudAppearance.RenderTexture,
		pointLightMode = PlatformPointLightMode.Faked,
		waterTier = PlatformWaterTier.Light,
		renderMode = PlatformRenderMode.Native,
		hbaoQuality = PlatformHBAOQuality.Lowest,
		textureCompression = PlatformTextureCompression.BC7,
		audioCompression = 100,
		physicsDelta = PlatformPhysicsDelta.Default
	};

	// Token: 0x040027DE RID: 10206
	public static readonly PlatformFeatureEntry XboxSeries = new PlatformFeatureEntry
	{
		platform = GamePlatform.XboxSeries,
		shadowMode = PlatformShadowMode.Unity,
		cloudAppearance = PlatformCloudAppearance.Replaced,
		unityShadowPreset = PlatformUnityShadowPreset.Console,
		pointLightMode = PlatformPointLightMode.Realtime,
		waterTier = PlatformWaterTier.High,
		renderMode = PlatformRenderMode.Native,
		hbaoQuality = PlatformHBAOQuality.High,
		textureCompression = PlatformTextureCompression.Disabled,
		audioCompression = 100,
		physicsDelta = PlatformPhysicsDelta.Default
	};

	// Token: 0x040027DF RID: 10207
	public static readonly IReadOnlyList<PlatformFeatureEntry> Entries = new PlatformFeatureEntry[]
	{
		PlatformFeatureConfig.PC,
		PlatformFeatureConfig.Switch,
		PlatformFeatureConfig.Switch2,
		PlatformFeatureConfig.PS4,
		PlatformFeatureConfig.PS5,
		PlatformFeatureConfig.XboxOne,
		PlatformFeatureConfig.XboxSeries
	};

	// Token: 0x040027E0 RID: 10208
	private static readonly Dictionary<GamePlatform, PlatformFeatureEntry> entriesByPlatform = PlatformFeatureConfig.BuildLookup();
}
