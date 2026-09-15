using System;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
[Serializable]
public abstract class CPMainCameraFX<T> : ControllableParameter where T : MonoBehaviour
{
	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x06004C11 RID: 19473 RVA: 0x001672F4 File Offset: 0x001654F4
	protected T FX
	{
		get
		{
			if (this.fx == null)
			{
				this.fx = CameraSystem.Instance.MainCamera.GetComponent<T>();
				if (this.fx == null)
				{
					Debug.LogError(string.Format("Couldn't find {0} component on MainCamera.", typeof(T)));
				}
			}
			return this.fx;
		}
	}

	// Token: 0x06004C12 RID: 19474
	protected abstract void SetIntensity(float value);

	// Token: 0x06004C13 RID: 19475 RVA: 0x0016735B File Offset: 0x0016555B
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		this.FX.enabled = v > 0f;
		if (v > 0f)
		{
			this.SetIntensity(v);
		}
	}

	// Token: 0x04003D3F RID: 15679
	private T fx;
}
