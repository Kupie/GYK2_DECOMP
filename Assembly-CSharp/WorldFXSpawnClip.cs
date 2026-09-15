using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x020007CB RID: 1995
[Serializable]
public class WorldFXSpawnClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06003345 RID: 13125 RVA: 0x000F6B47 File Offset: 0x000F4D47
	public override double duration
	{
		get
		{
			return 0.0;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06003346 RID: 13126 RVA: 0x00028294 File Offset: 0x00026494
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x000F6B52 File Offset: 0x000F4D52
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<WorldFXSpawnBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x040028FD RID: 10493
	public WorldFXSpawnBehaviour template = new WorldFXSpawnBehaviour();
}
