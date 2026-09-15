using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007BE RID: 1982
[Serializable]
public class AnimationControlClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x060032F9 RID: 13049 RVA: 0x00028294 File Offset: 0x00026494
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x000F5C66 File Offset: 0x000F3E66
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<AnimationControlBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x040028D0 RID: 10448
	public AnimationControlBehaviour template = new AnimationControlBehaviour();
}
