using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C36 RID: 3126
	[Name("Has Town Building LevelUp", 0)]
	public class Flow_HasTownBuildingLevelUp : GKCustomFlowNode
	{
		// Token: 0x06004FC7 RID: 20423 RVA: 0x00177DC8 File Offset: 0x00175FC8
		protected override void RegisterPorts()
		{
			this.hasLevelUp = base.AddValueOutput<bool>("hasLevelUp".CapitalizeFirst(), delegate
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(base.SelfWgoData.LinkedToTownBuildingUniqueId);
				return wgoData.TownBuildingWgoComponent.HasLevelUp && !wgoData.CraftComponent.IsStarted;
			}, "");
			this.upgradeLock = base.AddValueOutput<SmartRes>("upgradeLock".CapitalizeFirst(), delegate
			{
				WgoData wgoData2 = MainGame.WorldData.GetWgoData(base.SelfWgoData.LinkedToTownBuildingUniqueId);
				if (!wgoData2.TownBuildingWgoComponent.HasLevelUp || wgoData2.CraftComponent.IsStarted)
				{
					return null;
				}
				TownBuildingDef data = GameBalance.Me.GetData<TownBuildingDef>(wgoData2.TownBuildingWgoComponent.TownBuildingDef.lvlUpId);
				if (data.upgradeRequirements.Count <= 0)
				{
					return null;
				}
				SmartRes smartRes = new SmartRes();
				smartRes.gameRes = new GameRes();
				foreach (ExpressionGameRes expressionGameRes in data.upgradeRequirements)
				{
					smartRes.gameRes.Add(expressionGameRes.name, expressionGameRes.expression.EvaluateFloat());
				}
				return smartRes;
			}, "");
		}

		// Token: 0x04004139 RID: 16697
		private ValueOutput<bool> hasLevelUp;

		// Token: 0x0400413A RID: 16698
		private ValueOutput<SmartRes> upgradeLock;
	}
}
