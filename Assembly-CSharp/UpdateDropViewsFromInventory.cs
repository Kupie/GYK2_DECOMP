using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D5 RID: 981
[Serializable]
public class UpdateDropViewsFromInventory : ConditionalDrawerActionBase
{
	// Token: 0x06001A11 RID: 6673 RVA: 0x0007A424 File Offset: 0x00078624
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (context.WgoData == null)
		{
			return;
		}
		Inventory inventory = context.WgoData.Inventory;
		int num = inventory.Data.Inventory.Count - 1;
		for (int i = 0; i < this.bigDrops.Count; i++)
		{
			if (!(this.bigDrops[i] == null))
			{
				if (i > num)
				{
					this.bigDrops[i].Deactivate();
				}
				else
				{
					this.bigDrops[i].Activate(inventory.Data.Inventory[i].Definition.iconId);
				}
			}
		}
	}

	// Token: 0x04001960 RID: 6496
	[Tooltip("List of Bid drops to update")]
	public List<DropViewAtomMesh> bigDrops = new List<DropViewAtomMesh>();
}
