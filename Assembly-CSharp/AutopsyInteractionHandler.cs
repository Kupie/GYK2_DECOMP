using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000635 RID: 1589
public class AutopsyInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A4A RID: 10826 RVA: 0x000C77A8 File Offset: 0x000C59A8
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			return false;
		}
		if (this.HasInsertableOverheadBodyItem())
		{
			this.InsertOverheadItem();
			return true;
		}
		this.LinkZombieItem();
		bool wasPlayerSetAsWorker = false;
		if (this.assignedWgo.Data.Worker == null)
		{
			this.assignedWgo.Data.TrySetWorker(interactor, null);
			wasPlayerSetAsWorker = true;
		}
		LazyWindow<UIAutopsyWindowData> window = LazyUI.GetWindow<UIAutopsyWindow>();
		UIAutopsyWindowData uiautopsyWindowData = new UIAutopsyWindowData(this.assignedWgo.Data);
		window.Open(uiautopsyWindowData, delegate(UIAutopsyWindowData _)
		{
			if (wasPlayerSetAsWorker)
			{
				this.assignedWgo.Data.ClearWorker();
			}
		});
		return true;
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x000C7858 File Offset: 0x000C5A58
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		if (this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			return new InteractionInfos(base.GetInteractionInfoByUsingTool(true));
		}
		if (this.HasInsertableOverheadBodyItem())
		{
			return new InteractionInfos(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_place_body", GameKey.Interaction)));
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("action_inspect", GameKey.Interaction)));
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000C78E4 File Offset: 0x000C5AE4
	private bool HasInsertableOverheadBodyItem()
	{
		if (this.HasBodyItemInside())
		{
			return false;
		}
		Item item2;
		return MainGame.Instance.GameSave.playerData.TryGetOverheadItem((Item item) => !item.HasItemsByItemType(ItemType.Demon) && item.Definition.itemGroupIds.Contains("body"), out item2);
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000C7930 File Offset: 0x000C5B30
	private bool HasBodyItemInside()
	{
		using (List<Item>.Enumerator enumerator = this.assignedWgo.Data.Inventory.Data.Inventory.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Definition.itemGroupIds.Contains("body"))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000C79AC File Offset: 0x000C5BAC
	private void InsertOverheadItem()
	{
		Item item2;
		if (!MainGame.Instance.GameSave.playerData.TryGetOverheadItem((Item item) => !item.HasItemsByItemType(ItemType.Demon) && item.Definition.itemGroupIds.Contains("body"), out item2))
		{
			return;
		}
		this.assignedWgo.Data.Inventory.AddItemToInventory(item2, null, false);
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerInsertBodyToAutopsy, this.assignedWgo.Id + ":" + item2.id);
		MainGame.Instance.GameSave.playerData.RemoveOverheadItem(item2);
		this.assignedWgo.DrawWidgets();
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x000C7A4C File Offset: 0x000C5C4C
	private void LinkZombieItem()
	{
		foreach (Item item in this.assignedWgo.Data.Inventory.Data.Inventory)
		{
			if (item.Definition.itemGroupIds.Contains("body") && item.Definition.itemGroupIds.Contains("zombie"))
			{
				MainGame.ZombieSystemData.GetZombie(item.UniqueId).SetZombieItem(item);
				break;
			}
		}
	}
}
