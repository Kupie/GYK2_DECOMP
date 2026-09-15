using System;
using UnityEngine;

// Token: 0x0200052C RID: 1324
[ExecuteInEditMode]
public class SyncedLight : MonoBehaviour
{
	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06002209 RID: 8713 RVA: 0x0009FDA8 File Offset: 0x0009DFA8
	private Light ThisLight
	{
		get
		{
			if (!this.thisLight || this.thisLight == null)
			{
				this.thisLight = base.GetComponent<Light>();
			}
			return this.thisLight;
		}
	}

	// Token: 0x0600220A RID: 8714 RVA: 0x0009FDD8 File Offset: 0x0009DFD8
	private void Update()
	{
		if (this.light)
		{
			this.ThisLight.color = this.light.color;
			this.ThisLight.intensity = this.light.intensity;
			if (this.ThisLight.type == LightType.Directional)
			{
				this.light.transform.rotation = this.ThisLight.transform.rotation;
			}
			else if (this.syncRange)
			{
				this.ThisLight.range = this.light.range;
			}
			this.ThisLight.bounceIntensity = this.light.bounceIntensity;
		}
	}

	// Token: 0x04001EA3 RID: 7843
	public Light light;

	// Token: 0x04001EA4 RID: 7844
	public bool syncRange = true;

	// Token: 0x04001EA5 RID: 7845
	private Light thisLight;
}
