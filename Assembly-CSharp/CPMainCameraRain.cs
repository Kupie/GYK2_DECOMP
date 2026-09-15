using System;

// Token: 0x02000B26 RID: 2854
[Serializable]
public class CPMainCameraRain : CPMainCameraFX<CameraFilterPack_Atmosphere_Rain>
{
	// Token: 0x06004C15 RID: 19477 RVA: 0x00167384 File Offset: 0x00165584
	protected override void SetIntensity(float value)
	{
		base.FX.Fade = value;
	}
}
