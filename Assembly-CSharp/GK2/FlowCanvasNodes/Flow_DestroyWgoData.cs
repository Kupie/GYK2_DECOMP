using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B8C RID: 2956
	[Name("Destroy WGOData", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_DestroyWgoData : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004D9B RID: 19867 RVA: 0x0016E024 File Offset: 0x0016C224
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DestroyWgoData), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.destroyList)
			{
				this.wgoDataList = base.AddValueInput<List<WgoData>>("wgoDataList", "");
				return;
			}
			base.RegisterPorts();
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0016E098 File Offset: 0x0016C298
		private void DestroyWgoData(Flow flow)
		{
			if (this.destroyList && this.wgoDataList != null)
			{
				int count = this.wgoDataList.value.Count;
				for (int i = 0; i < count; i++)
				{
					MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this.wgoDataList.value[0], true);
				}
			}
			else
			{
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(base.GetWgoData(), true);
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x0016E121 File Offset: 0x0016C321
		public override string name
		{
			get
			{
				return "Destroy WgoData" + (this.destroyList ? " List" : "");
			}
		}

		// Token: 0x04003E89 RID: 16009
		[FlowNode.GatherPortsCallbackAttribute]
		public bool destroyList;

		// Token: 0x04003E8A RID: 16010
		private FlowInput @in;

		// Token: 0x04003E8B RID: 16011
		private FlowOutput @out;

		// Token: 0x04003E8C RID: 16012
		private ValueInput<List<WgoData>> wgoDataList;
	}
}
