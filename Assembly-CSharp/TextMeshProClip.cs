using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007C4 RID: 1988
[Serializable]
public class TextMeshProClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06003320 RID: 13088 RVA: 0x00028294 File Offset: 0x00026494
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x000F65A3 File Offset: 0x000F47A3
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<TextMeshProBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x040028EA RID: 10474
	public TextMeshProBehaviour template = new TextMeshProBehaviour();
}
