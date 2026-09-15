using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000353 RID: 851
[CreateAssetMenu(menuName = "GK2/Fighting/HitZone Reactions Preset", fileName = "HitZoneReactionsPreset")]
public class HitZoneReactionsPreset : ScriptableObject
{
	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600165F RID: 5727 RVA: 0x0006BB75 File Offset: 0x00069D75
	public IReadOnlyList<HitZoneReaction> Reactions
	{
		get
		{
			return this.reactions;
		}
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x0006BB80 File Offset: 0x00069D80
	[CanBeNull]
	public HitZoneReaction FindReaction(DamageSourceType sourceType)
	{
		HitZoneReaction hitZoneReaction = null;
		for (int i = 0; i < this.reactions.Count; i++)
		{
			HitZoneReaction hitZoneReaction2 = this.reactions[i];
			if (hitZoneReaction2 != null)
			{
				if (hitZoneReaction2.sourceType == sourceType)
				{
					return hitZoneReaction2;
				}
				if (hitZoneReaction == null && hitZoneReaction2.sourceType == DamageSourceType.Any)
				{
					hitZoneReaction = hitZoneReaction2;
				}
			}
		}
		return hitZoneReaction;
	}

	// Token: 0x040016BA RID: 5818
	[SerializeField]
	private List<HitZoneReaction> reactions = new List<HitZoneReaction>();
}
