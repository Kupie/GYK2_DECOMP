using System;
using LazyBearTechnology;

// Token: 0x0200064C RID: 1612
public class FlagStandInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06002AC6 RID: 10950 RVA: 0x000CA290 File Offset: 0x000C8490
	private SGuid AssociatedFlagSGuid
	{
		get
		{
			if (string.IsNullOrEmpty(this.assignedWgo.Data.GameResStr.Get("flag_stand_sguid", "")))
			{
				return SGuid.Empty;
			}
			return SGuid.Parse(this.assignedWgo.Data.GameResStr.Get("flag_stand_sguid", ""));
		}
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x000CA2F0 File Offset: 0x000C84F0
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		Wgo attachedWgo = interactor.attachedWgo;
		SGuid sguid = ((attachedWgo != null) ? attachedWgo.Data.UniqueId : null);
		return (sguid == null && !SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid)) || (sguid != null && SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid)) || (sguid != null && !SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid));
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x000CA36C File Offset: 0x000C856C
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Wgo attachedWgo = interactor.attachedWgo;
		SGuid sguid = ((attachedWgo != null) ? attachedWgo.Data.UniqueId : null);
		FlagStandComponent componentInChildren = this.assignedWgo.GetComponentInChildren<FlagStandComponent>();
		if (sguid == null && !SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.AssociatedFlagSGuid);
			if (wgoViewGlobal)
			{
				if (componentInChildren != null)
				{
					componentInChildren.DetachFlag();
				}
				interactor.AttachTheFlag(wgoViewGlobal);
				this.assignedWgo.Data.GameResStr.Set("flag_stand_sguid", string.Empty);
				AgentsGroupFlagController.SetInteractionLocked(wgoViewGlobal, true);
				if (this.assignedWgo.Id.EndsWith("one_time"))
				{
					if (componentInChildren != null && LazySingleton<FightingGameController>.Instance.CurrentFightState != FightState.Disabled)
					{
						LazySingleton<FightingGameController>.Instance.FlagStandComponents.Remove(componentInChildren);
					}
					this.assignedWgo.RemoveWithData();
				}
			}
			this.assignedWgo.SetCustomBubblePoint(null);
			this.assignedWgo.DrawWidgets();
			return true;
		}
		if (sguid != null && SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			Wgo wgoViewGlobal2 = GameScene.GetWgoViewGlobal(sguid);
			if (wgoViewGlobal2)
			{
				interactor.RemoveTheFlag();
				if (componentInChildren)
				{
					componentInChildren.AttachFlag(wgoViewGlobal2, true);
					componentInChildren.TryToSyncFlagPosition();
				}
				else
				{
					wgoViewGlobal2.Data.Position = this.assignedWgo.transform.position;
				}
				this.assignedWgo.Data.GameResStr.Set("flag_stand_sguid", wgoViewGlobal2.Data.UniqueId.ToString());
				AgentsGroupFlagController.SetInteractionLocked(wgoViewGlobal2, false);
				this.assignedWgo.SetCustomBubblePoint(wgoViewGlobal2.MainWgoPart.BubblePoint);
			}
			this.assignedWgo.DrawWidgets();
			return true;
		}
		if (sguid != null && !SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			Wgo wgoViewGlobal3 = GameScene.GetWgoViewGlobal(this.AssociatedFlagSGuid);
			Wgo wgoViewGlobal4 = GameScene.GetWgoViewGlobal(sguid);
			if (wgoViewGlobal3 && wgoViewGlobal4)
			{
				if (componentInChildren != null)
				{
					componentInChildren.DetachFlag();
				}
				interactor.RemoveTheFlag();
				if (componentInChildren)
				{
					componentInChildren.AttachFlag(wgoViewGlobal4, true);
					componentInChildren.TryToSyncFlagPosition();
				}
				else
				{
					wgoViewGlobal4.Data.Position = this.assignedWgo.transform.position;
				}
				this.assignedWgo.Data.GameResStr.Set("flag_stand_sguid", wgoViewGlobal4.Data.UniqueId.ToString());
				AgentsGroupFlagController.SetInteractionLocked(wgoViewGlobal4, false);
				interactor.AttachTheFlag(wgoViewGlobal3);
				AgentsGroupFlagController.SetInteractionLocked(wgoViewGlobal3, true);
				this.assignedWgo.SetCustomBubblePoint(wgoViewGlobal4.MainWgoPart.BubblePoint);
			}
			this.assignedWgo.DrawWidgets();
			return true;
		}
		return false;
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x000CA624 File Offset: 0x000C8824
	protected override InteractionInfos FormInteractionInfo()
	{
		this.ApplyCustomBubblePoint();
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		PlayerController interactor = this.interactor;
		SGuid sguid;
		if (interactor == null)
		{
			sguid = null;
		}
		else
		{
			Wgo attachedWgo = interactor.attachedWgo;
			if (attachedWgo == null)
			{
				sguid = null;
			}
			else
			{
				WgoData data = attachedWgo.Data;
				sguid = ((data != null) ? data.UniqueId : null);
			}
		}
		SGuid sguid2 = sguid;
		if (sguid2 == null && !SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			InteractionInfos interactionInfos2 = new InteractionInfos();
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take_flag", GameKey.Interaction)));
			interactionInfos2.Add(new InteractionInfo("", "icon-arrow_flag-take"));
			return interactionInfos2;
		}
		if (sguid2 != null && SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			InteractionInfos interactionInfos3 = new InteractionInfos();
			interactionInfos3.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_flag", GameKey.Interaction)));
			interactionInfos3.Add(new InteractionInfo("", "icon-arrow_flag-insert"));
			return interactionInfos3;
		}
		if (sguid2 != null && !SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			InteractionInfos interactionInfos4 = new InteractionInfos();
			interactionInfos4.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_flag", GameKey.Interaction)));
			interactionInfos4.Add(new InteractionInfo("", "icon-arrow_flag-insert"));
			return interactionInfos4;
		}
		return new InteractionInfos();
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x000CA75C File Offset: 0x000C895C
	private void ApplyCustomBubblePoint()
	{
		if (!SGuid.IsNullOrEmpty(this.AssociatedFlagSGuid))
		{
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.AssociatedFlagSGuid);
			if (wgoViewGlobal)
			{
				this.assignedWgo.SetCustomBubblePoint(wgoViewGlobal.MainWgoPart.BubblePoint);
				return;
			}
		}
		else
		{
			this.assignedWgo.SetCustomBubblePoint(null);
		}
	}

	// Token: 0x04002341 RID: 9025
	public const string GAME_RES_STR_KEY = "flag_stand_sguid";
}
