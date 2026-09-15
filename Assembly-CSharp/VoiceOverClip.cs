using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007C7 RID: 1991
[Serializable]
public class VoiceOverClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06003333 RID: 13107 RVA: 0x000F692A File Offset: 0x000F4B2A
	// (set) Token: 0x06003334 RID: 13108 RVA: 0x000F6932 File Offset: 0x000F4B32
	public AudioClip Clip
	{
		get
		{
			return this.clip;
		}
		set
		{
			this.clip = value;
		}
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06003335 RID: 13109 RVA: 0x000F693B File Offset: 0x000F4B3B
	// (set) Token: 0x06003336 RID: 13110 RVA: 0x000F6943 File Offset: 0x000F4B43
	public string VoiceOverId
	{
		get
		{
			return this.voiceOverId;
		}
		set
		{
			this.voiceOverId = value;
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06003337 RID: 13111 RVA: 0x000F694C File Offset: 0x000F4B4C
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.ClipIn | ClipCaps.Blending;
		}
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06003338 RID: 13112 RVA: 0x000F6950 File Offset: 0x000F4B50
	public override double duration
	{
		get
		{
			if (this.clip == null)
			{
				return base.duration;
			}
			return (double)this.clip.samples / (double)this.clip.frequency;
		}
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x000F6980 File Offset: 0x000F4B80
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		VoiceOverBehaviour voiceOverBehaviour = (VoiceOverBehaviour)this.template.Clone();
		voiceOverBehaviour.clip = this.clip;
		voiceOverBehaviour.voiceOverId = this.voiceOverId;
		return ScriptPlayable<VoiceOverBehaviour>.Create(graph, voiceOverBehaviour, 0);
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x000F69C3 File Offset: 0x000F4BC3
	public void ApplyFromClipAsset()
	{
		if (this.clip == null)
		{
			return;
		}
		this.voiceOverId = this.clip.name;
	}

	// Token: 0x040028F0 RID: 10480
	[SerializeField]
	private AudioClip clip;

	// Token: 0x040028F1 RID: 10481
	[SerializeField]
	private string voiceOverId;

	// Token: 0x040028F2 RID: 10482
	[SerializeField]
	private VoiceOverBehaviour template = new VoiceOverBehaviour();
}
