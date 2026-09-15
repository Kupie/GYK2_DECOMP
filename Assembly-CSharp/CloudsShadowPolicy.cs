using System;

// Token: 0x02000B15 RID: 2837
public static class CloudsShadowPolicy
{
	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x06004B99 RID: 19353 RVA: 0x00165711 File Offset: 0x00163911
	public static PlatformCloudAppearance ActiveAppearance
	{
		get
		{
			return PlatformFeatures.Current.cloudAppearance;
		}
	}

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06004B9A RID: 19354 RVA: 0x0016571D File Offset: 0x0016391D
	public static bool UseCloudsRTOverlay
	{
		get
		{
			return CloudsShadowPolicy.ActiveAppearance == PlatformCloudAppearance.RenderTexture;
		}
	}

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x06004B9B RID: 19355 RVA: 0x00165727 File Offset: 0x00163927
	public static bool UseReplacedCloudMaterial
	{
		get
		{
			return CloudsShadowPolicy.ActiveAppearance == PlatformCloudAppearance.Replaced;
		}
	}
}
