using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C38 RID: 3128
	[Name("Show Town Building Window", 0)]
	[Category("Game/UI")]
	public class Flow_OpenTownBuildingWindow : GKCustomFlowNode
	{
		// Token: 0x06004FCE RID: 20430 RVA: 0x00177F94 File Offset: 0x00176194
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onBuildPressed = base.AddFlowOutput("onBuildPressed".CapitalizeFirst(), "");
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x00178000 File Offset: 0x00176200
		private void Show(Flow flow)
		{
			Flow_OpenTownBuildingWindow.<>c__DisplayClass6_0 CS$<>8__locals1 = new Flow_OpenTownBuildingWindow.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.flow = flow;
			this.buildOnWgoData = (this.isForLevelUp ? MainGame.WorldData.GetWgoData(base.SelfWgoData.LinkedToTownBuildingUniqueId) : base.SelfWgoData);
			CS$<>8__locals1.buildWindow = LazyUI.GetWindow<UITownBuildingWindow>();
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.buildOnWgoData.UniqueId);
			PlayerData playerData = MainGame.PlayerController.PlayerData;
			List<TownBuildingDef> list;
			if (!this.isForLevelUp)
			{
				list = this.buildOnWgoData.TownBuildingWgoComponent.GetAvailableBuildings(this.buildOnWgoData);
			}
			else
			{
				(list = new List<TownBuildingDef>()).Add(GameBalance.Me.GetData<TownBuildingDef>(this.buildOnWgoData.TownBuildingWgoComponent.TownBuildingDef.lvlUpId));
			}
			UITownBuildingWindowData uitownBuildingWindowData = new UITownBuildingWindowData(wgoViewGlobal, playerData, list, new Action<TownBuildingDef, List<NeedItemData>>(CS$<>8__locals1.<Show>g__OnBuildPressed|0));
			CS$<>8__locals1.buildWindow.Open(uitownBuildingWindowData);
			this.@out.Call(CS$<>8__locals1.flow);
		}

		// Token: 0x0400413C RID: 16700
		private FlowInput @in;

		// Token: 0x0400413D RID: 16701
		private FlowOutput @out;

		// Token: 0x0400413E RID: 16702
		private FlowOutput onBuildPressed;

		// Token: 0x0400413F RID: 16703
		public bool isForLevelUp;

		// Token: 0x04004140 RID: 16704
		private WgoData buildOnWgoData;
	}
}
