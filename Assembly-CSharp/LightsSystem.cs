using System;
using PI.NGSS;
using UnityEngine;

// Token: 0x02000B1B RID: 2843
public class LightsSystem : MonoBehaviour
{
	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x06004BDA RID: 19418 RVA: 0x00166A6E File Offset: 0x00164C6E
	public static LightsSystem Instance
	{
		get
		{
			if (LightsSystem.instance == null)
			{
				LightsSystem.instance = global::UnityEngine.Object.FindObjectOfType<LightsSystem>(true);
			}
			return LightsSystem.instance;
		}
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x06004BDB RID: 19419 RVA: 0x00166A8D File Offset: 0x00164C8D
	public Light SunLight
	{
		get
		{
			return this.sunLight;
		}
	}

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x06004BDC RID: 19420 RVA: 0x00166A95 File Offset: 0x00164C95
	public Light BackLight
	{
		get
		{
			return this.backLight;
		}
	}

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x06004BDD RID: 19421 RVA: 0x00166A9D File Offset: 0x00164C9D
	public bool NgssFeatureEnabled
	{
		get
		{
			return this.ngssFeatureEnabled;
		}
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06004BDE RID: 19422 RVA: 0x00166AA5 File Offset: 0x00164CA5
	public bool BackLightEnabled
	{
		get
		{
			return this.backLightEnabled;
		}
	}

	// Token: 0x06004BDF RID: 19423 RVA: 0x00166AAD File Offset: 0x00164CAD
	private void Awake()
	{
		if (LightsSystem.instance == null)
		{
			LightsSystem.instance = this;
		}
		PlatformFeatures.ApplyShadowSettings();
		PlatformFeatures.ApplyNgssQuality();
		PlatformFeatures.ApplyBackLightSettings();
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x00166AD4 File Offset: 0x00164CD4
	public void DisableNGSS()
	{
		NGSS_Local componentInChildren = base.GetComponentInChildren<NGSS_Local>(true);
		if (componentInChildren != null)
		{
			componentInChildren.enabled = false;
		}
		NGSS_Directional componentInChildren2 = base.GetComponentInChildren<NGSS_Directional>(true);
		if (componentInChildren2 != null)
		{
			componentInChildren2.enabled = false;
		}
		this.ngssFeatureEnabled = false;
	}

	// Token: 0x06004BE1 RID: 19425 RVA: 0x00166B18 File Offset: 0x00164D18
	public void EnableNGSS()
	{
		NGSS_Local componentInChildren = base.GetComponentInChildren<NGSS_Local>(true);
		if (componentInChildren != null)
		{
			componentInChildren.enabled = true;
		}
		NGSS_Directional componentInChildren2 = base.GetComponentInChildren<NGSS_Directional>(true);
		if (componentInChildren2 != null)
		{
			componentInChildren2.enabled = true;
		}
		this.ngssFeatureEnabled = true;
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x00166B5C File Offset: 0x00164D5C
	public void DisableBackLight()
	{
		if (this.backLight != null)
		{
			this.backLight.enabled = false;
		}
		this.backLightEnabled = false;
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x00166B7F File Offset: 0x00164D7F
	public void EnableBackLight()
	{
		if (this.backLight != null)
		{
			this.backLight.enabled = true;
		}
		this.backLightEnabled = true;
	}

	// Token: 0x04003D29 RID: 15657
	[SerializeField]
	private Light sunLight;

	// Token: 0x04003D2A RID: 15658
	[SerializeField]
	private Light backLight;

	// Token: 0x04003D2B RID: 15659
	private static LightsSystem instance;

	// Token: 0x04003D2C RID: 15660
	private bool ngssFeatureEnabled = true;

	// Token: 0x04003D2D RID: 15661
	private bool backLightEnabled = true;
}
