using System;
using LazyBearTechnology;

// Token: 0x02000675 RID: 1653
public abstract class WGOInteractionHandlerBase : IWGOInteractionHandler, IInteractionHandler
{
	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000CEAF1 File Offset: 0x000CCCF1
	protected string WorkHint
	{
		get
		{
			return this.LocalizeHintWithActionIcon("hint_work", GameKey.Action);
		}
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000CEB03 File Offset: 0x000CCD03
	void IInteractionHandler.OnInteractionTargetEnter()
	{
		this.OnInteractionTargetEnter(null);
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000CEB0C File Offset: 0x000CCD0C
	bool IInteractionHandler.Interact()
	{
		return this.Interact(null);
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000CEB15 File Offset: 0x000CCD15
	bool IInteractionHandler.Interact2()
	{
		return this.Interact2(null);
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000CEB1E File Offset: 0x000CCD1E
	bool IInteractionHandler.HasInteraction()
	{
		return this.HasInteraction(this.interactor);
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000CEB2C File Offset: 0x000CCD2C
	bool IInteractionHandler.HasInteraction2()
	{
		return this.HasInteraction2(this.interactor);
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000CEB3A File Offset: 0x000CCD3A
	public virtual IWGOInteractionHandler Init(Wgo wgo)
	{
		this.assignedWgo = wgo;
		return this;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000CEB44 File Offset: 0x000CCD44
	public virtual void OnInteractionTargetEnter(PlayerController interactor)
	{
		this.interactor = interactor;
		this.assignedWgo.DrawWidgets();
		this.assignedWgo.TryDrawNpcWidget();
		UIObjectBubbleManager.Instance.PutOnTopTargetId = this.assignedWgo.Data.UniqueId;
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000CEB7D File Offset: 0x000CCD7D
	public virtual void OnInteractionTargetExit()
	{
		this.interactor = null;
		WgoBubbleDisplayHandler.Hide(this.assignedWgo);
		GUIElements.Instance.NpcWidget.Hide();
		this.assignedWgo.DrawWidgets();
		UIObjectBubbleManager.Instance.PutOnTopTargetId = SGuid.Empty;
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x000CEBBA File Offset: 0x000CCDBA
	public virtual bool Interact(PlayerController interactor)
	{
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.Interaction, this.assignedWgo.Id ?? "");
		return false;
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x000CEBBA File Offset: 0x000CCDBA
	public virtual bool Interact2(PlayerController interactor)
	{
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.Interaction, this.assignedWgo.Id ?? "");
		return false;
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000CEBD8 File Offset: 0x000CCDD8
	public virtual ItemType GetRequiredInteractionToolType()
	{
		if (this.assignedWgo.Data.CraftComponent.CurrentCraftElement != null)
		{
			CraftDef craftDef = this.assignedWgo.Data.CraftComponent.CurrentCraftElement.Def as CraftDef;
			if (craftDef != null)
			{
				if (this.assignedWgo.Data.CraftComponent.HasPreFinishUpdate)
				{
					foreach (CraftElementBase craftElementBase in this.assignedWgo.Data.CraftComponent.CraftElementsQueue)
					{
						if (this.assignedWgo.Data.CraftComponent.GetStartCraftStatus(craftElementBase, null) == CraftStatus.OK)
						{
							craftDef = (CraftDef)craftElementBase.Def;
							break;
						}
					}
				}
				if (craftDef.customItemTypeAction != ItemType.None && !craftDef.isAuto)
				{
					return craftDef.customItemTypeAction;
				}
			}
		}
		return this.assignedWgo.Data.Definition.toolAction.actionableTool;
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000CECE4 File Offset: 0x000CCEE4
	public InteractionInfos GetInteractionInfos()
	{
		InteractionInfos interactionInfos = this.FormInteractionInfo();
		foreach (InteractionInfo interactionInfo in interactionInfos.list)
		{
			interactionInfo.text = (string.IsNullOrEmpty(interactionInfo.text) ? interactionInfo.text : this.FixSpace(interactionInfo.text));
		}
		return interactionInfos;
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool HasInteraction(PlayerController interactor)
	{
		return false;
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool HasInteraction2(PlayerController interactor)
	{
		return false;
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000CED60 File Offset: 0x000CCF60
	protected virtual InteractionInfos FormInteractionInfo()
	{
		CraftElement craftElement = this.assignedWgo.Data.CraftComponent.CurrentCraftElement as CraftElement;
		if (craftElement != null && craftElement.Definition.isObjDestroyCraft)
		{
			return new InteractionInfos(this.GetInteractionInfoByUsingTool(false));
		}
		bool isControlsEnabledForInteractionHints = MainGame.PlayerController.IsControlsEnabledForInteractionHints;
		if (this.assignedWgo.Data.Events.Count > 0 && isControlsEnabledForInteractionHints)
		{
			return new InteractionInfos(this.GetInteractionInfoByEvent(this.assignedWgo.Data.PeekFirstAddedEvent()));
		}
		if (MainGame.Instance.GameSave.questSystemData.WgoHasReadyToFinishQuest(this.assignedWgo.Id) && isControlsEnabledForInteractionHints)
		{
			return new InteractionInfos(this.GetInteractionInfoByQuestStatus());
		}
		return new InteractionInfos();
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000CEE1C File Offset: 0x000CD01C
	protected bool TryGetCustomInteractionStr(out string hint)
	{
		hint = string.Empty;
		bool flag = !string.IsNullOrEmpty(this.assignedWgo.Data.Definition.customInteraction.hint);
		if (flag && this.assignedWgo.Data.Definition.customInteraction.IsInteractable(this.assignedWgo.Data))
		{
			hint = this.LocalizeHintWithActionIcon(this.assignedWgo.Data.Definition.customInteraction.hint, GameKey.Interaction);
			return true;
		}
		bool flag2 = !string.IsNullOrEmpty(this.assignedWgo.Data.Definition.customInteraction2.hint);
		if (flag2 && this.assignedWgo.Data.Definition.customInteraction2.IsInteractable(this.assignedWgo.Data))
		{
			hint = this.LocalizeHintWithActionIcon(this.assignedWgo.Data.Definition.customInteraction2.hint, GameKey.Interaction);
			return true;
		}
		if (flag)
		{
			hint = this.LocalizeHintWithActionIcon(this.assignedWgo.Data.Definition.customInteraction.hint, GameKey.Interaction);
			return true;
		}
		if (flag2)
		{
			hint = this.LocalizeHintWithActionIcon(this.assignedWgo.Data.Definition.customInteraction2.hint, GameKey.Interaction);
			return true;
		}
		return false;
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000C73B0 File Offset: 0x000C55B0
	protected string LocalizeHintWithActionIcon(string hintId, GameKey gameKey)
	{
		return ControllerIconLibrary.GetIconId(gameKey, null, true) + LLBase.L(hintId);
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x000CEF78 File Offset: 0x000CD178
	protected InteractionInfo GetInteractionInfoByUsingTool(bool isForCurrentCraft = false)
	{
		if (MainGame.PlayerData != null && MainGame.PlayerData.HasMultipleOverheadItems)
		{
			return new InteractionInfo();
		}
		if (this.interactor == null)
		{
			return new InteractionInfo(this.WorkHint);
		}
		TalentDef dataOrNull = GameBalance.Me.GetDataOrNull<TalentDef>(this.assignedWgo.Data.Definition.talent);
		ItemType itemType = this.GetRequiredInteractionToolType();
		if (itemType == ItemType.None || itemType == ItemType.Hand)
		{
			if (dataOrNull == null)
			{
				return new InteractionInfo(this.WorkHint);
			}
			itemType = ItemType.Hand;
		}
		bool flag = !this.interactor.PlayerData.toolBeltInventory.GetItemByType(itemType).IsEmpty;
		int num = this.assignedWgo.Data.Definition.MasteryLock;
		bool flag2;
		if (isForCurrentCraft)
		{
			CraftDefBase def = this.assignedWgo.Data.CraftComponent.CurrentCraftElement.Def;
			num = ((def.isStarCraft || def.isAutopsyCraft) ? 1 : def.talentLock);
			flag2 = dataOrNull == null || this.assignedWgo.Data.GetGameResInt("seed_mastery_lock") > 0 || MainGame.PlayerController.GetMasteryLevelForTalentBranch(dataOrNull.id, def) >= num;
			return new InteractionInfo(ControllerIconLibrary.GetIconId(GameKey.Action, null, true), itemType, flag, dataOrNull, num, flag2);
		}
		flag2 = dataOrNull == null || this.assignedWgo.Data.GetGameResInt("seed_mastery_lock") > 0 || MainGame.PlayerController.GetMasteryLevelForTalentBranch(dataOrNull.id, null) >= num;
		return new InteractionInfo(ControllerIconLibrary.GetIconId(GameKey.Action, null, true), itemType, flag, dataOrNull, num, flag2);
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000CF110 File Offset: 0x000CD310
	protected InteractionInfo GetInteractionInfoByEvent(InteractionEvent interactionEvent)
	{
		if (!(this.interactor != null))
		{
			return new InteractionInfo
			{
				customIconId = interactionEvent.CustomIcon
			};
		}
		GameKey gameKey = ((this.assignedWgo.Data.Definition.interactionType == WGODef.InteractionType.Work) ? GameKey.Action : GameKey.Interaction);
		if (this.assignedWgo.Data.Definition.interactionType != WGODef.InteractionType.Work)
		{
			return new InteractionInfo(ControllerIconLibrary.GetIconId(gameKey, null, true), interactionEvent.CustomIcon);
		}
		TalentDef dataOrNull = GameBalance.Me.GetDataOrNull<TalentDef>(this.assignedWgo.Data.Definition.talent);
		ItemType requiredInteractionToolType = this.GetRequiredInteractionToolType();
		if (requiredInteractionToolType == ItemType.None || requiredInteractionToolType == ItemType.Hand)
		{
			return new InteractionInfo(ControllerIconLibrary.GetIconId(gameKey, null, true), interactionEvent.CustomIcon);
		}
		bool flag = !this.interactor.PlayerData.toolBeltInventory.GetItemByType(requiredInteractionToolType).IsEmpty;
		int masteryLock = this.assignedWgo.Data.Definition.MasteryLock;
		bool flag2 = dataOrNull == null || this.assignedWgo.Data.GetGameResInt("seed_mastery_lock") > 0 || MainGame.PlayerController.GetMasteryLevelForTalentBranch(dataOrNull.id, null) >= masteryLock;
		return new InteractionInfo(ControllerIconLibrary.GetIconId(gameKey, null, true), interactionEvent.CustomIcon, requiredInteractionToolType, flag, dataOrNull, masteryLock, flag2);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000CF25F File Offset: 0x000CD45F
	protected InteractionInfo GetInteractionInfoByQuestStatus()
	{
		if (this.interactor != null)
		{
			return new InteractionInfo(ControllerIconLibrary.GetIconId(GameKey.Interaction, null, true), "icon_speech_bubble");
		}
		return new InteractionInfo
		{
			customIconId = "icon_speech_bubble"
		};
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x000CF298 File Offset: 0x000CD498
	protected bool HasInsertableZombieOverhead()
	{
		Item item;
		return this.TryGetInsertableZombieOverhead(out item);
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000CF2B0 File Offset: 0x000CD4B0
	protected virtual bool TryGetInsertableZombieOverhead(out Item zombieItem)
	{
		zombieItem = null;
		if (!(this.interactor == null))
		{
			Wgo wgo = this.assignedWgo;
			bool flag;
			if (wgo == null)
			{
				flag = null != null;
			}
			else
			{
				WgoData data = wgo.Data;
				flag = ((data != null) ? data.Definition : null) != null;
			}
			if (flag && this.assignedWgo.Data.Definition.canInsertZombie && this.assignedWgo.Data.Worker == null && this.assignedWgo.DockPoints != null && this.assignedWgo.DockPoints.Count != 0)
			{
				return this.interactor.PlayerData.TryGetOverheadItem((Item item) => item.Definition.itemGroupIds.Contains("zombie"), out zombieItem);
			}
		}
		return false;
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x000CF368 File Offset: 0x000CD568
	private string FixSpace(string s)
	{
		s = s.Replace(" ", LL.GetSpace());
		s = s.Replace("sprite" + LL.GetSpace(), "sprite ");
		return s;
	}

	// Token: 0x04002365 RID: 9061
	protected Wgo assignedWgo;

	// Token: 0x04002366 RID: 9062
	protected PlayerController interactor;
}
