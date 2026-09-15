using System;
using UnityEngine;

// Token: 0x020003D4 RID: 980
[Serializable]
public class SetSkinAction : ConditionalDrawerActionBase
{
	// Token: 0x06001A0E RID: 6670 RVA: 0x0007A358 File Offset: 0x00078558
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		AnimationComponent animationComponent = ((context.WgoPart.AnimationComponent == null) ? context.WgoPart.GetComponentInChildren<AnimationComponent>() : (context.WgoPart.AnimationComponent as AnimationComponent));
		if (animationComponent == null)
		{
			Debug.LogError("AnimationComponent not found on WGO");
		}
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.ChangeSkinPreset(conditionMet ? this.skinOnTrue : this.skinOnFalse);
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x0007A3C4 File Offset: 0x000785C4
	public override void Reset(ConditionalDrawerContext context)
	{
		AnimationComponent animationComponent = ((context.WgoPart.AnimationComponent == null) ? context.WgoPart.GetComponentInChildren<AnimationComponent>() : (context.WgoPart.AnimationComponent as AnimationComponent));
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.ChangeSkinPreset(this.isOnTrueDefault ? this.skinOnTrue : this.skinOnFalse);
	}

	// Token: 0x0400195D RID: 6493
	[Tooltip("Skin preset applying if condition is true")]
	public SkinPresetGK2 skinOnTrue;

	// Token: 0x0400195E RID: 6494
	[Tooltip("Skin preset applying if condition is false")]
	public SkinPresetGK2 skinOnFalse;

	// Token: 0x0400195F RID: 6495
	public bool isOnTrueDefault;
}
