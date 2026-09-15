using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003CE RID: 974
[Serializable]
public class PlaySoundAction : ConditionalDrawerActionBase
{
	// Token: 0x06001A00 RID: 6656 RVA: 0x0007A044 File Offset: 0x00078244
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (!conditionMet || string.IsNullOrEmpty(this.soundId))
		{
			return;
		}
		Transform transform = ((context.WgoPart != null) ? context.WgoPart.transform : null);
		if (transform == null)
		{
			return;
		}
		LazyAudio.PlayAtGameObject(this.soundId, transform, this.spatial, true);
	}

	// Token: 0x0400194B RID: 6475
	[Tooltip("Sound id to play")]
	public string soundId;

	// Token: 0x0400194C RID: 6476
	[Tooltip("How the sound is positioned relative to the WGO")]
	public SpatialType spatial;
}
