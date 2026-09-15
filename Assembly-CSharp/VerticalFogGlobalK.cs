using System;

// Token: 0x020001D1 RID: 465
public class VerticalFogGlobalK
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x0003C2AF File Offset: 0x0003A4AF
	public static float GlobalFogCoefficient
	{
		get
		{
			return 1f - VerticalFogDisableZone.GetFogDisableAmount();
		}
	}
}
