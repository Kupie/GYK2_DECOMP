using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000254 RID: 596
public class TalkingHeadPlayer
{
	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0004E5DA File Offset: 0x0004C7DA
	public bool IsPlaying
	{
		get
		{
			return this.isPlaying;
		}
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x0004E5E2 File Offset: 0x0004C7E2
	public TalkingHeadPlayer(AnimationComponent animationComponent, SkinChangerGK2 skinChanger, TalkingHeadPreset preset)
	{
		this.animationComponent = animationComponent;
		this.skinChanger = skinChanger;
		this.preset = preset;
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0004E606 File Offset: 0x0004C806
	public void Play(TalkingHeadPreset.TalkingHeadClipData clip)
	{
		if (clip == null || clip.frames == null || clip.frames.Count == 0)
		{
			return;
		}
		this.isPlaying = true;
		this.isSeries = false;
		this.BeginClip(clip);
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0004E638 File Offset: 0x0004C838
	public void PlaySeries()
	{
		if (this.preset == null || this.preset.clips == null || this.preset.clips.Count == 0)
		{
			return;
		}
		this.isPlaying = true;
		this.isSeries = true;
		this.BeginClip(this.PickRandomClip());
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0004E690 File Offset: 0x0004C890
	public void Stop()
	{
		this.isPlaying = false;
		this.isPaused = false;
		this.isSeries = false;
		this.currentClip = null;
		this.frameIndex = 0;
		this.timer = 0f;
		this.phase = TalkingHeadPlayer.Phase.None;
		this.currentFrame = 1;
		SkinChangerGK2 skinChangerGK = this.skinChanger;
		if (skinChangerGK == null)
		{
			return;
		}
		skinChangerGK.SetHeadFrameOverride(1);
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x0004E6EA File Offset: 0x0004C8EA
	public void Pause()
	{
		if (!this.isPlaying || this.isPaused)
		{
			return;
		}
		this.isPaused = true;
		if (this.currentFrame != 1)
		{
			this.currentFrame = 1;
			SkinChangerGK2 skinChangerGK = this.skinChanger;
			if (skinChangerGK == null)
			{
				return;
			}
			skinChangerGK.SetHeadFrameOverride(1);
		}
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0004E725 File Offset: 0x0004C925
	public void Resume()
	{
		this.isPaused = false;
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0004E730 File Offset: 0x0004C930
	public void Tick(float deltaTime)
	{
		if (!this.isPlaying || this.isPaused)
		{
			return;
		}
		global::AnimationState state = this.animationComponent.GetState();
		if (state > global::AnimationState.Walk)
		{
			if (this.currentFrame != 1)
			{
				this.currentFrame = 1;
				this.skinChanger.SetHeadFrameOverride(1);
			}
			return;
		}
		this.timer -= deltaTime;
		if (this.timer <= 0f)
		{
			this.Advance();
			return;
		}
		SkinChangerGK2 skinChangerGK = this.skinChanger;
		if (skinChangerGK == null)
		{
			return;
		}
		skinChangerGK.SetHeadFrameOverride(this.currentFrame);
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0004E7B3 File Offset: 0x0004C9B3
	private void BeginClip(TalkingHeadPreset.TalkingHeadClipData clip)
	{
		if (clip == null || clip.frames == null || clip.frames.Count == 0)
		{
			this.Stop();
			return;
		}
		this.currentClip = clip;
		this.frameIndex = 0;
		this.EnterFrame();
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0004E7E8 File Offset: 0x0004C9E8
	private void EnterFrame()
	{
		TalkingHeadPreset.TalkingHeadFrameData talkingHeadFrameData = this.currentClip.frames[this.frameIndex];
		this.phase = TalkingHeadPlayer.Phase.Frame;
		this.currentFrame = (int)talkingHeadFrameData.frame;
		this.timer = Mathf.Max(talkingHeadFrameData.duration, 0.001f);
		SkinChangerGK2 skinChangerGK = this.skinChanger;
		if (skinChangerGK == null)
		{
			return;
		}
		skinChangerGK.SetHeadFrameOverride(this.currentFrame);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0004E84C File Offset: 0x0004CA4C
	private void Advance()
	{
		if (this.phase == TalkingHeadPlayer.Phase.SeriesPause)
		{
			this.BeginClip(this.PickRandomClip());
			return;
		}
		if (this.currentClip == null || this.currentClip.frames == null)
		{
			this.Stop();
			return;
		}
		TalkingHeadPreset.TalkingHeadFrameData talkingHeadFrameData = this.currentClip.frames[this.frameIndex];
		if (this.phase == TalkingHeadPlayer.Phase.Frame && talkingHeadFrameData.pauseAfter > 0f)
		{
			this.phase = TalkingHeadPlayer.Phase.PauseAfter;
			this.currentFrame = 1;
			this.timer = talkingHeadFrameData.pauseAfter;
			SkinChangerGK2 skinChangerGK = this.skinChanger;
			if (skinChangerGK == null)
			{
				return;
			}
			skinChangerGK.SetHeadFrameOverride(1);
			return;
		}
		else
		{
			this.frameIndex++;
			if (this.frameIndex >= this.currentClip.frames.Count)
			{
				this.OnClipFinished();
				return;
			}
			this.EnterFrame();
			return;
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0004E918 File Offset: 0x0004CB18
	private void OnClipFinished()
	{
		if (!this.isSeries)
		{
			this.Stop();
			return;
		}
		float num = ((this.preset != null) ? this.preset.pauseBetweenClips : 0f);
		if (num <= 0f)
		{
			this.BeginClip(this.PickRandomClip());
			return;
		}
		this.phase = TalkingHeadPlayer.Phase.SeriesPause;
		this.currentFrame = 1;
		this.timer = num;
		SkinChangerGK2 skinChangerGK = this.skinChanger;
		if (skinChangerGK == null)
		{
			return;
		}
		skinChangerGK.SetHeadFrameOverride(1);
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0004E990 File Offset: 0x0004CB90
	private TalkingHeadPreset.TalkingHeadClipData PickRandomClip()
	{
		List<TalkingHeadPreset.TalkingHeadClipData> clips = this.preset.clips;
		if (clips == null || clips.Count == 0)
		{
			return null;
		}
		int num = global::UnityEngine.Random.Range(0, clips.Count);
		for (int i = 0; i < clips.Count; i++)
		{
			TalkingHeadPreset.TalkingHeadClipData talkingHeadClipData = clips[(num + i) % clips.Count];
			if (talkingHeadClipData != null && talkingHeadClipData.frames != null && talkingHeadClipData.frames.Count > 0)
			{
				return talkingHeadClipData;
			}
		}
		return null;
	}

	// Token: 0x04001203 RID: 4611
	private const float MinFrameDuration = 0.001f;

	// Token: 0x04001204 RID: 4612
	private readonly AnimationComponent animationComponent;

	// Token: 0x04001205 RID: 4613
	private readonly SkinChangerGK2 skinChanger;

	// Token: 0x04001206 RID: 4614
	private readonly TalkingHeadPreset preset;

	// Token: 0x04001207 RID: 4615
	private bool isPlaying;

	// Token: 0x04001208 RID: 4616
	private bool isPaused;

	// Token: 0x04001209 RID: 4617
	private bool isSeries;

	// Token: 0x0400120A RID: 4618
	private TalkingHeadPreset.TalkingHeadClipData currentClip;

	// Token: 0x0400120B RID: 4619
	private int frameIndex;

	// Token: 0x0400120C RID: 4620
	private float timer;

	// Token: 0x0400120D RID: 4621
	private TalkingHeadPlayer.Phase phase;

	// Token: 0x0400120E RID: 4622
	private int currentFrame = 1;

	// Token: 0x02000255 RID: 597
	private enum Phase
	{
		// Token: 0x04001210 RID: 4624
		None,
		// Token: 0x04001211 RID: 4625
		Frame,
		// Token: 0x04001212 RID: 4626
		PauseAfter,
		// Token: 0x04001213 RID: 4627
		SeriesPause
	}
}
