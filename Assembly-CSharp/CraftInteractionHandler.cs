using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200063F RID: 1599
public class CraftInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A7E RID: 10878 RVA: 0x000C8914 File Offset: 0x000C6B14
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
		if (this.HasOneTimeCraftStarted())
		{
			return false;
		}
		if (this.IsConveyorAutoCrafter())
		{
			return true;
		}
		Item item;
		if (this.assignedCraftComponent.HasCraftsByBalance && this.TryGetInsertableZombieOverhead(out item))
		{
			DockPoint dockPoint = this.assignedWgo.TryGetDockPointForWorker(true, interactor.transform.position);
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, dockPoint.transform.position, dockPoint.Direction);
			DockPointData dockPointData = this.assignedWgo.GetDockPointData(dockPoint);
			if (this.assignedWgo.Data.CraftableType == CraftableType.ConveyorWorkbench)
			{
				zombieWgoData.AttachToConveyorCraftWgoData(this.assignedWgo.Data.UniqueId, item, dockPointData);
			}
			else
			{
				zombieWgoData.AttachToCraftWgoData(this.assignedWgo.Data.UniqueId, item, dockPointData);
			}
			this.assignedWgo.Data.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
			this.assignedWgo.DrawWidgets();
			if (Vector3.Distance(dockPoint.transform.position, interactor.PlayerData.position.Value) <= 1f)
			{
				interactor.TryTeleportPlayerToAnyFreePlace();
			}
			return true;
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
			if (craftDef.isFuelCraft)
			{
				LazyWindow<UIBaseCraftSelectionWindowData> window = LazyUI.GetWindow<UIFuelCraftWindow>();
				UISingleCraftWindowData uisingleCraftWindowData = new UISingleCraftWindowData(this.assignedWgo.Data, craftDef, null, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(this.FormCraftElementAndStartCraft));
				window.Open(uisingleCraftWindowData, delegate(UIBaseCraftSelectionWindowData _)
				{
					if (wasPlayerSetAsWorker)
					{
						this.assignedWgo.Data.ClearWorker();
					}
				});
			}
			else
			{
				LazyWindow<UIBaseCraftSelectionWindowData> window2 = LazyUI.GetWindow<UISingleCraftWindow>();
				UISingleCraftWindowData uisingleCraftWindowData2 = new UISingleCraftWindowData(this.assignedWgo.Data, craftDef, null, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(this.FormCraftElementAndStartCraft));
				window2.Open(uisingleCraftWindowData2, delegate(UIBaseCraftSelectionWindowData _)
				{
					if (wasPlayerSetAsWorker)
					{
						this.assignedWgo.Data.ClearWorker();
					}
				});
			}
			return true;
		}
		LazyWindow<UIBaseCraftWindowData> window3 = LazyUI.GetWindow<UICraftWindow>();
		UIBaseCraftWindowData uibaseCraftWindowData = new UIBaseCraftWindowData(this.assignedWgo, delegate(CraftElement ce)
		{
			this.OnCraftPressed(ce, false);
		}, delegate(CraftElement ce)
		{
			this.OnCraftPressed(ce, true);
		});
		window3.Open(uibaseCraftWindowData, delegate(UIBaseCraftWindowData _)
		{
			if (wasPlayerSetAsWorker)
			{
				this.assignedWgo.Data.ClearWorker();
			}
		});
		return true;
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x000C8B88 File Offset: 0x000C6D88
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		this.assignedCraftComponent = this.assignedWgo.Data.CraftComponent;
		if (this.assignedCraftComponent.IsDestroyingCraftActive)
		{
			return false;
		}
		if (this.assignedCraftComponent.HasPreFinishUpdate)
		{
			return false;
		}
		if (this.HasOneTimeCraftStarted())
		{
			return false;
		}
		if (this.IsConveyorAutoCrafter())
		{
			return true;
		}
		base.HasInsertableZombieOverhead();
		return true;
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000C8BF0 File Offset: 0x000C6DF0
	public override bool Interact2(PlayerController interactor)
	{
		if (base.Interact2(interactor))
		{
			return true;
		}
		if (this.assignedCraftComponent.IsDestroyingCraftActive)
		{
			return false;
		}
		if (this.HasOneTimeCraftStarted())
		{
			return false;
		}
		if (this.IsConveyorAutoCrafter())
		{
			return false;
		}
		if (this.assignedWgo.Data.Definition.isAutoCrafter && this.assignedCraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
		{
			bool worker = this.assignedWgo.Data.Worker != null;
			bool flag = false;
			if (!worker)
			{
				flag = true;
				this.assignedWgo.Data.TrySetWorker(interactor, null);
			}
			this.assignedCraftComponent.ContinueAutoCraft();
			List<Item> list = new List<Item>();
			list.AddRange(this.assignedWgo.Data.CraftableObjectCraftInventory.Data.RemoveAllItems());
			if (list.Count > 0)
			{
				foreach (Item item in list)
				{
					this.assignedWgo.Data.MakeDrop(item);
				}
			}
			this.assignedWgo.Data.DropStoredTechPoints();
			if (flag)
			{
				this.assignedWgo.Data.ClearWorker();
			}
			interactor.PlayerInteractionComponent.ResetInteractionState();
			return true;
		}
		ZombieWgoData zombieWgoData = this.assignedWgo.Data.Worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			bool flag2 = false;
			string text = string.Empty;
			bool flag3 = zombieWgoData.CrafterCurrentOrder != null;
			for (int i = zombieWgoData.CrafterOrders.Count - 1; i >= 0; i--)
			{
				OrderBase orderBase = zombieWgoData.WorldZoneData.FindOrder(zombieWgoData.CrafterOrders[i]);
				if (orderBase != null)
				{
					string text2;
					if (orderBase.TryExecuteOrder(new PlayerOrderExecutor(), out text2))
					{
						zombieWgoData.WorldZoneData.RemoveOrder(orderBase.UniqueId);
						zombieWgoData.CrafterOnOrderExecuted(orderBase);
					}
					else
					{
						text = text2;
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				Bubble.Talk(new PhraseData(true, null, text, null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			}
			interactor.PlayerInteractionComponent.ResetInteractionState();
			return true;
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x000C8E0C File Offset: 0x000C700C
	public override bool HasInteraction2(PlayerController interactor)
	{
		this.assignedCraftComponent = this.assignedWgo.Data.CraftComponent;
		if (this.assignedCraftComponent.IsDestroyingCraftActive)
		{
			return true;
		}
		if (this.IsConveyorAutoCrafter())
		{
			return false;
		}
		if (this.assignedWgo.Data.Definition.isAutoCrafter && this.assignedCraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
		{
			return true;
		}
		ZombieWgoData zombieWgoData = this.assignedWgo.Data.Worker as ZombieWgoData;
		return (zombieWgoData != null && zombieWgoData.CrafterCurrentOrder != null) || this.HasOneTimeCraftStarted();
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x000C8EA0 File Offset: 0x000C70A0
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
		if (this.HasOneTimeCraftStarted())
		{
			interactionInfos2.Add(base.GetInteractionInfoByUsingTool(true));
			return interactionInfos2;
		}
		if (this.IsConveyorAutoCrafter())
		{
			return interactionInfos2;
		}
		if ((data.CraftComponent.HasCraftsByBalance || data.Definition.isAutoCrafter) && base.HasInsertableZombieOverhead() && !this.assignedCraftComponent.IsDestroyingCraftActive)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_zombie", GameKey.Interaction)));
			return interactionInfos2;
		}
		string text = ((data.id == "alchemy_flask_1_shed") ? "ui_place" : "hint_craft");
		if (data.Definition.isAutoCrafter)
		{
			if (data.CraftComponent.HasCraftsByBalance && data.CraftComponent.AvailableCrafts[0].isAuto)
			{
				interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon(text, GameKey.Interaction)));
				if (this.assignedWgo.Data.Definition.isAutoCrafter && this.assignedCraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
				{
					interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take_all", GameKey.Action)));
				}
			}
			else
			{
				interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon(text, GameKey.Interaction)));
			}
		}
		else
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon(text, GameKey.Interaction)));
		}
		ZombieWgoData zombieWgoData = this.assignedWgo.Data.Worker as ZombieWgoData;
		if (zombieWgoData != null && zombieWgoData.CrafterCurrentOrder != null)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon(zombieWgoData.CrafterCurrentOrder.GetInteractionHint(), GameKey.Action)));
		}
		if ((data.CraftComponent.HasCraftsByBalance || data.Definition.isAutoCrafter) && data.CraftComponent.HasCraftsInQueue && !this.HasInsertedWorker() && !data.Definition.isAutoCrafter)
		{
			interactionInfos2.Add(base.GetInteractionInfoByUsingTool(true));
		}
		return interactionInfos2;
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x000C90C8 File Offset: 0x000C72C8
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
			if (this.assignedWgo.Data.Definition.conveyorType == ConveyorElementType.Workbench && this.assignedCraftComponent.CraftElementsQueue.Count > 0 && this.assignedCraftComponent.CraftElementsQueue.Find((CraftElementBase x) => x.CraftId == craftElement.CraftId) == null)
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

	// Token: 0x06002A84 RID: 10884 RVA: 0x000C925B File Offset: 0x000C745B
	private bool IsConveyorAutoCrafter()
	{
		return this.assignedWgo.Data.Definition.conveyorType == ConveyorElementType.Workbench && this.assignedWgo.Data.Definition.isAutoCrafter;
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x000C928C File Offset: 0x000C748C
	private bool HasDroppableItem()
	{
		return this.assignedWgo.Data.CraftableObjectCraftInventory.Data.Inventory.Count > 0;
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x000C92B0 File Offset: 0x000C74B0
	private bool HasInsertedWorker()
	{
		return this.assignedWgo.Data.Worker != null && this.assignedWgo.Data.Worker != MainGame.PlayerController;
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x000C92E0 File Offset: 0x000C74E0
	private bool HasOneTimeCraftStarted()
	{
		if (this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			CraftElementBase currentCraftElement = this.assignedWgo.Data.CraftComponent.CurrentCraftElement;
			CraftDef craftDef = ((currentCraftElement != null) ? currentCraftElement.Def : null) as CraftDef;
			if (craftDef != null && craftDef.isOneTimeCraft)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x000C933C File Offset: 0x000C753C
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

	// Token: 0x04002333 RID: 9011
	protected CraftComponent assignedCraftComponent;
}
