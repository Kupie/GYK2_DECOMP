using System;

// Token: 0x020005E2 RID: 1506
[Serializable]
public class PickupOrder : OrderBase
{
	// Token: 0x060027D3 RID: 10195 RVA: 0x000B9D6C File Offset: 0x000B7F6C
	public PickupOrder(SGuid targetWgoUniqueId, Item item)
		: base(targetWgoUniqueId, item)
	{
	}

	// Token: 0x060027D4 RID: 10196 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override int GetPriority()
	{
		return 1;
	}

	// Token: 0x060027D5 RID: 10197 RVA: 0x000BA004 File Offset: 0x000B8204
	public override void ExecuteOrder(IOrderExecutor orderExecutor)
	{
		orderExecutor.AddItem(base.Item);
		base.ZombieWgoData.CraftableObjectCraftInventory.RemoveItemById(base.Item.id, base.Item.Count, null, null, false);
		WgoData attachedWgoData = base.ZombieWgoData.AttachedWgoData;
		if (attachedWgoData.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp)
		{
			attachedWgoData.CraftComponent.TryFinishCurCraft();
			attachedWgoData.DropStoredTechPoints();
		}
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x000BA074 File Offset: 0x000B8274
	public override bool CanOrderBeExecuted(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		bool flag = orderExecutor.CanAddItem(base.Item);
		reasonIfNot = (flag ? string.Empty : "player_talk_not_enough_space");
		return flag;
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x000B9E69 File Offset: 0x000B8069
	public override string GetInteractionHint()
	{
		return "hint_take";
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000B9E70 File Offset: 0x000B8070
	public override string GetStatusIcon()
	{
		return "craft_status_not_enough_items";
	}
}
