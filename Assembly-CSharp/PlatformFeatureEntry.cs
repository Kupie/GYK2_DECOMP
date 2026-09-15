using System;

// Token: 0x02000786 RID: 1926
[Serializable]
public class PlatformFeatureEntry
{
	// Token: 0x060031C3 RID: 12739 RVA: 0x000EEC9D File Offset: 0x000ECE9D
	public PlatformFeatureEntry Clone()
	{
		return (PlatformFeatureEntry)base.MemberwiseClone();
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000EECAC File Offset: 0x000ECEAC
	public float GetPhysicsTimestep()
	{
		PlatformPhysicsDelta platformPhysicsDelta = this.physicsDelta;
		if (platformPhysicsDelta == PlatformPhysicsDelta.Fps30)
		{
			return 0.033333335f;
		}
		if (platformPhysicsDelta != PlatformPhysicsDelta.Custom)
		{
			return 0.02f;
		}
		if (this.customPhysicsDelta <= 0f)
		{
			return 0.02f;
		}
		return this.customPhysicsDelta;
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x000EECF0 File Offset: 0x000ECEF0
	public PlatformSpecificMaterialType GetWaterMaterialType()
	{
		switch (this.waterTier)
		{
		case PlatformWaterTier.Medium:
			return PlatformSpecificMaterialType.Switch;
		case PlatformWaterTier.Light:
			return PlatformSpecificMaterialType.SwitchSimple;
		case PlatformWaterTier.Mobile:
			return PlatformSpecificMaterialType.Mobile;
		case PlatformWaterTier.Minimal:
			return PlatformSpecificMaterialType.Minimal;
		default:
			return PlatformSpecificMaterialType.Standalone;
		}
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x000EED27 File Offset: 0x000ECF27
	public MainCamera.RenderMode GetMainCameraRenderMode()
	{
		if (this.renderMode != PlatformRenderMode.Lightweight)
		{
			return MainCamera.RenderMode.Native;
		}
		return MainCamera.RenderMode.Lightweight;
	}

	// Token: 0x040027E1 RID: 10209
	public const int DefaultAudioCompression = 100;

	// Token: 0x040027E2 RID: 10210
	public GamePlatform platform;

	// Token: 0x040027E3 RID: 10211
	public PlatformShadowMode shadowMode = PlatformShadowMode.NGSS;

	// Token: 0x040027E4 RID: 10212
	public PlatformCloudAppearance cloudAppearance;

	// Token: 0x040027E5 RID: 10213
	public PlatformUnityShadowPreset unityShadowPreset = PlatformUnityShadowPreset.Balanced;

	// Token: 0x040027E6 RID: 10214
	public PlatformPointLightMode pointLightMode;

	// Token: 0x040027E7 RID: 10215
	public bool backLightEnabled = true;

	// Token: 0x040027E8 RID: 10216
	public PlatformWaterTier waterTier;

	// Token: 0x040027E9 RID: 10217
	public PlatformRenderMode renderMode;

	// Token: 0x040027EA RID: 10218
	public PlatformHBAOQuality hbaoQuality = PlatformHBAOQuality.Medium;

	// Token: 0x040027EB RID: 10219
	public NgssQualityPreset ngssQuality;

	// Token: 0x040027EC RID: 10220
	public PlatformTextureCompression textureCompression;

	// Token: 0x040027ED RID: 10221
	public int audioCompression = 100;

	// Token: 0x040027EE RID: 10222
	public PlatformPhysicsDelta physicsDelta;

	// Token: 0x040027EF RID: 10223
	public float customPhysicsDelta = 0.02f;
}
