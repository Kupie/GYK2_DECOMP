using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007C6 RID: 1990
[Serializable]
public class VoiceOverBehaviour : PlayableBehaviour
{
	// Token: 0x0600332E RID: 13102 RVA: 0x000F67F8 File Offset: 0x000F49F8
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (info.effectiveWeight <= 0f)
		{
			return;
		}
		if (Application.isPlaying)
		{
			if (this.isPlaying)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.voiceOverId) || !LazyAudio.IsInitialized)
			{
				return;
			}
			LazyAudio.VoiceOverPlayer.Play(this.voiceOverId, null);
			this.isPlaying = true;
			return;
		}
		else
		{
			this.previewSource = playerData as AudioSource;
			if (this.previewSource == null || this.clip == null)
			{
				return;
			}
			if (!this.isPlaying)
			{
				this.previewSource.clip = this.clip;
				this.previewSource.loop = false;
				this.previewSource.time = Mathf.Clamp((float)playable.GetTime<Playable>(), 0f, this.clip.length);
				this.previewSource.Play();
				this.isPlaying = true;
			}
			this.previewSource.volume = Mathf.Clamp01(info.effectiveWeight);
			return;
		}
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x000F68F0 File Offset: 0x000F4AF0
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		this.StopPlayback();
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x000F68F0 File Offset: 0x000F4AF0
	public override void OnGraphStop(Playable playable)
	{
		this.StopPlayback();
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x000F68F8 File Offset: 0x000F4AF8
	private void StopPlayback()
	{
		if (!this.isPlaying)
		{
			return;
		}
		if (!Application.isPlaying && this.previewSource != null)
		{
			this.previewSource.Stop();
		}
		this.isPlaying = false;
	}

	// Token: 0x040028EC RID: 10476
	[NonSerialized]
	public AudioClip clip;

	// Token: 0x040028ED RID: 10477
	[NonSerialized]
	public string voiceOverId;

	// Token: 0x040028EE RID: 10478
	private AudioSource previewSource;

	// Token: 0x040028EF RID: 10479
	private bool isPlaying;
}
