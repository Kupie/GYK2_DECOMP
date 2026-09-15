using System;
using HorizonBasedAmbientOcclusion;
using UnityEngine;

// Token: 0x020001CE RID: 462
[ExecuteInEditMode]
[DefaultExecutionOrder(-1)]
public class HBAOColorManager : MonoBehaviour
{
	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0003B693 File Offset: 0x00039893
	private static HBAO Hbao
	{
		get
		{
			if (!HBAOColorManager.hbao)
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
				HBAOColorManager.hbao = hbao;
			}
			return HBAOColorManager.hbao;
		}
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x0003B6C8 File Offset: 0x000398C8
	private void OnEnable()
	{
		PlatformFeatures.ApplyHBAO();
		if (HBAOColorManager.Hbao != null)
		{
			Debug.Log("HBAO ColorManager initialized, enabled = " + HBAOColorManager.Hbao.enabled.ToString());
		}
		this.UpdateColors();
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x0003B710 File Offset: 0x00039910
	public static void UpdateColorFromFog(VerticalFog fog, float totalFogIntensity)
	{
		if (!GameSettings.IsHBAOEnabled() || !HBAOColorManager.Hbao)
		{
			return;
		}
		if (!Application.isPlaying)
		{
			return;
		}
		HBAOColorManager.currentAdditionalColor = HBAOColorManager.defaultFogAdditionalColor.Evaluate(EnvironmentEngine.Instance.timeOfDay);
		HBAOColorManager.currentColor = Color.Lerp(HBAOColorManager.defaultColor, fog.Color + HBAOColorManager.currentAdditionalColor, totalFogIntensity * (float)fog.fogEnabled.ToInt(0));
		HBAOColorManager.Hbao.SetAoColor(HBAOColorManager.currentColor);
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x0003B78F File Offset: 0x0003998F
	private void UpdateColors()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		HBAOColorManager.defaultColor = this.color;
		HBAOColorManager.defaultFogAdditionalColor = this.fogAdditionalColor;
	}

	// Token: 0x04000CD0 RID: 3280
	private static HBAO hbao;

	// Token: 0x04000CD1 RID: 3281
	public Color color;

	// Token: 0x04000CD2 RID: 3282
	public Gradient fogAdditionalColor = new Gradient();

	// Token: 0x04000CD3 RID: 3283
	private static Color defaultColor = Color.white;

	// Token: 0x04000CD4 RID: 3284
	private static Gradient defaultFogAdditionalColor;

	// Token: 0x04000CD5 RID: 3285
	private static Color currentColor;

	// Token: 0x04000CD6 RID: 3286
	private static Color currentAdditionalColor;
}
