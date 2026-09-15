using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200034D RID: 845
public class HitZone : MonoBehaviour
{
	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06001651 RID: 5713 RVA: 0x0006B8E3 File Offset: 0x00069AE3
	public AnimationComponentBase AnimationComponent
	{
		get
		{
			return this.animationComponent;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06001652 RID: 5714 RVA: 0x0006B8EB File Offset: 0x00069AEB
	public HitZoneType ZoneType
	{
		get
		{
			return this.zoneType;
		}
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x0006B8F3 File Offset: 0x00069AF3
	private void Start()
	{
		this.animationComponent = base.GetComponentInParent<AnimationComponentBase>();
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x0006B904 File Offset: 0x00069B04
	[return: TupleElementNames(new string[] { "result", "reaction" })]
	public ValueTuple<HitResult, HitZoneReaction> ProcessHit(DamageSourceType sourceType, AttackContext context, Vector3 hitPosition)
	{
		HitZoneReaction hitZoneReaction = this.FindReaction(sourceType);
		if (hitZoneReaction == null)
		{
			return new ValueTuple<HitResult, HitZoneReaction>(HitResult.Pass(1f), null);
		}
		return new ValueTuple<HitResult, HitZoneReaction>(hitZoneReaction.Execute(context, hitPosition), hitZoneReaction);
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x0006B93C File Offset: 0x00069B3C
	[CanBeNull]
	private HitZoneReaction FindReaction(DamageSourceType sourceType)
	{
		for (int i = 0; i < this.reactionPresets.Count; i++)
		{
			HitZoneReactionsPreset hitZoneReactionsPreset = this.reactionPresets[i];
			if (!(hitZoneReactionsPreset == null))
			{
				HitZoneReaction hitZoneReaction = hitZoneReactionsPreset.FindReaction(sourceType);
				if (hitZoneReaction != null)
				{
					return hitZoneReaction;
				}
			}
		}
		HitZoneReaction hitZoneReaction2 = null;
		for (int j = 0; j < this.reactions.Count; j++)
		{
			HitZoneReaction hitZoneReaction3 = this.reactions[j];
			if (hitZoneReaction3 != null)
			{
				if (hitZoneReaction3.sourceType == sourceType)
				{
					return hitZoneReaction3;
				}
				if (hitZoneReaction2 == null && hitZoneReaction3.sourceType == DamageSourceType.Any)
				{
					hitZoneReaction2 = hitZoneReaction3;
				}
			}
		}
		return hitZoneReaction2;
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x0006B9D0 File Offset: 0x00069BD0
	public bool CheckDoorDirectionBlock(AnimationComponentBase attackerAnimation)
	{
		if (!this.animationComponent || !attackerAnimation)
		{
			return false;
		}
		if (this.zoneType != HitZoneType.Door || this.zoneType != HitZoneType.DoorFrontArea)
		{
			return false;
		}
		Direction direction = this.animationComponent.GetDirection().ConvertFromVector2();
		Direction direction2 = attackerAnimation.GetDirection().ConvertFromVector2();
		return direction.OppositeDir() == direction2;
	}

	// Token: 0x06001657 RID: 5719 RVA: 0x0006BA2C File Offset: 0x00069C2C
	public void PlayHitEffect(HitResult hitResult, HitZoneReaction reaction, Vector3 position)
	{
		if (reaction == null)
		{
			return;
		}
		HitEffectConfig hitEffectConfig = reaction.effects.Find((HitEffectConfig e) => e.resultType == hitResult.type);
		if (hitEffectConfig != null)
		{
			reaction.PlayEffect(hitEffectConfig, position);
		}
	}

	// Token: 0x040016A3 RID: 5795
	[SerializeField]
	private HitZoneType zoneType;

	// Token: 0x040016A4 RID: 5796
	[SerializeField]
	[Tooltip("Optional shared reaction presets. Looked up in order; first matching sourceType wins.")]
	private List<HitZoneReactionsPreset> reactionPresets = new List<HitZoneReactionsPreset>();

	// Token: 0x040016A5 RID: 5797
	[SerializeField]
	[Tooltip("Local reactions used when presets have no match (or when no presets are assigned).")]
	private List<HitZoneReaction> reactions = new List<HitZoneReaction>();

	// Token: 0x040016A6 RID: 5798
	private AnimationComponentBase animationComponent;
}
