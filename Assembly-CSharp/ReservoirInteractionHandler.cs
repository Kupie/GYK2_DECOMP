using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000657 RID: 1623
public class ReservoirInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B0A RID: 11018 RVA: 0x000CC0CC File Offset: 0x000CA2CC
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (MainGame.PlayerData.HasMultipleOverheadItems)
		{
			return false;
		}
		if (!interactor.PlayerData.toolBeltInventory.Data.HasItemsByItemType(ItemType.FishingRod))
		{
			Bubble.Talk(new PhraseData(true, null, "fishing_no_rod", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return true;
		}
		if (interactor.GetMasteryLevelForTalentBranch("talent_green", null) <= 0)
		{
			Bubble.Talk(new PhraseData(true, null, "fishing_no_mastery", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return true;
		}
		List<FishingDef> allForReservoir = FishingDef.GetAllForReservoir(this.assignedWgo.Id);
		allForReservoir.RemoveAll((FishingDef x) => this.assignedWgo.Data.GetGameResInt(x.fishId) == 0);
		if (allForReservoir.Count == 0)
		{
			Bubble.Talk(new PhraseData(true, null, "fishing_no_fish", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return true;
		}
		if (FishingDef.GetAvailableBaits(new List<Item>(MainGame.PlayerData.Inventory.GetItemsByType(ItemType.Bait))
		{
			new Item("no_bait", 1)
		}, allForReservoir).Count == 0)
		{
			Bubble.Talk(new PhraseData(true, null, "fishing_no_bait", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return true;
		}
		if (!this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			this.StartFishing(interactor.PlayerData.toolBeltInventory.Data.GetItemByType(ItemType.FishingRod).Definition);
		}
		return true;
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x000CC228 File Offset: 0x000CA428
	private void StartFishing(ItemDef fishingRodDef)
	{
		DockPoint dockPoint = this.assignedWgo.TryGetDockPointForWorker(false, default(Vector3));
		if (dockPoint == null)
		{
			Debug.LogError("[ReservoirInteractionHandler]: No dock point for fishing");
			return;
		}
		if (MainGame.PlayerData.HasOverheadItem)
		{
			MainGame.PlayerData.DropOverheadItem();
		}
		string currentGameSceneId = MainGame.PlayerData.currentGameSceneId;
		MainGame.PlayerController.MovementComponent.StartPath(dockPoint.transform.position, currentGameSceneId, currentGameSceneId, MovementType.Direct, 1.5f, "", delegate
		{
			UIFishingWindow fishingWindow = LazyUI.GetWindow<UIFishingWindow>();
			UIFishingWindowData windowData = new UIFishingWindowData(this.assignedWgo, fishingRodDef);
			if (!MainGame.PlayerData.interactedWithFishingReservoirOnce && MainGame.PlayerData.GetRes("fishing_tutorial_available", 0f) > 0f)
			{
				MainGame.PlayerData.interactedWithFishingReservoirOnce = true;
				UITutorialWindowData uitutorialWindowData = new UITutorialWindowData("tut_fishing_hdr", null, false);
				LazyUI.GetWindow<UITutorialWindow>().Open(uitutorialWindowData, delegate(UITutorialWindowData _)
				{
					fishingWindow.Open(windowData);
				});
				return;
			}
			fishingWindow.Open(windowData);
		}, MainGame.PlayerController.PlayerLocalAreaMovement.Seeker, MovementComponent.DestinationType.Position);
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000CC2D8 File Offset: 0x000CA4D8
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || !MainGame.PlayerData.HasMultipleOverheadItems;
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x000CC2F4 File Offset: 0x000CA4F4
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfo interactionInfoByUsingTool = base.GetInteractionInfoByUsingTool(false);
		if (!interactionInfoByUsingTool.isItemEquipped)
		{
			return new InteractionInfos(interactionInfoByUsingTool);
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("ui_submit_bait", GameKey.Interaction)));
	}
}
