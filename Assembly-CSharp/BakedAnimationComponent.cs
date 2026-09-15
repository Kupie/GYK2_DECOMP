using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class BakedAnimationComponent : AnimationComponentBase
{
	// Token: 0x06000EF0 RID: 3824 RVA: 0x0004D950 File Offset: 0x0004BB50
	protected override void InitInternal(SkinPresetGK2 skinPreset)
	{
		this.skinChanger = new SkinChangerGK2(base.gameObject, false);
		this.skinChanger.ApplySkin(this.bakeSkinPreset, null);
	}

	// Token: 0x040011DA RID: 4570
	[SerializeField]
	protected SkinPresetGK2 bakeSkinPreset;

	// Token: 0x040011DB RID: 4571
	private SkinChangerGK2 skinChanger;
}
