using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000350 RID: 848
[Serializable]
public class HitZoneReaction
{
	// Token: 0x0600165B RID: 5723 RVA: 0x0006BAA0 File Offset: 0x00069CA0
	public HitResult Execute(AttackContext context, Vector3 hitPosition)
	{
		HitResult hitResult;
		switch (this.reactionType)
		{
		case HitReactionType.Pass:
			hitResult = HitResult.Pass(this.damageMultiplier);
			break;
		case HitReactionType.Block:
			hitResult = HitResult.Blocked();
			break;
		case HitReactionType.Absorb:
			hitResult = HitResult.Absorbed();
			break;
		case HitReactionType.Deflect:
			hitResult = HitResult.Deflected();
			break;
		case HitReactionType.MarkZone:
			hitResult = HitResult.ZoneMarked();
			break;
		default:
			hitResult = HitResult.Pass(1f);
			break;
		}
		return hitResult;
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x0006BB0C File Offset: 0x00069D0C
	public void PlayEffect(HitEffectConfig effectConfig, Vector3 position)
	{
		if (!string.IsNullOrEmpty(effectConfig.fxName))
		{
			WorldFX.Spawn(position, effectConfig.fxName, null, default(Vector3));
		}
		if (!string.IsNullOrEmpty(effectConfig.soundId))
		{
			LazyAudio.PlayAtPos(effectConfig.soundId, position);
		}
	}

	// Token: 0x040016AD RID: 5805
	public DamageSourceType sourceType;

	// Token: 0x040016AE RID: 5806
	public HitReactionType reactionType;

	// Token: 0x040016AF RID: 5807
	public float damageMultiplier = 1f;

	// Token: 0x040016B0 RID: 5808
	[Header("Effects")]
	public List<HitEffectConfig> effects = new List<HitEffectConfig>();
}
