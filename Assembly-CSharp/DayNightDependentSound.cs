using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
[RequireComponent(typeof(DayNightLight))]
public class DayNightDependentSound : MonoBehaviour
{
	// Token: 0x06002111 RID: 8465 RVA: 0x0009C3CA File Offset: 0x0009A5CA
	private void Awake()
	{
		if (this.dayNightLight == null)
		{
			this.dayNightLight = base.GetComponent<DayNightLight>();
		}
		DayNightLight dayNightLight = this.dayNightLight;
		dayNightLight.OnLightIntensityChanged = (Action<float>)Delegate.Combine(dayNightLight.OnLightIntensityChanged, new Action<float>(this.UpdateSound));
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x0009C408 File Offset: 0x0009A608
	private void OnDestroy()
	{
		SoundHandler soundHandler = this.soundHandler;
		if (soundHandler != null)
		{
			soundHandler.Stop();
		}
		if (this.dayNightLight != null)
		{
			DayNightLight dayNightLight = this.dayNightLight;
			dayNightLight.OnLightIntensityChanged = (Action<float>)Delegate.Remove(dayNightLight.OnLightIntensityChanged, new Action<float>(this.UpdateSound));
		}
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x0009C45C File Offset: 0x0009A65C
	private void OnEnable()
	{
		if (this.soundHandler == null)
		{
			this.soundHandler = LazyAudio.PlayAtGameObject(this.soundId, base.transform, this.spatial, true);
			return;
		}
		this.soundHandler.UnPause();
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x0009C491 File Offset: 0x0009A691
	private void OnDisable()
	{
		SoundHandler soundHandler = this.soundHandler;
		if (soundHandler == null)
		{
			return;
		}
		soundHandler.Pause();
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x0009C4A4 File Offset: 0x0009A6A4
	private void UpdateSound(float intensity)
	{
		if (this.soundHandler == null)
		{
			return;
		}
		this.soundHandler.SetVolume(Mathf.Abs(intensity));
	}

	// Token: 0x04001DAE RID: 7598
	[SerializeField]
	private string soundId;

	// Token: 0x04001DAF RID: 7599
	[SerializeField]
	private SpatialType spatial = SpatialType.sound1D;

	// Token: 0x04001DB0 RID: 7600
	[SerializeField]
	private DayNightLight dayNightLight;

	// Token: 0x04001DB1 RID: 7601
	private SoundHandler soundHandler;
}
