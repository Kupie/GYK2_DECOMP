using System;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public abstract class DayNightLightBase : MonoBehaviour
{
	// Token: 0x06002117 RID: 8471
	protected abstract void ApplyLightMode(DayNightLightBase.LightMode mode);

	// Token: 0x06002118 RID: 8472 RVA: 0x0009C4D0 File Offset: 0x0009A6D0
	public void ApplyLightModeInt(int mode)
	{
		this.ApplyLightMode((DayNightLightBase.LightMode)mode);
	}

	// Token: 0x04001DB2 RID: 7602
	protected static readonly int idLightMode = Shader.PropertyToID("_LightMode");

	// Token: 0x04001DB3 RID: 7603
	protected static readonly string idSunLightDependencyKeyword = "USE_SUN_LIGHT_DEPENDENCY";

	// Token: 0x04001DB4 RID: 7604
	public DayNightLightBase.LightMode mode;

	// Token: 0x020004F7 RID: 1271
	public enum LightMode
	{
		// Token: 0x04001DB6 RID: 7606
		Night,
		// Token: 0x04001DB7 RID: 7607
		Day,
		// Token: 0x04001DB8 RID: 7608
		Static
	}
}
