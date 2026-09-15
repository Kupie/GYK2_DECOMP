using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000256 RID: 598
[CreateAssetMenu(fileName = "TalkingHeadPreset", menuName = "GK2/Animation/TalkingHeadPreset")]
public class TalkingHeadPreset : ScriptableObject
{
	// Token: 0x04001214 RID: 4628
	[Min(0f)]
	[Tooltip("Pause between clips when playing a series")]
	public float pauseBetweenClips;

	// Token: 0x04001215 RID: 4629
	public List<TalkingHeadPreset.TalkingHeadClipData> clips = new List<TalkingHeadPreset.TalkingHeadClipData>();

	// Token: 0x02000257 RID: 599
	[Serializable]
	public class TalkingHeadClipData
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0004EA14 File Offset: 0x0004CC14
		public float TotalTime
		{
			get
			{
				List<TalkingHeadPreset.TalkingHeadFrameData> list = this.frames;
				if (list == null)
				{
					return 0f;
				}
				return list.Sum((TalkingHeadPreset.TalkingHeadFrameData f) => f.duration + f.pauseAfter);
			}
		}

		// Token: 0x04001216 RID: 4630
		public List<TalkingHeadPreset.TalkingHeadFrameData> frames = new List<TalkingHeadPreset.TalkingHeadFrameData>();
	}

	// Token: 0x02000259 RID: 601
	[Serializable]
	public class TalkingHeadFrameData
	{
		// Token: 0x04001219 RID: 4633
		public TalkingHeadPreset.HeadFrame frame = TalkingHeadPreset.HeadFrame.Frame01;

		// Token: 0x0400121A RID: 4634
		[Min(0f)]
		public float duration = 0.08f;

		// Token: 0x0400121B RID: 4635
		[Min(0f)]
		[Tooltip("Hold the original frame this long after this frame")]
		public float pauseAfter;
	}

	// Token: 0x0200025A RID: 602
	public enum HeadFrame
	{
		// Token: 0x0400121D RID: 4637
		Frame01 = 1,
		// Token: 0x0400121E RID: 4638
		Frame02,
		// Token: 0x0400121F RID: 4639
		Frame03
	}
}
