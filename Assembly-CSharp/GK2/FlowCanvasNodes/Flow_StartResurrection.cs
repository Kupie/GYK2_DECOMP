using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C19 RID: 3097
	[Name("Start Resurrection", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_StartResurrection : GKCustomFlowNode
	{
		// Token: 0x06004F66 RID: 20326 RVA: 0x001763FB File Offset: 0x001745FB
		protected override void RegisterPorts()
		{
			this.flowInput = base.AddFlowInput("In", new FlowHandler(this.StartResurrection), "");
			this.flowOutput = base.AddFlowOutput("Out", "");
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x00176438 File Offset: 0x00174638
		private void StartResurrection(Flow flow)
		{
			List<WgoData> wgoDataList = MainGame.WorldData.GetWgoDataList("resurrection_table_1");
			PlayerData playerData = MainGame.PlayerData;
			foreach (WgoData wgoData in wgoDataList)
			{
				if (wgoData.GetGameResInt("resurrection_prepared") > 0)
				{
					CraftDef craftDef = GameBalance.GetCraftDef("corpse_zombie_transition");
					CraftElement craftElement = new CraftElement(craftDef.id, 1, new List<NeedItemData>(), new CraftParamsData(craftDef.id, new GameRes()));
					if (wgoData.CraftComponent.TryStartCraft(craftElement))
					{
						playerData.AddRes("cur_zombies_count", 1f);
					}
				}
			}
			if (playerData.CurrentWorldZoneData != null && playerData.CurrentWorldZoneData.Definition.id == "resurrection")
			{
				GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
			}
			playerData.SetRes("resurrection_has_power", 0f);
			this.flowOutput.Call(flow);
		}

		// Token: 0x040040CD RID: 16589
		private FlowInput flowInput;

		// Token: 0x040040CE RID: 16590
		private FlowOutput flowOutput;
	}
}
