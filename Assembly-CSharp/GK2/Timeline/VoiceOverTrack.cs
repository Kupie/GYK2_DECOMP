using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace GK2.Timeline
{
	// Token: 0x02000B52 RID: 2898
	[TrackColor(0.85f, 0.45f, 0.2f)]
	[TrackClipType(typeof(VoiceOverClip))]
	[TrackBindingType(typeof(AudioSource))]
	public class VoiceOverTrack : TrackAsset
	{
		// Token: 0x06004CE2 RID: 19682 RVA: 0x0016A3AD File Offset: 0x001685AD
		protected override void OnCreateClip(TimelineClip clip)
		{
			base.OnCreateClip(clip);
			VoiceOverTrack.SyncClipFromAudio(clip);
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x0016A3BC File Offset: 0x001685BC
		public TimelineClip CreateClip(AudioClip audioClip)
		{
			if (audioClip == null)
			{
				return null;
			}
			TimelineClip timelineClip = base.CreateDefaultClip();
			VoiceOverClip voiceOverClip = timelineClip.asset as VoiceOverClip;
			if (voiceOverClip != null)
			{
				voiceOverClip.Clip = audioClip;
				VoiceOverTrack.ApplyAudioToTimelineClip(timelineClip, voiceOverClip);
			}
			return timelineClip;
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x0016A3FC File Offset: 0x001685FC
		public static void SyncClipFromAudio(TimelineClip timelineClip)
		{
			VoiceOverClip voiceOverClip = ((timelineClip != null) ? timelineClip.asset : null) as VoiceOverClip;
			if (voiceOverClip == null)
			{
				return;
			}
			if (voiceOverClip.Clip == null)
			{
				return;
			}
			VoiceOverTrack.ApplyAudioToTimelineClip(timelineClip, voiceOverClip);
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x0016A435 File Offset: 0x00168635
		public static void ApplyAudioToTimelineClip(TimelineClip timelineClip, VoiceOverClip voiceOverClip)
		{
			voiceOverClip.ApplyFromClipAsset();
			timelineClip.duration = (double)voiceOverClip.Clip.length;
			timelineClip.displayName = voiceOverClip.VoiceOverId;
		}
	}
}
