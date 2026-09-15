using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007C2 RID: 1986
[Serializable]
public class EnvironmentControlClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x0600330D RID: 13069 RVA: 0x00028294 File Offset: 0x00026494
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x000F5FD8 File Offset: 0x000F41D8
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<EnvironmentControlBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x040028E0 RID: 10464
	public EnvironmentControlBehaviour template = new EnvironmentControlBehaviour();
}
