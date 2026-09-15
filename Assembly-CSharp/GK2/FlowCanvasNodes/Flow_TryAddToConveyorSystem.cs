using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C26 RID: 3110
	[Name("Try Add To Conveyor System", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_TryAddToConveyorSystem : GKCustomFlowNode
	{
		// Token: 0x06004F8F RID: 20367 RVA: 0x001770DC File Offset: 0x001752DC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.TryAddToConveyorSystem), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.wgoData = base.AddValueInput<WgoData>("wgoData", "");
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00177144 File Offset: 0x00175344
		private void TryAddToConveyorSystem(Flow flow)
		{
			if (this.wgoData.value != null)
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.wgoData.value.UniqueId);
				if (wgoViewGlobal != null)
				{
					ConveyorBuildPointer.TryMakeConnectionsOutsideBuildSystem(wgoViewGlobal);
				}
				else
				{
					ConveyorWgoData conveyorWgoData = this.wgoData.value as ConveyorWgoData;
					if (conveyorWgoData != null)
					{
						MainGame.Instance.conveyorSystem.AddConveyorObject(conveyorWgoData.ConveyorComponent);
					}
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004107 RID: 16647
		private FlowInput @in;

		// Token: 0x04004108 RID: 16648
		private FlowOutput @out;

		// Token: 0x04004109 RID: 16649
		private ValueInput<WgoData> wgoData;

		// Token: 0x0400410A RID: 16650
		private WgoData spawnedData;
	}
}
