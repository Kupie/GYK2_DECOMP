using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007C9 RID: 1993
[Serializable]
public class AddWgoDataClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x0600333F RID: 13119 RVA: 0x00028294 File Offset: 0x00026494
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x000F6AC5 File Offset: 0x000F4CC5
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<AddWgoDataBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x040028F9 RID: 10489
	public AddWgoDataBehaviour template = new AddWgoDataBehaviour();
}
