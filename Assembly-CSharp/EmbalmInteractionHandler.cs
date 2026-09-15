using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000645 RID: 1605
public class EmbalmInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AA5 RID: 10917 RVA: 0x000C9B0C File Offset: 0x000C7D0C
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
		LazyWindow<UIEmbalmWindowData> window = LazyUI.GetWindow<UIEmbalmWindow>();
		UIEmbalmWindowData uiembalmWindowData = new UIEmbalmWindowData(this.assignedWgo.Data);
		window.Open(uiembalmWindowData, delegate(UIEmbalmWindowData _)
		{
			if (wasPlayerSetAsWorker)
			{
				this.assignedWgo.Data.ClearWorker();
			}
		});
		return true;
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000C9BBC File Offset: 0x000C7DBC
	public override bool HasInteraction(PlayerController interactor)
	{
		if (this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			return false;
		}
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000C9BE0 File Offset: 0x000C7DE0
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

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000C9C6C File Offset: 0x000C7E6C
	private bool HasInsertableOverheadBodyItem()
	{
		if (this.HasBodyItemInside())
		{
			return false;
		}
		Item item2;
		return MainGame.Instance.GameSave.playerData.TryGetOverheadItem((Item item) => !item.HasItemsByItemType(ItemType.Demon) && item.Definition.itemGroupIds.Contains("body"), out item2);
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000C9CB8 File Offset: 0x000C7EB8
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

	// Token: 0x06002AAA RID: 10922 RVA: 0x000C9D34 File Offset: 0x000C7F34
	private void InsertOverheadItem()
	{
		Item item2;
		if (!MainGame.Instance.GameSave.playerData.TryGetOverheadItem((Item item) => !item.HasItemsByItemType(ItemType.Demon) && item.Definition.itemGroupIds.Contains("body"), out item2))
		{
			return;
		}
		this.assignedWgo.Data.Inventory.AddItemToInventory(item2, null, false);
		MainGame.Instance.GameSave.playerData.RemoveOverheadItem(item2);
		this.assignedWgo.DrawWidgets();
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000C9DB4 File Offset: 0x000C7FB4
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
