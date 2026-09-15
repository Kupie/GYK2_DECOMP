using System;

// Token: 0x020005DE RID: 1502
[Serializable]
public class DeliveryOrder : OrderBase
{
	// Token: 0x060027B8 RID: 10168 RVA: 0x000B9D6C File Offset: 0x000B7F6C
	public DeliveryOrder(SGuid targetWgoUniqueId, Item item)
		: base(targetWgoUniqueId, item)
	{
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x000B9E77 File Offset: 0x000B8077
	public override void ExecuteOrder(IOrderExecutor orderExecutor)
	{
		orderExecutor.RemoveItem(base.Item);
		base.ZombieWgoData.CraftableObjectCraftInventory.AddItemToInventory(new Item(base.Item.id, base.Item.Count), null, false);
	}

	// Token: 0x060027BA RID: 10170 RVA: 0x000B9EB4 File Offset: 0x000B80B4
	public override bool CanOrderBeExecuted(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		bool flag = orderExecutor.HasItem(base.Item);
		reasonIfNot = (flag ? string.Empty : "player_talk_not_enough_items");
		return flag;
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x000B9EE0 File Offset: 0x000B80E0
	public override string GetInteractionHint()
	{
		return "hint_put";
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000B9EE7 File Offset: 0x000B80E7
	public override string GetStatusIcon()
	{
		return "craft_status_wait";
	}
}
