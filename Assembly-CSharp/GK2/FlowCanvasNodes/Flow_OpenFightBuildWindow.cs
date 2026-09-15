using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C32 RID: 3122
	[Name("Open Fight Build Window", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_OpenFightBuildWindow : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004FB6 RID: 20406 RVA: 0x00177A18 File Offset: 0x00175C18
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.OpenFightBuildWindow), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x00177A68 File Offset: 0x00175C68
		private void OpenFightBuildWindow(Flow flow)
		{
			MilitaryBaseData militaryBaseData = MainGame.Instance.GameSave.militaryBaseData;
			LazySingleton<BuildManager>.Instance.TryEnable(GameScene.GetWgoViewGlobal(base.SelfWgoData.UniqueId), new Func<List<Inventory>>(militaryBaseData.CreateBaseBuildingsInventory));
			this.@out.Call(flow);
		}

		// Token: 0x0400412E RID: 16686
		private FlowInput @in;

		// Token: 0x0400412F RID: 16687
		private FlowOutput @out;
	}
}
