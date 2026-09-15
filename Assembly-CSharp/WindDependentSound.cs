using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class WindDependentSound : MonoBehaviour
{
	// Token: 0x0600222E RID: 8750 RVA: 0x000A0A0C File Offset: 0x0009EC0C
	public static void UpdateSounds()
	{
		if (WindDependentSound.activeSounds.Count == 0)
		{
			return;
		}
		float windIntensity = WindDependentSound.GetWindIntensity();
		for (int i = 0; i < WindDependentSound.activeSounds.Count; i++)
		{
			WindDependentSound.activeSounds[i].ApplyFromWind(windIntensity);
		}
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000A0A52 File Offset: 0x0009EC52
	private void OnEnable()
	{
		WindDependentSound.activeSounds.Add(this);
		this.PlaySound();
		this.ApplyFromWind(WindDependentSound.GetWindIntensity());
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000A0A70 File Offset: 0x0009EC70
	private void OnDisable()
	{
		WindDependentSound.activeSounds.Remove(this);
		this.StopSound();
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000A0A84 File Offset: 0x0009EC84
	private void PlaySound()
	{
		if (string.IsNullOrEmpty(this.soundId))
		{
			return;
		}
		this.soundHandler = LazyAudio.PlayAtGameObject(this.soundId, base.transform, this.spatial, false);
		if (this.soundHandler == null)
		{
			return;
		}
		if (this.continueFromSamePlace)
		{
			this.soundHandler.SetTime(this.cachedPlaybackTime);
		}
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000A0AE0 File Offset: 0x0009ECE0
	private void StopSound()
	{
		if (this.soundHandler != null && this.soundHandler.IsActive)
		{
			if (this.continueFromSamePlace)
			{
				this.cachedPlaybackTime = this.soundHandler.GetTime();
			}
			this.soundHandler.Stop();
		}
		this.soundHandler = null;
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000A0B30 File Offset: 0x0009ED30
	private void ApplyFromWind(float windIntensity)
	{
		if (this.soundHandler == null || !this.soundHandler.IsActive)
		{
			return;
		}
		this.soundHandler.SetVolume(this.maxVolume * windIntensity);
		this.soundHandler.SetPitch(Mathf.Lerp(this.pitchRange.x, this.pitchRange.y, windIntensity));
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000A0B8F File Offset: 0x0009ED8F
	private static float GetWindIntensity()
	{
		if (!(WeatherSystem.Instance != null))
		{
			return 0f;
		}
		return Mathf.Clamp01(WeatherSystem.Instance.WindValue);
	}

	// Token: 0x04001EBF RID: 7871
	private static readonly List<WindDependentSound> activeSounds = new List<WindDependentSound>();

	// Token: 0x04001EC0 RID: 7872
	[SerializeField]
	private string soundId;

	// Token: 0x04001EC1 RID: 7873
	[SerializeField]
	[Range(0f, 1f)]
	private float maxVolume = 1f;

	// Token: 0x04001EC2 RID: 7874
	[SerializeField]
	[Tooltip("Pitch at wind 0 (x) and wind 1 (y).")]
	private Vector2 pitchRange = new Vector2(0.7f, 1f);

	// Token: 0x04001EC3 RID: 7875
	[SerializeField]
	[Tooltip("Resume playback from the last position when the sound starts again. Works when an AudioSource is taken from the pool.")]
	private bool continueFromSamePlace;

	// Token: 0x04001EC4 RID: 7876
	[SerializeField]
	private SpatialType spatial;

	// Token: 0x04001EC5 RID: 7877
	private SoundHandler soundHandler;

	// Token: 0x04001EC6 RID: 7878
	private float cachedPlaybackTime;
}
