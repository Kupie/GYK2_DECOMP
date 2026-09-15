using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace GK2.Timeline
{
	// Token: 0x02000B4F RID: 2895
	[TrackColor(0.2f, 0.6f, 0.9f)]
	[TrackClipType(typeof(AudioTrack2Clip))]
	[TrackBindingType(typeof(AudioSource))]
	public class AudioTrack2 : TrackAsset
	{
		// Token: 0x06004CDE RID: 19678 RVA: 0x0016A360 File Offset: 0x00168560
		public TimelineClip CreateClip(AudioClip clip)
		{
			if (clip == null)
			{
				return null;
			}
			TimelineClip timelineClip = base.CreateDefaultClip();
			AudioTrack2Clip audioTrack2Clip = timelineClip.asset as AudioTrack2Clip;
			if (audioTrack2Clip != null)
			{
				audioTrack2Clip.Clip = clip;
			}
			timelineClip.duration = (double)clip.length;
			timelineClip.displayName = clip.name;
			return timelineClip;
		}
	}
}
