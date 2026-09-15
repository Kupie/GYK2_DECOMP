using System;
using LazyBearTechnology;

// Token: 0x02000638 RID: 1592
public class BarricadeInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A58 RID: 10840 RVA: 0x000C7B48 File Offset: 0x000C5D48
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		FlagPlacementPoint componentInChildren = this.assignedWgo.GetComponentInChildren<FlagPlacementPoint>();
		if (componentInChildren)
		{
			if (!componentInChildren.FlagWgo && interactor.attachedWgo)
			{
				componentInChildren.FlagWgo = interactor.RemoveTheFlag();
				AgentsGroupFlagController componentInChildren2 = componentInChildren.FlagWgo.GetComponentInChildren<AgentsGroupFlagController>();
				componentInChildren2.AttachedSGuid = this.assignedWgo.Data.UniqueId;
				componentInChildren2.IsSetAtPoint = true;
				componentInChildren2.AgentsController.TryRetargetAgents();
				componentInChildren.FlagWgo.Data.Position = componentInChildren.transform.position;
				AgentsGroupFlagController.SetInteractionLocked(componentInChildren.FlagWgo, true);
				this.assignedWgo.DrawWidgets();
				return true;
			}
			if (componentInChildren.FlagWgo && !interactor.attachedWgo)
			{
				Wgo flagWgo = componentInChildren.FlagWgo;
				componentInChildren.FlagWgo = null;
				interactor.AttachTheFlag(flagWgo);
				AgentsGroupFlagController componentInChildren3 = interactor.attachedWgo.GetComponentInChildren<AgentsGroupFlagController>();
				componentInChildren3.AttachedSGuid = null;
				componentInChildren3.IsSetAtPoint = false;
				foreach (FightingAgent fightingAgent in componentInChildren3.AgentsController.Agents)
				{
					if (fightingAgent.IsExecutingCommand)
					{
						fightingAgent.StopCommandExecution(true);
					}
				}
				componentInChildren3.AgentsController.TryRetargetAgents();
				foreach (FightingAgent fightingAgent2 in componentInChildren3.AgentsController.Agents)
				{
					WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(fightingAgent2.Wgo.Data.takenDockPointsParentSGuid);
					if (wgoData != null)
					{
						DockPointData occupiedDockPointBy = wgoData.MainWgoPartData.GetOccupiedDockPointBy(fightingAgent2.Wgo.Data.UniqueId);
						if (occupiedDockPointBy != null)
						{
							occupiedDockPointBy.UnOccupy();
						}
						fightingAgent2.Wgo.Data.takenDockPointsParentSGuid = SGuid.Empty;
					}
				}
				AgentsGroupFlagController.SetInteractionLocked(flagWgo, false);
				this.assignedWgo.DrawWidgets();
				return true;
			}
			if (componentInChildren.FlagWgo && interactor.attachedWgo)
			{
				Wgo flagWgo2 = componentInChildren.FlagWgo;
				Wgo wgo = interactor.RemoveTheFlag();
				componentInChildren.FlagWgo = wgo;
				AgentsGroupFlagController componentInChildren4 = wgo.GetComponentInChildren<AgentsGroupFlagController>();
				componentInChildren4.AttachedSGuid = this.assignedWgo.Data.UniqueId;
				componentInChildren4.IsSetAtPoint = true;
				componentInChildren4.AgentsController.TryRetargetAgents();
				wgo.Data.Position = componentInChildren.transform.position;
				AgentsGroupFlagController.SetInteractionLocked(wgo, true);
				interactor.AttachTheFlag(flagWgo2);
				AgentsGroupFlagController componentInChildren5 = interactor.attachedWgo.GetComponentInChildren<AgentsGroupFlagController>();
				componentInChildren5.AttachedSGuid = null;
				componentInChildren5.IsSetAtPoint = false;
				foreach (FightingAgent fightingAgent3 in componentInChildren5.AgentsController.Agents)
				{
					if (fightingAgent3.IsExecutingCommand)
					{
						fightingAgent3.StopCommandExecution(true);
					}
				}
				componentInChildren5.AgentsController.TryRetargetAgents();
				foreach (FightingAgent fightingAgent4 in componentInChildren5.AgentsController.Agents)
				{
					WgoData wgoData2 = MainGame.Instance.GameSave.WorldData.GetWgoData(fightingAgent4.Wgo.Data.takenDockPointsParentSGuid);
					if (wgoData2 != null)
					{
						DockPointData occupiedDockPointBy2 = wgoData2.MainWgoPartData.GetOccupiedDockPointBy(fightingAgent4.Wgo.Data.UniqueId);
						if (occupiedDockPointBy2 != null)
						{
							occupiedDockPointBy2.UnOccupy();
						}
						fightingAgent4.Wgo.Data.takenDockPointsParentSGuid = SGuid.Empty;
					}
				}
				AgentsGroupFlagController.SetInteractionLocked(flagWgo2, false);
				this.assignedWgo.DrawWidgets();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000C7F28 File Offset: 0x000C6128
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		FlagPlacementPoint componentInChildren = this.assignedWgo.GetComponentInChildren<FlagPlacementPoint>();
		if (componentInChildren)
		{
			bool flag = interactor.attachedWgo;
			if (componentInChildren.FlagWgo)
			{
				return true;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000C7F78 File Offset: 0x000C6178
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		FlagPlacementPoint componentInChildren = this.assignedWgo.GetComponentInChildren<FlagPlacementPoint>();
		bool flag = this.interactor.attachedWgo;
		if (componentInChildren)
		{
			if (!componentInChildren.FlagWgo && flag)
			{
				interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_flag", GameKey.Interaction)));
				interactionInfos2.Add(new InteractionInfo("", "icon-arrow_flag-insert"));
			}
			else if (componentInChildren.FlagWgo && !flag)
			{
				interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take_flag", GameKey.Interaction)));
				interactionInfos2.Add(new InteractionInfo("", "icon-arrow_flag-take"));
			}
			else if (componentInChildren.FlagWgo && flag)
			{
				interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_flag", GameKey.Interaction)));
				interactionInfos2.Add(new InteractionInfo("", "icon-arrow_flag-insert"));
			}
		}
		return interactionInfos2;
	}
}
