using System;
using HorizonBasedAmbientOcclusion;
using PI.NGSS;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public static class PlatformFeatures
{
	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000EED6C File Offset: 0x000ECF6C
	public static PlatformFeatureEntry Current
	{
		get
		{
			GamePlatform gamePlatform = GamePlatformResolver.Current;
			GraphicsTier graphicsTier = PlatformFeatures.ResolveGraphicsTier();
			if (PlatformFeatures.cachedCurrent != null)
			{
				GamePlatform? gamePlatform2 = PlatformFeatures.cachedPlatform;
				GamePlatform gamePlatform3 = gamePlatform;
				if ((gamePlatform2.GetValueOrDefault() == gamePlatform3) & (gamePlatform2 != null))
				{
					GraphicsTier? graphicsTier2 = PlatformFeatures.cachedGraphicsTier;
					GraphicsTier graphicsTier3 = graphicsTier;
					if ((graphicsTier2.GetValueOrDefault() == graphicsTier3) & (graphicsTier2 != null))
					{
						goto IL_0055;
					}
				}
			}
			PlatformFeatures.RebuildCurrent(gamePlatform, graphicsTier);
			IL_0055:
			return PlatformFeatures.cachedCurrent;
		}
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x000EEDD3 File Offset: 0x000ECFD3
	public static void RebuildCurrent()
	{
		PlatformFeatures.RebuildCurrent(GamePlatformResolver.Current, PlatformFeatures.ResolveGraphicsTier());
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x000EEDE4 File Offset: 0x000ECFE4
	private static void RebuildCurrent(GamePlatform platform, GraphicsTier graphicsTier)
	{
		PlatformFeatures.cachedCurrent = GraphicsTierConfig.ApplyTier(PlatformFeatureConfig.Get(platform), graphicsTier);
		PlatformFeatures.cachedPlatform = new GamePlatform?(platform);
		PlatformFeatures.cachedGraphicsTier = new GraphicsTier?(graphicsTier);
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x000EEE0D File Offset: 0x000ED00D
	private static GraphicsTier ResolveGraphicsTier()
	{
		if (GamePlatformResolver.Current != GamePlatform.PC)
		{
			return GraphicsTier.High;
		}
		if (GameSettings.Instance == null)
		{
			return GraphicsTier.High;
		}
		return GameSettings.Instance.graphicsTier;
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x000EEE2B File Offset: 0x000ED02B
	public static void ReapplyAll()
	{
		PlatformFeatures.RebuildCurrent();
		PlatformFeatures.ApplyShadowSettings();
		PlatformFeatures.ApplyNgssQuality();
		PlatformFeatures.ApplyBackLightSettings();
		PlatformFeatures.ApplyRenderMode();
		PlatformFeatures.ApplyHBAO();
		PlatformFeatures.RefreshPlatformDependentLightElements();
		SwitchLightPolicy.ApplyLightRTPolicy();
		PlatformFeatures.RefreshLightFakers();
		PlatformFeatures.RefreshWaterMaterials();
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x000EEE60 File Offset: 0x000ED060
	public static void ApplyShadowSettings()
	{
		PlatformFeatureEntry platformFeatureEntry = PlatformFeatures.Current;
		PlatformUnityShadowPreset platformUnityShadowPreset = PlatformFeatures.ResolveUnityShadowPreset(platformFeatureEntry);
		PlatformUnityShadowSettings platformUnityShadowSettings = PlatformUnityShadowPresets.Get(platformUnityShadowPreset);
		LightsSystem instance = LightsSystem.Instance;
		Light light = ((instance != null) ? instance.SunLight : null);
		platformUnityShadowSettings.Apply(light);
		SwitchLightPolicy.NotifyLocalLightShadowPolicyChanged();
		if (LightsSystem.Instance == null)
		{
			return;
		}
		if (PlatformFeatures.ShouldUseNgss(platformFeatureEntry, platformUnityShadowPreset))
		{
			LightsSystem.Instance.EnableNGSS();
			return;
		}
		LightsSystem.Instance.DisableNGSS();
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x000EEECC File Offset: 0x000ED0CC
	public static void ApplyNgssQuality()
	{
		if (LightsSystem.Instance == null)
		{
			return;
		}
		PlatformFeatureEntry platformFeatureEntry = PlatformFeatures.Current;
		PlatformUnityShadowPreset platformUnityShadowPreset = PlatformFeatures.ResolveUnityShadowPreset(platformFeatureEntry);
		if (!PlatformFeatures.ShouldUseNgss(platformFeatureEntry, platformUnityShadowPreset))
		{
			return;
		}
		NGSS_Directional ngss_Directional = ((LightsSystem.Instance.SunLight != null) ? LightsSystem.Instance.SunLight.GetComponent<NGSS_Directional>() : null);
		NGSS_Local componentInChildren = LightsSystem.Instance.GetComponentInChildren<NGSS_Local>(true);
		NgssQualityPresets.Get(platformFeatureEntry.ngssQuality).Apply(ngss_Directional, componentInChildren);
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x000EEF45 File Offset: 0x000ED145
	public static PlatformUnityShadowPreset GetActiveUnityShadowPreset()
	{
		return PlatformFeatures.ResolveUnityShadowPreset(PlatformFeatures.Current);
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x000EEF51 File Offset: 0x000ED151
	public static PlatformUnityShadowPreset GetPlatformDefaultUnityShadowPreset(PlatformFeatureEntry features)
	{
		if (features.shadowMode == PlatformShadowMode.Off)
		{
			return PlatformUnityShadowPreset.Off;
		}
		if (features.shadowMode == PlatformShadowMode.Unity)
		{
			return features.unityShadowPreset;
		}
		return PlatformUnityShadowPreset.DesktopLike;
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x000EEF51 File Offset: 0x000ED151
	private static PlatformUnityShadowPreset ResolveUnityShadowPreset(PlatformFeatureEntry features)
	{
		if (features.shadowMode == PlatformShadowMode.Off)
		{
			return PlatformUnityShadowPreset.Off;
		}
		if (features.shadowMode == PlatformShadowMode.Unity)
		{
			return features.unityShadowPreset;
		}
		return PlatformUnityShadowPreset.DesktopLike;
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x000EEF6E File Offset: 0x000ED16E
	private static bool ShouldUseNgss(PlatformFeatureEntry features, PlatformUnityShadowPreset preset)
	{
		return features.shadowMode == PlatformShadowMode.NGSS && preset == PlatformUnityShadowPreset.DesktopLike;
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x000EEF7F File Offset: 0x000ED17F
	public static void ApplyBackLightSettings()
	{
		if (LightsSystem.Instance == null)
		{
			return;
		}
		if (PlatformFeatures.Current.backLightEnabled)
		{
			LightsSystem.Instance.EnableBackLight();
			return;
		}
		LightsSystem.Instance.DisableBackLight();
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x000EEFB0 File Offset: 0x000ED1B0
	public static void ApplyRenderMode()
	{
		if (CameraSystem.Instance == null)
		{
			return;
		}
		CameraSystem.Instance.ApplyRenderActiveState();
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x000EEFCC File Offset: 0x000ED1CC
	public static void ApplyHBAO()
	{
		CameraSystem instance = CameraSystem.Instance;
		HBAO hbao;
		if (instance == null)
		{
			hbao = null;
		}
		else
		{
			MainCamera mainCamera = instance.MainCamera;
			hbao = ((mainCamera != null) ? mainCamera.GetComponent<HBAO>() : null);
		}
		HBAO hbao2 = hbao;
		if (hbao2 == null)
		{
			return;
		}
		PlatformFeatureEntry platformFeatureEntry = PlatformFeatures.Current;
		hbao2.enabled = platformFeatureEntry.hbaoQuality > PlatformHBAOQuality.Off;
		if (!hbao2.enabled)
		{
			return;
		}
		hbao2.SetQuality(PlatformFeatures.MapHBAOQuality(platformFeatureEntry.hbaoQuality));
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x000EF030 File Offset: 0x000ED230
	public static bool IsHBAOEnabled()
	{
		return PlatformFeatures.Current.hbaoQuality > PlatformHBAOQuality.Off;
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x000EF03F File Offset: 0x000ED23F
	public static PlatformSpecificMaterialType GetWaterMaterialType()
	{
		return PlatformFeatures.Current.GetWaterMaterialType();
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x000EF04C File Offset: 0x000ED24C
	private static void RefreshWaterMaterials()
	{
		MaterialProvider[] array = global::UnityEngine.Object.FindObjectsByType<MaterialProvider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ApplyMaterial();
		}
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x000EF078 File Offset: 0x000ED278
	private static void RefreshPlatformDependentLightElements()
	{
		foreach (PlatformDependentElementGK2 platformDependentElementGK in global::UnityEngine.Object.FindObjectsByType<PlatformDependentElementGK2>(FindObjectsInactive.Include, FindObjectsSortMode.None))
		{
			if (platformDependentElementGK.rtLightInUse != platformDependentElementGK.rtLightNotInUse)
			{
				platformDependentElementGK.Init();
			}
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x000EF0B4 File Offset: 0x000ED2B4
	private static void RefreshLightFakers()
	{
		LightFaker[] array = global::UnityEngine.Object.FindObjectsByType<LightFaker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].RefreshPolicyState();
		}
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000EF0DF File Offset: 0x000ED2DF
	private static HBAO.Quality MapHBAOQuality(PlatformHBAOQuality quality)
	{
		switch (quality)
		{
		case PlatformHBAOQuality.Lowest:
			return HBAO.Quality.Lowest;
		case PlatformHBAOQuality.Low:
			return HBAO.Quality.Low;
		case PlatformHBAOQuality.High:
			return HBAO.Quality.High;
		case PlatformHBAOQuality.Highest:
			return HBAO.Quality.Highest;
		}
		return HBAO.Quality.Medium;
	}

	// Token: 0x040027F0 RID: 10224
	private static PlatformFeatureEntry cachedCurrent;

	// Token: 0x040027F1 RID: 10225
	private static GamePlatform? cachedPlatform;

	// Token: 0x040027F2 RID: 10226
	private static GraphicsTier? cachedGraphicsTier;
}
