using System;

// Token: 0x020005DD RID: 1501
[Serializable]
public class ConveyorPickupOrder : OrderBase
{
	// Token: 0x060027B1 RID: 10161 RVA: 0x000B9D6C File Offset: 0x000B7F6C
	public ConveyorPickupOrder(SGuid targetWgoUniqueId, Item item)
		: base(targetWgoUniqueId, item)
	{
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x060027B2 RID: 10162 RVA: 0x000B9D76 File Offset: 0x000B7F76
	public WgoData TargetWgoData
	{
		get
		{
			return MainGame.WorldData.GetWgoData(base.TargetWgoUniqueId);
		}
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override int GetPriority()
	{
		return 1;
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x000B9D88 File Offset: 0x000B7F88
	public override void ExecuteOrder(IOrderExecutor orderExecutor)
	{
		orderExecutor.AddItem(base.Item);
		WgoData targetWgoData = this.TargetWgoData;
		if (targetWgoData != null)
		{
			targetWgoData.Inventory.RemoveItemById(base.Item.id, base.Item.Count, null, null, false);
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(targetWgoData.UniqueId);
			if (wgoViewGlobal != null && wgoViewGlobal.MainWgoPart != null)
			{
				wgoViewGlobal.MainWgoPart.UpdateDropViewFromInventory(null);
			}
		}
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x000B9E00 File Offset: 0x000B8000
	public override bool CanOrderBeExecuted(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		if (!orderExecutor.CanAddItem(base.Item))
		{
			reasonIfNot = "player_talk_not_enough_space";
			return false;
		}
		WgoData targetWgoData = this.TargetWgoData;
		if (targetWgoData == null || targetWgoData.Inventory.Data.GetTotalCountInInventory(base.Item.id, null, false) < base.Item.Count)
		{
			reasonIfNot = "player_talk_not_enough_items";
			return false;
		}
		reasonIfNot = string.Empty;
		return true;
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x000B9E69 File Offset: 0x000B8069
	public override string GetInteractionHint()
	{
		return "hint_take";
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x000B9E70 File Offset: 0x000B8070
	public override string GetStatusIcon()
	{
		return "craft_status_not_enough_items";
	}
}
