using System;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public interface IFightDecal
{
	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06001220 RID: 4640 RVA: 0x00028294 File Offset: 0x00026494
	int SortingGroup
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06001221 RID: 4641 RVA: 0x00059250 File Offset: 0x00057450
	float GroupYOffset
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001222 RID: 4642
	IFightDecal SpawnDecal(Vector3 position, Direction orientation, string customDeathEffectId = "");
}
