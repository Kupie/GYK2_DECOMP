using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class ModdedVoiceOverPlayer : VoiceOverPlayer
{
	// Token: 0x060000A7 RID: 167 RVA: 0x00004A70 File Offset: 0x00002C70
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void RegisterFactory()
	{
		LazyAudio.VoiceOverPlayerFactory = () => new ModdedVoiceOverPlayer();
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00004A98 File Offset: 0x00002C98
	public override void Play(string id, VoiceID voiceId = null)
	{
		if (!VoiceOverSettings.IsEnabled)
		{
			return;
		}
		if (VoiceOverPlayer.IsMuted(id))
		{
			return;
		}
		this.Stop();
		if (VoiceOverModLoader.TryLoadClip(id, out this.audioClip))
		{
			this.ownsModAudioClip = true;
			this.voiceClipData = null;
			this.currentLocalKey = id;
			this.currentVoiceId = voiceId;
			base.PlayLoadedClip();
			return;
		}
		base.Play(id, voiceId);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00004AF8 File Offset: 0x00002CF8
	public override void Stop()
	{
		AudioClip audioClip = (this.ownsModAudioClip ? this.audioClip : null);
		base.Stop();
		if (audioClip != null)
		{
			VoiceOverModLoader.ReleaseClip(audioClip);
		}
		this.ownsModAudioClip = false;
	}

	// Token: 0x04000063 RID: 99
	private bool ownsModAudioClip;
}
