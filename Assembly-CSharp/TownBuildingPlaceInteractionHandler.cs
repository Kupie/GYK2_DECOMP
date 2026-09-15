using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000663 RID: 1635
public class TownBuildingPlaceInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B3E RID: 11070 RVA: 0x000CCDE8 File Offset: 0x000CAFE8
	public override bool Interact(PlayerController interactor)
	{
		TownBuildingPlaceInteractionHandler.<>c__DisplayClass0_0 CS$<>8__locals1 = new TownBuildingPlaceInteractionHandler.<>c__DisplayClass0_0();
		CS$<>8__locals1.<>4__this = this;
		if (base.Interact(interactor))
		{
			return true;
		}
		CS$<>8__locals1.buildWindow = LazyUI.GetWindow<UITownBuildingWindow>();
		CS$<>8__locals1.availableBuildings = this.assignedWgo.Data.TownBuildingWgoComponent.GetAvailableBuildings(this.assignedWgo.Data);
		if (CS$<>8__locals1.availableBuildings.Count == 1 && CS$<>8__locals1.availableBuildings[0].townBuildingType == TownBuildingType.TownRepair && CS$<>8__locals1.availableBuildings[0].upgradeRequirements.Count > 0)
		{
			TownBuildingDef townBuildingDef = CS$<>8__locals1.availableBuildings[0];
			List<AnswerVisualData> list = new List<AnswerVisualData>();
			list.Add(new AnswerVisualData
			{
				answerData = new AnswerData
				{
					lockRes = this.GetUpgradeRequirementsLockRes(townBuildingDef)
				},
				hiddenByDefault = false,
				id = "hint_build"
			});
			list.Add(new AnswerVisualData
			{
				answerData = new AnswerData(),
				hiddenByDefault = false,
				id = "common_leave"
			});
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFlow, false);
			Bubble.ShowMultiAnswer(list, MainGame.PlayerController.BubblePoint, this.assignedWgo.Data, delegate(string chosen)
			{
				MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFlow, true);
				if (chosen == "hint_build")
				{
					base.<Interact>g__OpenBuildWindow|1();
				}
			}, delegate
			{
			}, false);
			return true;
		}
		CS$<>8__locals1.<Interact>g__OpenBuildWindow|1();
		return true;
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x000CCF4C File Offset: 0x000CB14C
	private SmartRes GetUpgradeRequirementsLockRes(TownBuildingDef def)
	{
		SmartRes smartRes = new SmartRes();
		smartRes.gameRes = new GameRes();
		foreach (ExpressionGameRes expressionGameRes in def.upgradeRequirements)
		{
			smartRes.gameRes.Add(expressionGameRes.name, expressionGameRes.expression.EvaluateFloat());
		}
		return smartRes;
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x000CCFC8 File Offset: 0x000CB1C8
	public override bool HasInteraction(PlayerController interactor)
	{
		return !this.assignedWgo.Data.CraftComponent.IsStarted;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000CCFE4 File Offset: 0x000CB1E4
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_build", GameKey.Interaction)));
	}
}
