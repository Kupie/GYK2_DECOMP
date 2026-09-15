using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000359 RID: 857
public class Weapon : MonoBehaviour
{
	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060016A4 RID: 5796 RVA: 0x0006CE58 File Offset: 0x0006B058
	public List<IDamageDealer> Dealers
	{
		get
		{
			return this.dealers ?? base.GetComponentsInChildren<IDamageDealer>(true).ToList<IDamageDealer>();
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x060016A5 RID: 5797 RVA: 0x0006CE70 File Offset: 0x0006B070
	public ItemType ItemType
	{
		get
		{
			return this.itemType;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060016A6 RID: 5798 RVA: 0x0006CE78 File Offset: 0x0006B078
	// (set) Token: 0x060016A7 RID: 5799 RVA: 0x0006CE9E File Offset: 0x0006B09E
	public ItemDef ItemDef
	{
		get
		{
			if (this.itemDef == null)
			{
				this.itemDef = GameBalance.Me.GetData<ItemDef>(this.defaultItemDefId);
			}
			return this.itemDef;
		}
		set
		{
			this.itemDef = value;
		}
	}

	// Token: 0x040016E2 RID: 5858
	public global::AnimationState animState = global::AnimationState.AttackCommon;

	// Token: 0x040016E3 RID: 5859
	public string defaultItemDefId;

	// Token: 0x040016E4 RID: 5860
	[SerializeField]
	private ItemType itemType;

	// Token: 0x040016E5 RID: 5861
	private List<IDamageDealer> dealers;

	// Token: 0x040016E6 RID: 5862
	private ItemDef itemDef;
}
