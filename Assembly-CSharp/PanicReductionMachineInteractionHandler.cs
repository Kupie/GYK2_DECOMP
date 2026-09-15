using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000651 RID: 1617
public class PanicReductionMachineInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AED RID: 10989 RVA: 0x000CB688 File Offset: 0x000C9888
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (this.assignedCraftComponent.IsDestroyingCraftActive)
		{
			return false;
		}
		if (MainGame.Instance.GameSave.environmentData.CurrentDayNumber != ConstDef.Get("day_gluttony").IntValue)
		{
			Bubble.Talk(new PhraseData(true, null, "pr_machine_available_tomorrow", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return false;
		}
		bool wasPlayerSetAsWorker = false;
		if (this.assignedWgo.Data.Worker == null)
		{
			this.assignedWgo.Data.TrySetWorker(interactor, null);
			wasPlayerSetAsWorker = true;
		}
		if (this.assignedCraftComponent.CraftsIn.Count == 1)
		{
			CraftDef craftDef = (CraftDef)this.assignedCraftComponent.CraftsIn[0];
			LazyWindow<UIBaseCraftSelectionWindowData> window = LazyUI.GetWindow<UISingleCraftWindow>();
			UISingleCraftWindowData uisingleCraftWindowData = new UISingleCraftWindowData(this.assignedWgo.Data, craftDef, null, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(this.FormCraftElementAndStartCraft));
			window.Open(uisingleCraftWindowData, delegate(UIBaseCraftSelectionWindowData _)
			{
				if (wasPlayerSetAsWorker)
				{
					this.assignedWgo.Data.ClearWorker();
				}
			});
			return true;
		}
		LazyWindow<UIBaseCraftWindowData> window2 = LazyUI.GetWindow<UICraftWindow>();
		UIBaseCraftWindowData uibaseCraftWindowData = new UIBaseCraftWindowData(this.assignedWgo, delegate(CraftElement ce)
		{
			this.OnCraftPressed(ce, false);
		}, delegate(CraftElement ce)
		{
			this.OnCraftPressed(ce, true);
		});
		window2.Open(uibaseCraftWindowData, delegate(UIBaseCraftWindowData _)
		{
			if (wasPlayerSetAsWorker)
			{
				this.assignedWgo.Data.ClearWorker();
			}
		});
		return true;
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x000CB7D0 File Offset: 0x000C99D0
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		this.assignedCraftComponent = this.assignedWgo.Data.CraftComponent;
		return !this.assignedCraftComponent.IsDestroyingCraftActive && !this.assignedCraftComponent.HasPreFinishUpdate && !this.assignedCraftComponent.IsStarted;
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x000CB82C File Offset: 0x000C9A2C
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		WgoData data = this.assignedWgo.Data;
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (this.assignedCraftComponent.IsDestroyingCraftActive)
		{
			interactionInfos2.Add(base.GetInteractionInfoByUsingTool(true));
			return interactionInfos2;
		}
		if (data.CraftComponent.HasCraftsByBalance)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_craft", GameKey.Interaction)));
		}
		return interactionInfos2;
	}

	// Token: 0x06002AF0 RID: 10992 RVA: 0x000CB8A4 File Offset: 0x000C9AA4
	private void OnCraftPressed(CraftElement craftElement, bool addToQueueTop = false)
	{
		if (craftElement.Definition.skipQueue)
		{
			if (this.assignedCraftComponent.GetStartCraftStatus(craftElement, null) != CraftStatus.OK)
			{
				return;
			}
			if (craftElement.CanFinishCraft(this.assignedWgo.Data) != CraftStatus.OK)
			{
				return;
			}
			this.assignedCraftComponent.ProcessInstantCraft(this.assignedWgo.Data, craftElement);
			return;
		}
		else
		{
			if (craftElement.Definition.IsMultipleCraftsDisabled && this.assignedCraftComponent.CraftElementsQueue.Find((CraftElementBase x) => x.CraftId == craftElement.CraftId) != null)
			{
				return;
			}
			Debug.Log(string.Format("Adding Craft: id: {0}, count: {1}, addToQueue: {2}", craftElement.CraftId, craftElement.Count, addToQueueTop));
			if (this.assignedWgo.Data.Definition.isAutoCrafter && this.assignedCraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
			{
				this.assignedCraftComponent.AddToQueue(craftElement, false, 1);
				LazyUI.GetWindow<UICraftWindow>().Close();
				return;
			}
			this.assignedCraftComponent.AddToQueue(craftElement, addToQueueTop, -1);
			if (addToQueueTop)
			{
				this.assignedCraftComponent.TryContinueFromQueue();
				LazyUI.GetWindow<UICraftWindow>().Close();
			}
			return;
		}
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x000CB9F0 File Offset: 0x000C9BF0
	private void FormCraftElementAndStartCraft(CraftDef craftDefinition, List<NeedItemData> selectedNeedItems, CraftParamsData craftParams, int craftsCount = 1)
	{
		CraftElement craftElement = new CraftElement(craftDefinition.id, craftsCount, selectedNeedItems, craftParams);
		craftElement.DoBeforeStartCalculations(this.assignedWgo.Data);
		if (craftElement.Definition.isFuelCraft)
		{
			ItemDef data = GameBalance.Me.GetData<ItemDef>(craftElement.Definition.addItemsToWgoOnFinish.chanceOutputItems[0].id);
			int num = Mathf.FloorToInt((float)this.assignedWgo.Data.Inventory.Data.CanAddItemCountToInventory(data, 99999, true, null, false) / (float)craftElement.PreToWgoOnFinishItems[0].count);
			craftElement.Count = ((craftsCount > num) ? num : craftsCount);
		}
		this.OnCraftPressed(craftElement, true);
	}

	// Token: 0x04002346 RID: 9030
	protected CraftComponent assignedCraftComponent;
}
