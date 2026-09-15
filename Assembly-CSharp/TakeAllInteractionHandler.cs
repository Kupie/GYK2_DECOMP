using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000661 RID: 1633
public class TakeAllInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B36 RID: 11062 RVA: 0x000CCBE8 File Offset: 0x000CADE8
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		WgoData data = this.assignedWgo.Data;
		foreach (Item item in data.Inventory.Data.Inventory)
		{
			List<Item> list = new List<Item>();
			MainGame.Instance.dropSystem.DropItem(item, data.WorldId, data.Position, list);
			foreach (Item item2 in list)
			{
				DropView dropView = null;
				foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
				{
					dropView = gameScene.GetDropView(item2);
					if (dropView != null)
					{
						break;
					}
				}
				if (dropView != null)
				{
					dropView.MoveToCustomPosition(MainGame.PlayerController.transform.position);
				}
			}
		}
		data.Inventory.Clear();
		data.IsInteractable = false;
		return true;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000CCD44 File Offset: 0x000CAF44
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_take_all", GameKey.Interaction)));
	}
}
