using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007BF RID: 1983
[Serializable]
public class AudioTrack2Behaviour : PlayableBehaviour
{
	// Token: 0x060032FC RID: 13052 RVA: 0x000F5C90 File Offset: 0x000F3E90
	public float GetFadeMultiplier(Playable playable)
	{
		if (this.fadeDuration <= 0f)
		{
			return 1f;
		}
		double time = playable.GetTime<Playable>();
		double duration = playable.GetDuration<Playable>();
		if (time < (double)this.fadeDuration)
		{
			return this.fadeInEase.Evaluate(Mathf.Clamp01((float)(time / (double)this.fadeDuration)));
		}
		if (duration > (double)this.fadeDuration && time > duration - (double)this.fadeDuration)
		{
			return this.fadeOutEase.Evaluate(Mathf.Clamp01((float)((time - duration + (double)this.fadeDuration) / (double)this.fadeDuration)));
		}
		return 1f;
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x000F5D24 File Offset: 0x000F3F24
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		this.audioSource = playerData as AudioSource;
		if (this.audioSource == null || this.clip == null || info.effectiveWeight <= 0f)
		{
			return;
		}
		if (!this.isPlaying)
		{
			this.audioSource.clip = this.clip;
			this.audioSource.loop = this.loop;
			this.audioSource.time = Mathf.Clamp((float)playable.GetTime<Playable>(), 0f, this.clip.length);
			this.audioSource.Play();
			this.isPlaying = true;
		}
		this.audioSource.volume = Mathf.Clamp01(this.volume * this.GetFadeMultiplier(playable) * info.effectiveWeight);
	}

	// Token: 0x060032FE RID: 13054 RVA: 0x000F5DF1 File Offset: 0x000F3FF1
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		this.StopPlayback();
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x000F5DF1 File Offset: 0x000F3FF1
	public override void OnGraphStop(Playable playable)
	{
		this.StopPlayback();
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x000F5DF9 File Offset: 0x000F3FF9
	private void StopPlayback()
	{
		if (!this.isPlaying || this.audioSource == null)
		{
			return;
		}
		this.audioSource.Stop();
		this.isPlaying = false;
	}

	// Token: 0x040028D1 RID: 10449
	[NonSerialized]
	public AudioClip clip;

	// Token: 0x040028D2 RID: 10450
	[NonSerialized]
	public bool loop;

	// Token: 0x040028D3 RID: 10451
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x040028D4 RID: 10452
	public float fadeDuration;

	// Token: 0x040028D5 RID: 10453
	public AnimationCurve fadeInEase = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040028D6 RID: 10454
	public AnimationCurve fadeOutEase = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x040028D7 RID: 10455
	private AudioSource audioSource;

	// Token: 0x040028D8 RID: 10456
	private bool isPlaying;
}
