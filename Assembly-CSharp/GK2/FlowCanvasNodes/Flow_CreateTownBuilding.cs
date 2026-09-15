using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B88 RID: 2952
	[Name("Create Town Building", 0)]
	[Category("Game/UI")]
	public class Flow_CreateTownBuilding : GKCustomFlowNode
	{
		// Token: 0x06004D8C RID: 19852 RVA: 0x0016DC58 File Offset: 0x0016BE58
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Create), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.townBuildingId = base.AddValueInput<string>("townBuildingId".CapitalizeFirst(), "");
			this.buildOnWgoData = base.AddValueInput<WgoData>("buildOnWgoData".CapitalizeFirst(), "");
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0016DCE0 File Offset: 0x0016BEE0
		private void Create(Flow flow)
		{
			MainGame.Instance.GameSave.townSystem.StartTownBuildingCraftOnWgoFromScript(GameBalance.Me.GetData<TownBuildingDef>(this.townBuildingId.value), this.buildOnWgoData.value);
			this.@out.Call(flow);
		}

		// Token: 0x04003E78 RID: 15992
		private FlowInput @in;

		// Token: 0x04003E79 RID: 15993
		private FlowOutput @out;

		// Token: 0x04003E7A RID: 15994
		private ValueInput<string> townBuildingId;

		// Token: 0x04003E7B RID: 15995
		private ValueInput<WgoData> buildOnWgoData;
	}
}
