using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007C0 RID: 1984
[Serializable]
public class AudioTrack2Clip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06003302 RID: 13058 RVA: 0x000F5E80 File Offset: 0x000F4080
	// (set) Token: 0x06003303 RID: 13059 RVA: 0x000F5E88 File Offset: 0x000F4088
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

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06003304 RID: 13060 RVA: 0x000F5E91 File Offset: 0x000F4091
	// (set) Token: 0x06003305 RID: 13061 RVA: 0x000F5E99 File Offset: 0x000F4099
	public bool Loop
	{
		get
		{
			return this.loop;
		}
		set
		{
			this.loop = value;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06003306 RID: 13062 RVA: 0x000F5EA2 File Offset: 0x000F40A2
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending | (this.loop ? ClipCaps.Looping : ClipCaps.None);
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x06003307 RID: 13063 RVA: 0x000F5EB3 File Offset: 0x000F40B3
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

	// Token: 0x06003308 RID: 13064 RVA: 0x000F5EE4 File Offset: 0x000F40E4
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		if (this.clip == null)
		{
			return Playable.Null;
		}
		AudioTrack2Behaviour audioTrack2Behaviour = (AudioTrack2Behaviour)this.template.Clone();
		audioTrack2Behaviour.clip = this.clip;
		audioTrack2Behaviour.loop = this.loop;
		return ScriptPlayable<AudioTrack2Behaviour>.Create(graph, audioTrack2Behaviour, 0);
	}

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	private AudioClip clip;

	// Token: 0x040028DA RID: 10458
	[SerializeField]
	private bool loop;

	// Token: 0x040028DB RID: 10459
	[SerializeField]
	private AudioTrack2Behaviour template = new AudioTrack2Behaviour();
}
